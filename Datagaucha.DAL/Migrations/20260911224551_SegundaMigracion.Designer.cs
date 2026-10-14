/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using System;
using Datagaucha.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Datagaucha.DAL.Migrations
{
    [DbContext(typeof(DatagauchaDbContext))]
    [Migration("20260911224551_SegundaMigracion")]
    partial class SegundaMigracion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.30")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Datagaucha.Domain.Auth.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("ImageId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Datagaucha.Domain.FileSystem.File", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StoragePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Files");

                    b.HasDiscriminator().HasValue("File");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Datagaucha.Domain.Post.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Datagaucha.Domain.Social.Follow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("FollowerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("FollowingId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FollowerId");

                    b.HasIndex("FollowingId");

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("Datagaucha.Domain.FileSystem.Image", b =>
                {
                    b.HasBaseType("Datagaucha.Domain.FileSystem.File");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Image");
                });

            modelBuilder.Entity("Datagaucha.Domain.Auth.User", b =>
                {
                    b.HasOne("Datagaucha.Domain.FileSystem.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Datagaucha.Domain.Post.Post", b =>
                {
                    b.HasOne("Datagaucha.Domain.FileSystem.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("Datagaucha.Domain.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Datagaucha.Domain.Social.Follow", b =>
                {
                    b.HasOne("Datagaucha.Domain.Auth.User", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowerId");

                    b.HasOne("Datagaucha.Domain.Auth.User", "Following")
                        .WithMany()
                        .HasForeignKey("FollowingId");

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });
#pragma warning restore 612, 618
        }
    }
}
