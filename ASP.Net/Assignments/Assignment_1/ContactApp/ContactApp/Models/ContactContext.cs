using System.Data.Entity;

namespace ContactApp.Models
{

    public class ContactContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public ContactContext() : base("DefaultConnection") { }
    }
}