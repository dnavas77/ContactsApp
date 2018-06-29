using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp
{
    public class ContactRepository : GenericRepository<ContactsDataModel>, IContactRepository
    {
        private readonly DbSet<ContactsDataModel> _dbSet;

        public ContactRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<ContactsDataModel>();
        }

        public PagedList<ContactsDataModel> GetContacts(ContactResourceParameters contactResourceParameters)
        {
            IQueryable<ContactsDataModel> query = _dbSet;

            return PagedList<ContactsDataModel>.Create(
                query, contactResourceParameters.PageNumber,
                contactResourceParameters.PageSize
            );
        }
    }
}
