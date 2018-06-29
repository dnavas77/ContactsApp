using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ContactsApp.Enums;

namespace ContactsApp.Controllers
{
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        const string FOLDER_NAME = "profile-pics";

        private readonly IHostingEnvironment _hostingEnv;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly AppDbContext _context;

        public ContactsController(
            IHostingEnvironment hostingEnv,
            IContactRepository contactRepository,
            IMapper mapper,
            IUrlHelper urlHelper,
            AppDbContext context
        )
        {
            _hostingEnv = hostingEnv;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _context = context;
        }

        // GET api/contacts
        [HttpGet]
        public IActionResult Get(ContactResourceParameters contactResourceParameters)
        {
            var contacts = _contactRepository.GetContacts(contactResourceParameters);
            var previousPageLink = contacts.HasPrevious ?
                CreateResourceUri(contactResourceParameters, ResourceUriType.Next)
                : null;

            var nextPageLink = contacts.HasNext ?
                CreateResourceUri(contactResourceParameters, ResourceUriType.Next)
                : null;

            var paginationMetadata = new
            {
                totalCount = contacts.TotalCount,
                pageSize = contacts.PageSize,
                currentPage = contacts.CurrentPage,
                totalPages = contacts.TotalPages,
                previousPage = previousPageLink,
                nextPage = nextPageLink
            };

            //Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(
                new
                {
                    contacts = _mapper.Map<IEnumerable<ContactViewModel>>(contacts),
                    pagination = paginationMetadata
                }
            );
        }

        // GET api/contacts/{id}
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            ContactsDataModel contact = _context.Contacts.Find(id);
            return Ok(contact);
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

            string _newId;
            ContactsDataModel newContact = new ContactsDataModel
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Comments = contact.Comments,
                Birthday = contact.Birthday,
                Groups = contact.Groups,
                ProfilePicture = fileName != "" ? FOLDER_NAME + "/" + fileName : null,
            };
            _contactRepository.Add(newContact);
            _contactRepository.SaveChangesAsync();
            _newId = newContact.ContactID;
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
            ContactsDataModel _found = _context.Contacts.Find(contact.ContactID);
            _found.FirstName = contact.FirstName;
            _found.LastName = contact.LastName;
            _found.Email = contact.Email;
            _found.Phone = contact.Phone;
            _found.Comments = contact.Comments;
            _found.Birthday = contact.Birthday;
            _found.Groups = contact.Groups;

            if (fileName != "")
            {
                _found.ProfilePicture = FOLDER_NAME + "/" + fileName;
            }
            _context.SaveChanges();
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
            ContactsDataModel contact = _context.Contacts.Find(id);
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return StatusCode(204);
        }

        private string CreateResourceUri(ContactResourceParameters contactResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.Previous:
                    return _urlHelper.Link("GetContacts",
                        new
                        {
                            pageNumber = contactResourceParameters.PageNumber - 1,
                            pageSize = contactResourceParameters.PageSize
                        });
                case ResourceUriType.Next:
                    return _urlHelper.Link("GetContacts",
                        new
                        {
                            pageNumber = contactResourceParameters.PageNumber - 1,
                            pageSize = contactResourceParameters.PageSize
                        });
                default:
                    return _urlHelper.Link("GetContacts",
                        new
                        {
                            pageNumber = contactResourceParameters.PageNumber,
                            pageSize = contactResourceParameters.PageSize
                        });
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

            public string[] Groups { get; set; }
        }
    }
}
