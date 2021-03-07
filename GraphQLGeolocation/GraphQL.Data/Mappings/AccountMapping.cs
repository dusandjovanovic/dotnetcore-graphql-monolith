using GraphQL.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQL.Data.Mappings
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");
            
            builder.Property(c => c.Name)
                .HasColumnName("Name");
            
            builder.Property(c => c.Email)
                .HasColumnName("Email");
            
            builder.Property(c => c.DateOfBirth)
                .HasColumnType("Date")
                .HasColumnName("DateOfBirth");
            
            builder.Property(c => c.Friends)
                .HasColumnName("Friends");
            
            builder.Property(c => c.AppearsIn)
                .HasColumnName("AppearsIn");
            
            builder.Property(c => c.SharedTags)
                .HasColumnName("SharedTags");
            
            builder.Property(c => c.Created)
                .HasColumnType("Date")
                .HasColumnName("Created");
            
            builder.Property(c => c.Modified)
                .HasColumnType("Date")
                .HasColumnName("Modified");
        }
    }
}