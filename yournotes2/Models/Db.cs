using Microsoft.EntityFrameworkCore;
using yournotes.Models;

namespace yournotes.Models
{
    public class Db : Microsoft.EntityFrameworkCore.DbContext
    {
        public Db(DbContextOptions<Db> options)
            : base(options)
        {
        }
 
        public DbSet<User> Users { get; set; }
 
        public DbSet<Note> Notes { get; set; }
    }
}