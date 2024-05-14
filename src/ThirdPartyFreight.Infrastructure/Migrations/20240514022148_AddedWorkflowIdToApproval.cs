using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedWorkflowIdToApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkFlowId",
                table: "TPF_Approvals",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkFlowId",
                table: "TPF_Approvals");
        }
    }
}
