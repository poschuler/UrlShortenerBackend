using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Shared.Domain.Entities;

namespace UrlShortener.Api.Shared.Persistence
{
    public class UrlShortenerDbContext(
        DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {

        public DbSet<ShortenedUrl> ShortenedUrls => Set<ShortenedUrl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlShortenerDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
