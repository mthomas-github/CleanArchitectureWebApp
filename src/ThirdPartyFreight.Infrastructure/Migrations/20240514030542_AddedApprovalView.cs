using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApprovalView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF OBJECT_ID('View_TPFApprovals', 'V') IS NOT NULL
                                    BEGIN
                                        DROP VIEW View_TPFApprovals;
                                    END");
            migrationBuilder.Sql(@"
                    CREATE VIEW View_TPFApprovals AS
                    SELECT
                        a.Id AS ApprovalId,
                        wft.ExternalId AS TaskId,
                        a.AgreementId AS AgreementId,
                        wft.[Description],
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
            migrationBuilder.Sql(@"DROP VIEW View_TPFApprovals;");
        }
    }
}
