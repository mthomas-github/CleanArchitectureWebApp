using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Inital : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
#pragma warning disable IDE0053 // Use expression body for lambda expression
        migrationBuilder.CreateTable(
            name: "TPF_Agreements",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ContactInfo_CustomerNumber = table.Column<int>(type: "int", nullable: false),
                ContactInfo_CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ContactInfo_CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ContactInfo_CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Ticket = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Status = table.Column<int>(type: "int", nullable: false),
                AgreementType = table.Column<int>(type: "int", nullable: false),
                SiteType = table.Column<int>(type: "int", nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Agreements", x => x.Id);
            });
#pragma warning restore IDE0053 // Use expression body for lambda expression

#pragma warning disable IDE0053 // Use expression body for lambda expression
        migrationBuilder.CreateTable(
            name: "TPF_OutboxMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar", nullable: false),
                ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_OutboxMessages", x => x.Id);
            });
#pragma warning restore IDE0053 // Use expression body for lambda expression

        migrationBuilder.CreateTable(
            name: "TPF_Permissions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
#pragma warning disable IDE0053
            constraints: table =>
#pragma warning restore IDE0053
            {
                table.PrimaryKey("PK_TPF_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
#pragma warning disable IDE0053
            constraints: table =>
#pragma warning restore IDE0053
            {
                table.PrimaryKey("PK_TPF_Roles", x => x.Id);
            });

#pragma warning disable IDE0053 // Use expression body for lambda expression
        migrationBuilder.CreateTable(
            name: "TPF_Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                IdentityId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Users", x => x.Id);
            });
#pragma warning restore IDE0053 // Use expression body for lambda expression

        migrationBuilder.CreateTable(
            name: "TPF_Approvals",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                FirstApprovalOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                FirstApprovalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                SecondApprovalOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                SecondApprovalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                ThirdApprovalOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                ThirdApprovalEndUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Approvals", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Approvals_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Audits",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AuditInfo_AuditDateUtc = table.Column<DateOnly>(type: "date", nullable: false),
                AuditInfo_IsAuditActive = table.Column<bool>(type: "bit", nullable: false),
                AuditInfo_AuditCompleteDateUtc = table.Column<DateOnly>(type: "date", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Audits", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Audits_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Carriers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CarrierInfo_CarrierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CarrierInfo_CarrierAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CarrierInfo_CarrierType = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Carriers", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Carriers_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Documents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DocumentDetails_DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DocumentDetails_DocumentData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DocumentDetails_Type = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Documents", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Documents_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Envelopes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EnvelopeStatus = table.Column<int>(type: "int", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                InitialSentOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                SentOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastStatusChangedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                CompletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeliveredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                ExpiringOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                VoidedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                VoidReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                AutoRespondReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Envelopes", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Envelopes_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Notes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Content = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                NoteType = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Notes", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Notes_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_Sites",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AgreementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SiteNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                SiteAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SiteAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SiteAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SiteAddress_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_Sites", x => x.Id);
                table.ForeignKey(
                    name: "FK_TPF_Sites_TPF_Agreements_AgreementId",
                    column: x => x.AgreementId,
                    principalTable: "TPF_Agreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TPF_RolePermissions",
            columns: table => new
            {
                RoleId = table.Column<int>(type: "int", nullable: false),
                PermissionId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TPF_RolePermissions", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_TPF_RolePermissions_TPF_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "TPF_Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TPF_RolePermissions_TPF_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "TPF_Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleUser",
            columns: table => new
            {
                RolesId = table.Column<int>(type: "int", nullable: false),
                UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                table.ForeignKey(
                    name: "FK_RoleUser_TPF_Roles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "TPF_Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleUser_TPF_Users_UsersId",
                    column: x => x.UsersId,
                    principalTable: "TPF_Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

#pragma warning disable CA1861 // Avoid constant arrays as arguments
        migrationBuilder.InsertData(
            table: "TPF_Permissions",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1, "user:read" });
#pragma warning restore CA1861 // Avoid constant arrays as arguments

#pragma warning disable CA1861 // Avoid constant arrays as arguments
        migrationBuilder.InsertData(
            table: "TPF_Roles",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1, "Registered" });
#pragma warning restore CA1861 // Avoid constant arrays as arguments

        migrationBuilder.InsertData(
            table: "TPF_RolePermissions",
#pragma warning disable CA1861
            columns: new[] { "PermissionId", "RoleId" },
#pragma warning restore CA1861
            values: new object[] { 1, 1 });

        migrationBuilder.CreateIndex(
            name: "IX_RoleUser_UsersId",
            table: "RoleUser",
            column: "UsersId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Approvals_AgreementId",
            table: "TPF_Approvals",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Audits_AgreementId",
            table: "TPF_Audits",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Carriers_AgreementId",
            table: "TPF_Carriers",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Documents_AgreementId",
            table: "TPF_Documents",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Envelopes_AgreementId",
            table: "TPF_Envelopes",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Notes_AgreementId",
            table: "TPF_Notes",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_RolePermissions_PermissionId",
            table: "TPF_RolePermissions",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Sites_AgreementId",
            table: "TPF_Sites",
            column: "AgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Users_Email",
            table: "TPF_Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TPF_Users_IdentityId",
            table: "TPF_Users",
            column: "IdentityId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleUser");

        migrationBuilder.DropTable(
            name: "TPF_Approvals");

        migrationBuilder.DropTable(
            name: "TPF_Audits");

        migrationBuilder.DropTable(
            name: "TPF_Carriers");

        migrationBuilder.DropTable(
            name: "TPF_Documents");

        migrationBuilder.DropTable(
            name: "TPF_Envelopes");

        migrationBuilder.DropTable(
            name: "TPF_Notes");

        migrationBuilder.DropTable(
            name: "TPF_OutboxMessages");

        migrationBuilder.DropTable(
            name: "TPF_RolePermissions");

        migrationBuilder.DropTable(
            name: "TPF_Sites");

        migrationBuilder.DropTable(
            name: "TPF_Users");

        migrationBuilder.DropTable(
            name: "TPF_Permissions");

        migrationBuilder.DropTable(
            name: "TPF_Roles");

        migrationBuilder.DropTable(
            name: "TPF_Agreements");
    }
}
