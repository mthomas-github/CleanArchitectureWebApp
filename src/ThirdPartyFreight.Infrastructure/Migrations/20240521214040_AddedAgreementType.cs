﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddedAgreementType : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
                             DROP VIEW VIEW_TPFApprovals
                             GO
                             CREATE VIEW [dbo].[View_TPFApprovals]
                             AS
                             SELECT
                                a.Id AS ApprovalId, 
                                wft.ExternalId AS TaskId, 
                                a.AgreementId, 
                                wft.Approver, 
                                a.FirstApprovalOnUtc AS FirstApprovalStart, 
                                a.FirstApprovalEndUtc AS FirstApprovalEnd, 
                                a.SecondApprovalOnUtc AS SecondApprovalStart, 
                                a.SecondApprovalEndUtc AS SecondApprovalEnd, 
                                a.ThirdApprovalOnUtc AS ThirdApprovalStart, 
                                a.ThirdApprovalEndUtc AS ThirdApprovalEnd, 
                                a.CompletedOn, 
                                ag.AgreementType, 
                                wft.ProcessId AS WorkFlowTaskId
                             FROM
                                dbo.TPF_Approvals AS a
                                INNER JOIN dbo.TPF_WorkflowTasks AS wft 
                                    ON a.TaskId = wft.ExternalId 
                                INNER JOIN dbo.TPF_Agreements AS ag 
                                    ON a.AgreementId = ag.Id
                             """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP VIEW View_TPFApprovals");
    }
}