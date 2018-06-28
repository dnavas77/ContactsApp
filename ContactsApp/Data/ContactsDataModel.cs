using System;
using System.ComponentModel.DataAnnotations;

namespace ContactsApp
{
    public class ContactsDataModel
    {
        [Key]
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

        public DateTime? Birthday { get; set; }

        [MaxLength(256)]
        public string ProfilePicture { get; set; }

        [MaxLength(2000)]
        public string Comments { get; set; }
    }
}
