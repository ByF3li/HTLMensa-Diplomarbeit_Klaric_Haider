﻿// <auto-generated />
using System;
using MensaWebAPI.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MensaWebAPI.Migrations
{
    [DbContext(typeof(MenuContext))]
    [Migration("20231208203627_shoppingcart")]
    partial class shoppingcart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
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

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Starter")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WhichMenu")
                        .HasColumnType("int");

                    b.HasKey("MenuId");

                    b.ToTable("Menues");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.MenuShoppingCartItem", b =>
                {
                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("int");

                    b.HasKey("MenuId", "ShoppingCartId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("MenuShoppingCartItems");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Order", b =>
                {
                    b.Property<int?>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("OrderDate")
                        .HasColumnType("date");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.ShoppingCart", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("MenuOrder", b =>
                {
                    b.Property<int>("MenusMenuId")
                        .HasColumnType("int");

                    b.Property<int>("OrdersOrderId")
                        .HasColumnType("int");

                    b.HasKey("MenusMenuId", "OrdersOrderId");

                    b.HasIndex("OrdersOrderId");

                    b.ToTable("MenuOrder");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.MenuShoppingCartItem", b =>
                {
                    b.HasOne("MensaAppKlassenBibliothek.Menu", "Menu")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MensaAppKlassenBibliothek.ShoppingCart", "ShoppingCart")
                        .WithMany("MenuItems")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("MenuOrder", b =>
                {
                    b.HasOne("MensaAppKlassenBibliothek.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenusMenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MensaAppKlassenBibliothek.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.Menu", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("MensaAppKlassenBibliothek.ShoppingCart", b =>
                {
                    b.Navigation("MenuItems");
                });
#pragma warning restore 612, 618
        }
    }
}
