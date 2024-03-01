﻿// <auto-generated />
using System;
using MensaWebsite.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MensaWebsite.Migrations
{
    [DbContext(typeof(MenuContext))]
    [Migration("20240207073719_Persons_Update")]
    partial class Persons_Update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MensaAppKlassenBibliothek.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("MainCourse")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PricesPriceId")
                        .HasColumnType("int");

                    b.Property<string>("Starter")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("MenuId");

                    b.HasIndex("PricesPriceId");

                    b.ToTable("Menues");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.MenuPerson", b =>
                {
                    b.Property<int>("MenuPersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Activated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("InShoppingcart")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("OrderDate")
                        .HasColumnType("date");

                    b.Property<bool>("Payed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PersonEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("MenuPersonId");

                    b.HasIndex("MenuId");

                    b.HasIndex("PersonEmail");

                    b.ToTable("MenuPersons");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Person", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsTeacher")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Email");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.PriceForMenu", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("PriceStudent")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("PriceTeacher")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("PriceId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Menu", b =>
                {
                    b.HasOne("MensaAppKlassenBibliothek.PriceForMenu", "Prices")
                        .WithMany()
                        .HasForeignKey("PricesPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prices");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.MenuPerson", b =>
                {
                    b.HasOne("MensaAppKlassenBibliothek.Menu", "Menu")
                        .WithMany("MenuPersons")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MensaAppKlassenBibliothek.Person", "Person")
                        .WithMany("MenuPersons")
                        .HasForeignKey("PersonEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Menu", b =>
                {
                    b.Navigation("MenuPersons");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Person", b =>
                {
                    b.Navigation("MenuPersons");
                });
#pragma warning restore 612, 618
        }
    }
}
