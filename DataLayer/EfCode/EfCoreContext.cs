using DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer.EfCode
{
    public  class EfCoreContext : DbContext
    {
        private readonly Guid _userId;
        public EfCoreContext(DbContextOptions<EfCoreContext> options, IUserIdService userIdService = null)
            : base(options)
        {
            _userId = userIdService?.GetUserId() ?? new ReplacementUserIdService().GetUserId();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<PriceOffer> PriceOffers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasKey(x => new { x.BookId, x.AuthorId });

        }
    }
}
