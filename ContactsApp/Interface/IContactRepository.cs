using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp
{
    public interface IContactRepository : IRepository<ContactsDataModel>
    {
        PagedList<ContactsDataModel> GetContacts(ContactResourceParameters contactResourceParameters);
    }
}
