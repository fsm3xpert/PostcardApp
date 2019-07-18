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
using Microsoft.EntityFrameworkCore;
using MimeTypes;
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

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + MimeTypeMap.GetExtension(file.ContentType);
                    string outputPath = _hostingEnvironment.ContentRootPath + "\\wwwroot\\temp\\" + fileName;

                    using (var output = new FileStream(outputPath, FileMode.Create))
                    {
                        file.CopyTo(output);
                    }

                    Logger.WriteInfo("Image uploaded to temporary location.");
                    return Ok(fileName);
                }
                else
                {
                    Logger.WriteWarning("The bad uploaded request is sent from client.");
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
                // Buffering the modified image from client
                var data = Request.Form["file"];
                var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var buffer = Convert.FromBase64String(base64Data);

                // Validating the image is uploaded or not
                if (buffer.Length > 0)
                {
                    string contentType = Regex.Match(data, @"data:(?<type>.+?);base64").Groups["type"].Value;
                    string fileName = Guid.NewGuid().ToString() + MimeTypeMap.GetExtension(contentType);
                    string outputPath = _hostingEnvironment.ContentRootPath + "\\FileSystem\\Images\\" + fileName;

                    // Move the image to File System
                    using (var stream = new MemoryStream(buffer))
                    {
                        using (var output = new FileStream(outputPath, FileMode.Create))
                        {
                            stream.CopyTo(output);
                        }
                    }

                    Logger.WriteInfo("Image generated to File System.");

                    // // Creating the email and attach the image file
                    // MailMessage mailMessage = new MailMessage();
                    // mailMessage.From = new MailAddress("fsm.expert@outlook.com");
                    // mailMessage.To.Add(Request.Form["sentEmailTo"].ToString());
                    // mailMessage.Subject = Request.Form["subject"].ToString();
                    // mailMessage.Body = Request.Form["body"].ToString();
                    // mailMessage.Attachments.Add(new Attachment(outputPath));

                    // // Sending the email through SMTP connection
                    // SmtpClient client = new SmtpClient("smtp.sendgrid.net");
                    // client.UseDefaultCredentials = false;
                    // client.Credentials = new NetworkCredential("apikey", "XYZ");
                    // client.Port = 587;
                    // client.Send(mailMessage);

                    // Logger.WriteInfo("Sent Postcard through Email.");

                    // Saving the image history to SQLite database
                    var newImage = new Image
                    {
                        ImageName = fileName,
                        GeoTag = Request.Form["geoTag"].ToString(),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    _context.Images.Add(newImage);
                    _context.SaveChanges();

                    Logger.WriteInfo("Save Image copy to Database.");

                    return Ok();
                }
                else
                {
                    Logger.WriteWarning("The bad request is sent from client.");
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
