using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp
{
    public class ContactResourceParameters : PaginationResourceParams
    {
        public string FirstName { get; set; }
    }
}
