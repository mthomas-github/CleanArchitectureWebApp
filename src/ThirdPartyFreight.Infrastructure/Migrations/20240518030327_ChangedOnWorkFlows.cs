using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangedOnWorkFlows : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Description",
            table: "TPF_WorkflowTasks");

        migrationBuilder.AddColumn<int>(
            name: "Approver",
            table: "TPF_WorkflowTasks",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql(@"DROP View View_TPFApprovals");
        migrationBuilder.Sql(@"    
        CREATE VIEW [dbo].[View_TPFApprovals] AS
                    SELECT
                        a.Id AS ApprovalId,
                        wft.ExternalId AS TaskId,
                        a.AgreementId AS AgreementId,
                        wft.[Approver],
                        a.FirstApprovalOnUtc AS FirstApprovalStart,
                        a.FirstApprovalEndUtc AS FirstApprovalEnd,
                        a.SecondApprovalOnUtc AS SecondApprovalStart,
                        a.SecondApprovalEndUtc AS SecondApprovalEnd,
                        a.ThirdApprovalOnUtc AS ThirdApprovalStart,
                        a.ThirdApprovalEndUtc AS ThirdApprovalEnd,
                        a.CompletedOn
                    FROM
                        TPF_Approvals a
                        JOIN TPF_WorkflowTasks wft
                        ON a.TaskId = wft.ExternalId;");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Approver",
            table: "TPF_WorkflowTasks");

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "TPF_WorkflowTasks",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql("DROP View View_TPFApprovals ");
    }
}
