﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp
{
    public class ContactsDataModel
    {
        private static readonly char delimiter = ',';

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

        private string _groups;

        [NotMapped]
        public string[] Groups {
            get { return _groups != null ? _groups.Split(delimiter) : null;  }
            set
            {
                _groups = value != null ? string.Join($"{delimiter}", value) : null;
            }
        }
    }
}
