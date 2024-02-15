using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlashCards.Models;

namespace FlashCards.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Card> Cards { get; set; }
        public DbSet<cardCollectionLink> CardCollectionLinks{ get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<userCollectionLink> UserCollectionLinks { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
    }
}
