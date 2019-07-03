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

            if (_context.Images.Count() == 0)
            {
                _context.Images.Add(new Image { ImageName = "Test123.png", GeoTag = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
                _context.SaveChanges();
            }
        }

        // GET: api/Postcard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetPostcardImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Postcard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetPostcardImage(long id)
        {
            var todoItem = await _context.Images.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
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

                    // MailMessage mailMessage = new MailMessage();
                    // mailMessage.From = new MailAddress("fsm.expert@outlook.com");
                    // mailMessage.To.Add(Request.Form["sentEmailTo"].ToString());
                    // mailMessage.Subject = Request.Form["subject"].ToString();
                    // mailMessage.Body = Request.Form["body"].ToString();
                    // mailMessage.Attachments.Add(new Attachment(outputPath));

                    // SmtpClient client = new SmtpClient("smtp.sendgrid.net");
                    // client.UseDefaultCredentials = false;
                    // client.Credentials = new NetworkCredential("apikey", "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");
                    // client.Port = 587;
                    // client.Send(mailMessage);

                    var newImage = new Image {
                        ImageName = fileName,
                        GeoTag = "", 
                        CreatedOn = DateTime.Now, 
                        ModifiedOn = DateTime.Now
                    };

                    _context.Images.Add(newImage);
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Postcard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> ListImages()
        {
            return await _context.Images.ToListAsync();
        }
    }
}