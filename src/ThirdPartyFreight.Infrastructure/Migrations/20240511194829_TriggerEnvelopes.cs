using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations;

/// <inheritdoc />
public partial class TriggerEnvelopes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"USE [csddevapps]
GO

/****** Object:  Trigger [Updated_EnvelopeStatus_Trigger]    Script Date: 5/11/2024 1:00:37 PM ******/
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'Updated_EnvelopeStatus_Trigger' AND parent_class = 0)
 BEGIN
    DROP TRIGGER [dbo].[Updated_EnvelopeStatus_Trigger]
 END
GO

/****** Object:  Trigger [dbo].[Updated_EnvelopeStatus_Trigger]    Script Date: 5/11/2024 1:00:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Michael Thomas
-- Create date: 2024-05-09
-- Description: This trigger fires after an update on the TPF_Envelopes table and performs actions based on the new status.
-- =============================================
CREATE TRIGGER [dbo].[Updated_EnvelopeStatus_Trigger]
   ON [dbo].[TPF_Envelopes]
   AFTER UPDATE
AS
BEGIN
   SET  NOCOUNT ON;

   -- Check if EnvelopeStatus was updated
   IF UPDATE (EnvelopeStatus)
      BEGIN
         -- Check if EnvelopeStatus is within expected range
         IF EXISTS
               (SELECT 1
                FROM inserted
                WHERE EnvelopeStatus NOT IN (1,
                                             2,
                                             5,
                                             6,
                                             8))
            BEGIN
               -- Update relevant columns based on EnvelopeStatus
               UPDATE e
               SET LastModifiedOnUtc = GETDATE (),
                   LastStatusChangedOnUtc = GETDATE (),
                   CompletedOnUtc =
                      CASE
                         WHEN i.EnvelopeStatus = 7 THEN GETDATE ()
                         ELSE NULL
                      END,
                   DeliveredOnUtc =
                      CASE
                         WHEN i.EnvelopeStatus = 4 THEN GETDATE ()
                         ELSE NULL
                      END
               FROM TPF_Envelopes e INNER JOIN inserted i ON e.Id = i.Id;

			   Update a
			   SET ModifiedBy = 'System',
				   ModifiedOnUtc = GETDATE(),
				   [Status] = CASE 
					WHEN i.EnvelopeStatus IN (5,6,7) 
					THEN 4 
					ELSE 
					CASE
					WHEN i.EnvelopeStatus = 8 
					THEN 9 
					ELSE a.SiteType END  
					END
			   FROM TPF_Agreements a
			   INNER JOIN inserted i ON a.Id = i.AgreementId
            END
      END
END;
GO

ALTER TABLE [dbo].[TPF_Envelopes] ENABLE TRIGGER [Updated_EnvelopeStatus_Trigger]
GO");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Updated_EnvelopeStatus_Trigger] GO");
    }
}
