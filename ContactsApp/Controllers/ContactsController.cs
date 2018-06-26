using System;
using System.Collections.Generic;
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
        public ContactsDataModel Get(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Contacts.Find(id);
            }
        }

        // POST api/contacts/
        [HttpPost]
        public void Post()
        {
            using (var context = new AppDbContext())
            {
                context.Contacts.Add(new ContactsDataModel
                {
                    FirstName = "dani",
                    LastName = "navas",
                    Email = "dani@navas",
                    Phone = "7777777777"
                });
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
    }
}
