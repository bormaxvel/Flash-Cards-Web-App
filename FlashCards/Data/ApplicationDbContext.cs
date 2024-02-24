using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlashCards.Models;
using Microsoft.AspNetCore.Identity;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(iul => new { iul.LoginProvider, iul.ProviderKey });

            modelBuilder.Entity<cardCollectionLink>()
                .HasKey(cc => cc.Id);
            modelBuilder.Entity<cardCollectionLink>()
                .HasOne(cc => cc.Card)
                .WithMany(c => c.CardCollectionLinks)
                .HasForeignKey(cc => cc.Id);
            modelBuilder.Entity<cardCollectionLink>()
                .HasOne(cc => cc.Collection)
                .WithMany(c => c.CardCollectionLinks)
                .HasForeignKey(cc => cc.Id);

            modelBuilder.Entity<userCollectionLink>()
                .HasKey(uc => uc.Id);
            modelBuilder.Entity<userCollectionLink>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCollectionLinks)
                .HasForeignKey(uc => uc.Id);
            modelBuilder.Entity<userCollectionLink>()
                .HasOne(uc => uc.Collection)
                .WithMany(c => c.UserCollectionLinks)
                .HasForeignKey(uc => uc.Id);

            //modelBuilder.Entity<Status>()
                //.HasKey(s => s.Id);
            modelBuilder.Entity<Status>()
                .HasOne(s => s.User)
                .WithMany(u => u.Statuses)
                .HasForeignKey(s => s.Id);
            modelBuilder.Entity<Status>()
                .HasOne(s => s.Card)
                .WithMany(c => c.Statuses)
                .HasForeignKey(s => s.CardId);
            base.OnModelCreating(modelBuilder);
        }

    }

}
