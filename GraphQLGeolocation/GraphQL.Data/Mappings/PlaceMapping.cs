using GraphQL.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQL.Data.Mappings
{
    public class PlaceMapping : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");
            
            builder.Property(c => c.Name)
                .HasColumnName("Name");
            
            builder.Property(c => c.LocationId)
                .HasColumnName("LocationId");
            
            builder.Property(c => c.Tags)
                .HasColumnName("Tags");

            builder.Property(c => c.Created)
                .HasColumnType("Date")
                .HasColumnName("Created");
            
            builder.Property(c => c.Modified)
                .HasColumnType("Date")
                .HasColumnName("Modified");
        }
    }
}