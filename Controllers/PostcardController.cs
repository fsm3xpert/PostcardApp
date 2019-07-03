using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PostcardApp.Infrastructure.Logging;
using PostcardApp.Models;

namespace PostcardApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostcardController : ControllerBase
    {
        private readonly PostcardContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PostcardController(PostcardContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            try
            {
                var data = Request.Form["file"];
                var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var buffer = Convert.FromBase64String(base64Data);

                if (buffer.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + ".png";
                    string outputPath = _hostingEnvironment.ContentRootPath + "\\FileSystem\\Temp\\" + fileName;

                    using (var stream = new MemoryStream(buffer))
                    {
                        using (var output = new FileStream(outputPath, FileMode.Create))
                        {
                            stream.CopyTo(output);
                        }
                    }

                    Logger.WriteInfo("Upload Image temporary to File System.");

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SendEmail()
        {
            try
            {
                var data = Request.Form["file"];
                var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var buffer = Convert.FromBase64String(base64Data);

                if (buffer.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + ".png";
                    string outputPath = _hostingEnvironment.ContentRootPath + "\\FileSystem\\Images\\" + fileName;

                    using (var stream = new MemoryStream(buffer))
                    {
                        using (var output = new FileStream(outputPath, FileMode.Create))
                        {
                            stream.CopyTo(output);
                        }
                    }

                    Logger.WriteInfo("Image generated to File System.");

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("fsm.expert@outlook.com");
                    mailMessage.To.Add(Request.Form["sentEmailTo"].ToString());
                    mailMessage.Subject = Request.Form["subject"].ToString();
                    mailMessage.Body = Request.Form["body"].ToString();
                    mailMessage.Attachments.Add(new Attachment(outputPath));

                    SmtpClient client = new SmtpClient("smtp.sendgrid.net");
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("apikey", "XYZ");
                    client.Port = 587;
                    client.Send(mailMessage);

                    Logger.WriteInfo("Sent Postcard through Email.");

                    // var newImage = new Image {
                    //     ImageName = fileName,
                    //     GeoTag = "", 
                    //     CreatedOn = DateTime.Now, 
                    //     ModifiedOn = DateTime.Now
                    // };

                    // _context.Images.Add(newImage);
                    // _context.SaveChanges();

                    // Logger.WriteInfo("Save Image copy to Database.");

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> ListImages()
        {
            Logger.WriteInfo("Fetching list of Images");
            return await _context.Images.ToListAsync();
        }
    }
}
