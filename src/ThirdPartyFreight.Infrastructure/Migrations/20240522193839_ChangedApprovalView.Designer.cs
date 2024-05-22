﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThirdPartyFreight.Infrastructure;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240522193839_ChangedApprovalView")]
    partial class ChangedApprovalView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Agreements.Agreement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AgreementType")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("SiteType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TPF_Agreements", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Approvals.Approval", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CompletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FirstApprovalEndUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FirstApprovalOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SecondApprovalEndUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SecondApprovalOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ThirdApprovalEndUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ThirdApprovalOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Approvals", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Audits.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Audits", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Carriers.Carrier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Carriers", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Documents.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Documents", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Envelopes.Envelope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AutoRespondReason")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("CompletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeliveredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EnvelopeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EnvelopeStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpiringOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("InitialSentOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastStatusChangedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SentOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("VoidReason")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("VoidedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Envelopes", null, t =>
                        {
                            t.HasTrigger("Updated_EnvelopeStatus_Trigger");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Notes.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("NoteType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Notes", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Sites.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SiteNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId");

                    b.ToTable("TPF_Sites", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Users.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TPF_Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "user:read"
                        });
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TPF_Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Registered"
                        });
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Users.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("TPF_RolePermissions", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 1
                        });
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IdentityId")
                        .IsUnique();

                    b.ToTable("TPF_Users", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.WorkflowTask.WorkFlowTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Approver")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("CompletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TPF_WorkflowTasks", (string)null);
                });

            modelBuilder.Entity("ThirdPartyFreight.Infrastructure.OutBox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TPF_OutboxMessages", (string)null);
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThirdPartyFreight.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Agreements.Agreement", b =>
                {
                    b.OwnsOne("ThirdPartyFreight.Domain.Agreements.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("AgreementId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CompanyName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CustomerEmail")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CustomerName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("CustomerNumber")
                                .HasColumnType("int");

                            b1.HasKey("AgreementId");

                            b1.ToTable("TPF_Agreements");

                            b1.WithOwner()
                                .HasForeignKey("AgreementId");
                        });

                    b.Navigation("ContactInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Approvals.Approval", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Audits.Audit", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ThirdPartyFreight.Domain.Audits.AuditInfo", "AuditInfo", b1 =>
                        {
                            b1.Property<Guid>("AuditId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateOnly?>("AuditCompleteDateUtc")
                                .HasColumnType("date");

                            b1.Property<DateOnly>("AuditDateUtc")
                                .HasColumnType("date");

                            b1.Property<bool>("IsAuditActive")
                                .HasColumnType("bit");

                            b1.HasKey("AuditId");

                            b1.ToTable("TPF_Audits");

                            b1.WithOwner()
                                .HasForeignKey("AuditId");
                        });

                    b.Navigation("AuditInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Carriers.Carrier", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ThirdPartyFreight.Domain.Carriers.CarrierInfo", "CarrierInfo", b1 =>
                        {
                            b1.Property<Guid>("CarrierId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CarrierAccount")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CarrierAddress")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CarrierName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("CarrierType")
                                .HasColumnType("int");

                            b1.HasKey("CarrierId");

                            b1.ToTable("TPF_Carriers");

                            b1.WithOwner()
                                .HasForeignKey("CarrierId");
                        });

                    b.Navigation("CarrierInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Documents.Document", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ThirdPartyFreight.Domain.Documents.Details", "DocumentDetails", b1 =>
                        {
                            b1.Property<Guid>("DocumentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DocumentData")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("DocumentName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

                            b1.HasKey("DocumentId");

                            b1.ToTable("TPF_Documents");

                            b1.WithOwner()
                                .HasForeignKey("DocumentId");
                        });

                    b.Navigation("DocumentDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Envelopes.Envelope", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Notes.Note", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Sites.Site", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Agreements.Agreement", null)
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ThirdPartyFreight.Domain.Shared.Address", "SiteAddress", b1 =>
                        {
                            b1.Property<Guid>("SiteId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("SiteId");

                            b1.ToTable("TPF_Sites");

                            b1.WithOwner()
                                .HasForeignKey("SiteId");
                        });

                    b.Navigation("SiteAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("ThirdPartyFreight.Domain.Users.RolePermission", b =>
                {
                    b.HasOne("ThirdPartyFreight.Domain.Users.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThirdPartyFreight.Domain.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
