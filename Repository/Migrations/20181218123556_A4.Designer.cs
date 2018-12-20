﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

namespace Repository.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20181218123556_A4")]
    partial class A4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Common.Notes", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("note_id")
                        .HasColumnType("BIGINT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Lang")
                        .HasColumnName("lang")
                        .HasColumnType("TINYINT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("note","dbo");
                });

            modelBuilder.Entity("Common.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_id")
                        .HasColumnType("BIGINT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnName("first_name")
                        .HasMaxLength(100);

                    b.Property<bool>("IsAdmin")
                        .HasColumnName("is_admin");

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasMaxLength(100);

                    b.Property<bool>("RememberMe")
                        .HasColumnName("remember_me");

                    b.Property<string>("ResetAnswer")
                        .IsRequired()
                        .HasColumnName("reset_answer");

                    b.HasKey("Id");

                    b.ToTable("user","dbo");
                });

            modelBuilder.Entity("Common.Notes", b =>
                {
                    b.HasOne("Common.User", "User")
                        .WithMany("Note")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Common.AuditedEntity", "AuditedEntity", b1 =>
                        {
                            b1.Property<long>("NotesId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasColumnName("created_by")
                                .HasColumnType("nvarchar(100)");

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnName("created_on")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("UpdatedOn")
                                .HasColumnName("updated_on")
                                .HasColumnType("datetime2");

                            b1.ToTable("note","dbo");

                            b1.HasOne("Common.Notes")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "NotesId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Common.User", b =>
                {
                    b.OwnsOne("Common.AuditedEntity", "AuditedEntity", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasColumnName("created_by")
                                .HasColumnType("nvarchar(100)");

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnName("created_on")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("UpdatedOn")
                                .HasColumnName("updated_on")
                                .HasColumnType("datetime2");

                            b1.ToTable("user","dbo");

                            b1.HasOne("Common.User")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "UserId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
