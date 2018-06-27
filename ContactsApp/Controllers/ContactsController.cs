using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private IHostingEnvironment _hostingEnv;

        public ContactsController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        // GET api/contacts
        [HttpGet]
        public IEnumerable<ContactsDataModel> Get()
        {
            using (var context = new AppDbContext())
            {
                return context.Contacts.ToList();
            }
        }

        // GET api/contacts/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var context = new AppDbContext())
            {
                ContactsDataModel contact = context.Contacts.Find(id);
                return Ok(new Contact {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone
                });
            }
        }

        // POST api/contacts/
        [HttpPost]
        public IActionResult Post([FromForm]Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Save Image
            string folderName = "profile-pics";
            string fileName = "";
            if (contact.ProfilePicture != null)
            {
                try
                {
                    var file = contact.ProfilePicture;
                    string webRootPath = _hostingEnv.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var milliseconds = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        fileName = milliseconds + fileName;
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            string _newId = null;
            using (var context = new AppDbContext())
            {
                ContactsDataModel newContact = new ContactsDataModel
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    ProfilePicture = fileName != "" ? folderName + "/" + fileName : null
                };
                context.Contacts.Add(newContact);
                context.SaveChanges();
                _newId = newContact.ContactID;
            }
            return CreatedAtAction("Get", new { id = _newId });
        }

        // PUT api/contacts/
        [HttpPut]
        public void Put([FromBody]Contact contact)
        {
            using (var context = new AppDbContext())
            {
                // Update contact
                context.SaveChanges();
            }
        }

        // DELETE api/contacts/{id}
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var context = new AppDbContext())
            {
                ContactsDataModel contact = context.Contacts.Find(id);
                context.Contacts.Remove(contact);
                context.SaveChanges();
                return "success";
            }
        }

        public class Contact
        {
            [Required]
            [MaxLength(50)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(50)]
            public string LastName { get; set; }

            [Required]
            [MaxLength(256)]
            public string Email { get; set; }

            [MaxLength(10)]
            public string Phone { get; set; }

            public DateTime Birthday { get; set; }

            [MaxLength(2000)]
            public string Comments { get; set; }

            public IFormFile ProfilePicture { get; set; }
        }
    }
}
