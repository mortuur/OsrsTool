using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OsrsTool.Domain.Entities;

namespace OsrsTool.Infrastructure.Configurations
{
    public class ItemPriceHistoryConfiguration : IEntityTypeConfiguration<ItemPriceHistory>
    {
        public void Configure(EntityTypeBuilder<ItemPriceHistory> builder)
        {
            builder.ToTable("ItemPriceHistory");
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Item)
                   .WithMany(i => i.PriceHistory)
                   .HasForeignKey(p => p.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.RecordedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
