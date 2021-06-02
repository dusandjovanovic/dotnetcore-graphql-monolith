using GraphQL.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQL.Data.Mappings
{
    public class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");
            
            builder.Property(c => c.Description)
                .HasColumnName("Description");
            
            builder.Property(c => c.LocationId)
                .HasColumnName("LocationId");

            builder.Property(c => c.Created)
                .HasColumnType("Date")
                .HasColumnName("Created");
            
            builder.Property(c => c.Modified)
                .HasColumnType("Date")
                .HasColumnName("Modified");
        }
    }
}