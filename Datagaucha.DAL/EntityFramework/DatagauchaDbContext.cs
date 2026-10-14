/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.EntityFramework;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Social;
using Microsoft.EntityFrameworkCore;
namespace Datagaucha.DAL.EntityFramework;

public class DatagauchaDbContext(DbContextOptions<DatagauchaDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().ToTable("Users");

        modelBuilder.Entity<Datagaucha.Domain.Post.Post>()
            .HasMany(post => post.Comments)
            .WithOne(comment => comment.Post)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Person> People => Set<Person>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Datagaucha.Domain.FileSystem.File> Files => Set<Datagaucha.Domain.FileSystem.File>();
    public DbSet<Datagaucha.Domain.FileSystem.Image> Images => Set<Datagaucha.Domain.FileSystem.Image>();
    public DbSet<Datagaucha.Domain.Post.Post> Posts => Set<Datagaucha.Domain.Post.Post>();
    public DbSet<Datagaucha.Domain.Post.Comment> Comments => Set<Datagaucha.Domain.Post.Comment>();
    public DbSet<Datagaucha.Domain.Security.Ban> Bans => Set<Datagaucha.Domain.Security.Ban>();
}
