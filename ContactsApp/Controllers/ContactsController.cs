using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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
        public IActionResult Post([FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var context = new AppDbContext())
            {
                context.Contacts.Add(new ContactsDataModel
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone
                });
                context.SaveChanges();
            }
            return CreatedAtAction("Get", new { id = 33 });
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

            [MaxLength(256)]
            public string ProfilePicture { get; set; }

            [MaxLength(2000)]
            public string Comments { get; set; }
        }
    }
}
