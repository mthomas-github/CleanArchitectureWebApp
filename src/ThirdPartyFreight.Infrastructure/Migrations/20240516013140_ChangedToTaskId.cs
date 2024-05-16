using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToTaskId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkFlowId",
                table: "TPF_Approvals");

            migrationBuilder.AddColumn<long>(
                name: "TaskId",
                table: "TPF_Approvals",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "TPF_Approvals");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFlowId",
                table: "TPF_Approvals",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
