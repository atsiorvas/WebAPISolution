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
    [Migration("20190110143753_Alert4")]
    partial class Alert4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Common.Data.Alert", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("alert_id")
                        .HasColumnType("BIGINT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Arguments")
                        .IsRequired()
                        .HasColumnName("arguments")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnName("date_created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSent")
                        .HasColumnName("date_sent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FromDate")
                        .HasColumnName("from_date");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ToDate")
                        .HasColumnName("to_date");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("alert","dbo");
                });

            modelBuilder.Entity("Common.Data.OrderAlert", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("order_alert_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AlertId")
                        .HasColumnName("alert_id");

                    b.Property<long>("OrderId")
                        .HasColumnName("order_id");

                    b.HasKey("Id");

                    b.HasIndex("AlertId");

                    b.HasIndex("OrderId");

                    b.ToTable("order_alert");
                });

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

            modelBuilder.Entity("Common.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("order_id")
                        .HasColumnType("BIGINT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateSent")
                        .HasColumnName("date_sent");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("order","dbo");
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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user","dbo");
                });

            modelBuilder.Entity("Common.Data.Alert", b =>
                {
                    b.HasOne("Common.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Common.AuditedEntity", "AuditedEntity", b1 =>
                        {
                            b1.Property<long>("AlertId")
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

                            b1.HasKey("AlertId");

                            b1.ToTable("alert","dbo");

                            b1.HasOne("Common.Data.Alert")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "AlertId")
                                .OnDelete(DeleteBehavior.Restrict);
                        });
                });

            modelBuilder.Entity("Common.Data.OrderAlert", b =>
                {
                    b.HasOne("Common.Data.Alert", "Alert")
                        .WithMany("OrderAlert")
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Common.Order", "Order")
                        .WithMany("OrderAlert")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Common.Notes", b =>
                {
                    b.HasOne("Common.User", "User")
                        .WithMany("Note")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

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

                            b1.HasKey("NotesId");

                            b1.ToTable("note","dbo");

                            b1.HasOne("Common.Notes")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "NotesId")
                                .OnDelete(DeleteBehavior.Restrict);
                        });
                });

            modelBuilder.Entity("Common.Order", b =>
                {
                    b.HasOne("Common.User", "User")
                        .WithMany("Order")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Common.AuditedEntity", "AuditedEntity", b1 =>
                        {
                            b1.Property<long>("OrderId")
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

                            b1.HasKey("OrderId");

                            b1.ToTable("order","dbo");

                            b1.HasOne("Common.Order")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "OrderId")
                                .OnDelete(DeleteBehavior.Restrict);
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

                            b1.HasKey("UserId");

                            b1.ToTable("user","dbo");

                            b1.HasOne("Common.User")
                                .WithOne("AuditedEntity")
                                .HasForeignKey("Common.AuditedEntity", "UserId")
                                .OnDelete(DeleteBehavior.Restrict);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
