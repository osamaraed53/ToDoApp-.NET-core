using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.EF.Models;


namespace ToDoApp.EF.Data.config;

public class UserTasksConfigration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("UserTasks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasColumnType("varchar").HasMaxLength(225).IsRequired();
        builder.Property(x => x.TaskDescription).HasColumnType("varchar").HasMaxLength(2500);
        builder.Property(x => x.Status).HasColumnType("varchar").HasMaxLength(20);
        builder.Property(x => x.DueDate).HasColumnType("datetime2");
        builder.Property(x => x.Created_at).HasColumnType("datetime2").IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserTasks)
            .HasForeignKey(x => x.UserId).IsRequired();
    }
}




