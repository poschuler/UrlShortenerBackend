using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Api.Shared.Domain.Entities;

namespace UrlShortener.Api.Shared.Persistence.Configurations
{
    public sealed class ShortenedUrlConfiguration : IEntityTypeConfiguration<ShortenedUrl>
    {
        public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
        {
            builder.ToTable("shortened_urls");
            builder.HasKey(e => e.ShortCode);
            builder.Property(e => e.ShortCode)
                .HasColumnName("short_code")
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(e => e.LongUrl)
                .HasColumnName("long_url")
                .IsRequired();
            builder.Property(e => e.CreatedOnUtc)
                .HasColumnName("created_on_utc")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        }
    }
}
