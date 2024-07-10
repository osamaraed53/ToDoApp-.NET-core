using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.EF.Models;

namespace ToDoApp.EF.Data.config;

public class UserConfigration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id).IsClustered();
        builder.Property(x => x.Username).HasColumnType("varchar").HasMaxLength(225).IsRequired();
        builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(225).IsRequired();
        builder.Property(x => x.HashPassword).HasColumnType("varchar").HasMaxLength(1024).IsRequired();
        builder.Property(x => x.Created_at).HasColumnType("datetime2").HasDefaultValue(DateTime.Now).IsRequired();
    }
}
