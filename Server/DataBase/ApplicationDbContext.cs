using System.Data.Entity;
using Server.DataBase.Models;

namespace Server.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(ConnectionStringBuilder.GetConnectionString())
        {
            Database.SetInitializer<ApplicationDbContext>(new DbInitializer());
        }        
        
        public DbSet<WordInfo> Words { get; set; }
    }
}
