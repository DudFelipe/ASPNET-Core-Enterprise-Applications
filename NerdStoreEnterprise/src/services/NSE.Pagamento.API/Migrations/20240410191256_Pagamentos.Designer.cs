﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSE.Pagamento.API.Data;

#nullable disable

namespace NSE.Pagamento.API.Migrations
{
    [DbContext(typeof(PagamentosContext))]
    [Migration("20240410191256_Pagamentos")]
    partial class Pagamentos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NSE.Pagamento.API.Models.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TipoPagamento")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Pagamentos", (string)null);
                });

            modelBuilder.Entity("NSE.Pagamento.API.Models.Transacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BandeiraCartao")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CodigoAutorizacao")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("CustoTransacao")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DataTransacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NSU")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PagamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TID")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PagamentoId");

                    b.ToTable("Transacoes", (string)null);
                });

            modelBuilder.Entity("NSE.Pagamento.API.Models.Transacao", b =>
                {
                    b.HasOne("NSE.Pagamento.API.Models.Pagamento", "Pagamento")
                        .WithMany("Transacoes")
                        .HasForeignKey("PagamentoId")
                        .IsRequired();

                    b.Navigation("Pagamento");
                });

            modelBuilder.Entity("NSE.Pagamento.API.Models.Pagamento", b =>
                {
                    b.Navigation("Transacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
