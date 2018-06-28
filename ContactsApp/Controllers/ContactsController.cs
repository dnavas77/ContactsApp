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
        const string FOLDER_NAME = "profile-pics";
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
                return context.Contacts.OrderBy(c => c.FirstName).ToList();
            }
        }

        // GET api/contacts/{id}
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            using (var context = new AppDbContext())
            {
                ContactsDataModel contact = context.Contacts.Find(id);
                return Ok(contact);
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
            string fileName = saveImageIfExists(contact);

            string _newId = null;
            using (var context = new AppDbContext())
            {
                ContactsDataModel newContact = new ContactsDataModel
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Comments = contact.Comments,
                    Birthday = contact.Birthday,
                    ProfilePicture = fileName != "" ? FOLDER_NAME + "/" + fileName : null,
                };
                context.Contacts.Add(newContact);
                context.SaveChanges();
                _newId = newContact.ContactID;
            }
            return CreatedAtAction("Get", new { id = _newId });
        }


        // PUT api/contacts/
        [HttpPut]
        public IActionResult Put([FromForm]Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Save Image if exists
            //--------------------------------
            string fileName = saveImageIfExists(contact);

            // Update contact
            //--------------------------------
            using (var context = new AppDbContext())
            {
                ContactsDataModel _found = context.Contacts.Find(contact.ContactID);
                _found.FirstName = contact.FirstName;
                _found.LastName = contact.LastName;
                _found.Email = contact.Email;
                _found.Phone = contact.Phone;
                _found.Comments = contact.Comments;
                _found.Birthday = contact.Birthday;

                if (fileName != "")
                {
                    _found.ProfilePicture = FOLDER_NAME + "/" + fileName;
                }
                context.SaveChanges();
            }
            return StatusCode(204);
        }


        private string saveImageIfExists(Contact contact)
        {
            string fileName = "";
            if (contact.ProfilePicture != null)
            {
                try
                {
                    var file = contact.ProfilePicture;
                    string webRootPath = _hostingEnv.WebRootPath;
                    string newPath = Path.Combine(webRootPath, FOLDER_NAME);
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
                    throw new ApplicationException("Error saving image to server.");
                }
            }
            return fileName;
        }


        // DELETE api/contacts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            using (var context = new AppDbContext())
            {
                ContactsDataModel contact = context.Contacts.Find(id);
                context.Contacts.Remove(contact);
                context.SaveChanges();
                return StatusCode(204);
            }
        }

        public class Contact
        {
            public string ContactID { get; set; }

            [Required]
            [MaxLength(50)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(50)]
            public string LastName { get; set; }

            [Required]
            [MaxLength(256)]
            public string Email { get; set; }

            [MaxLength(20)]
            public string Phone { get; set; }

            [DataType(DataType.Date)]
            public DateTime? Birthday { get; set; }

            [MaxLength(2000)]
            public string Comments { get; set; }

            public IFormFile ProfilePicture { get; set; }
        }
    }
}
