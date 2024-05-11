using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThirdPartyFreight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingStoreProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarrierInfo_CarrierAddress",
                table: "TPF_Carriers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql(
                @"IF OBJECT_ID('', 'P') IS NOT NULL BEGIN DROP PROCEDURE TPF_GetCustomerActiveSites END");
            migrationBuilder.Sql(@"USE [csddevapps]
                                   GO
                                   /****** Object:  StoredProcedure [dbo].[TPF_GetCustomerActiveSites]    Script Date: 5/3/2024 10:06:49 PM ******/
                                   SET ANSI_NULLS ON
                                   GO
                                   SET QUOTED_IDENTIFIER ON
                                   GO
                                   -- =======================================================================
                                   -- Author:		Michael Thomas
                                   -- Create date: 5/3/2024
                                   -- Description:	Get All Action Sites For A Customer Returns a JSON Object
                                   -- =======================================================================
                                   CREATE PROCEDURE [dbo].[TPF_GetCustomerActiveSites]
                                   	@CustomerNumber VARCHAR(30)
                                   AS
                                   BEGIN
                                   
                                   	SET NOCOUNT ON;
                                   
                                   	DECLARE @Sites TABLE (
                                           CustomerNumber VARCHAR(30),
                                           Sites NVARCHAR(MAX)
                                       );
                                   
                                       INSERT INTO @Sites (CustomerNumber, Sites)
                                       SELECT
                                           cm.CUSTOMER_NUMBER AS 'CustomerNumber',
                                           (
                                               SELECT
                                                   SITE_NUMBER AS 'Site #',
                                                   CONCAT(
                                                       ISNULL(NULLIF(cm1.ADDRESS1, ''), ''),
                                                       CASE WHEN cm1.ADDRESS2 IS NOT NULL AND cm1.ADDRESS2 != '' THEN ', ' + cm1.ADDRESS2 ELSE '' END,
                                                       CASE WHEN cm1.ADDRESS3 IS NOT NULL AND cm1.ADDRESS3 != '' THEN ', ' + cm1.ADDRESS3 ELSE '' END,
                                                       CASE WHEN cm1.ADDRESS4 IS NOT NULL AND cm1.ADDRESS4 != '' THEN ', ' + cm1.ADDRESS4 ELSE '' END
                                                   ) AS 'SiteAddress',
                                   				  CONCAT(
                                                       ISNULL(NULLIF(cm1.ADDRESS1, ''), ''),
                                                       CASE WHEN cm1.ADDRESS2 IS NOT NULL AND cm1.ADDRESS2 != '' THEN ', ' + cm1.ADDRESS2 ELSE '' END,
                                                       CASE WHEN cm1.ADDRESS3 IS NOT NULL AND cm1.ADDRESS3 != '' THEN ', ' + cm1.ADDRESS3 ELSE '' END,
                                                       CASE WHEN cm1.ADDRESS4 IS NOT NULL AND cm1.ADDRESS4 != '' THEN ', ' + cm1.ADDRESS4 ELSE '' END,
                                   					CASE WHEN cm1.CITY IS NOT NULL AND cm1.CITY != '' THEN ', ' + cm1.CITY ELSE '' END,
                                   					CASE WHEN cm1.STATE IS NOT NULL AND cm1.STATE != '' THEN ', ' + cm1.STATE ELSE '' END,
                                   					CASE WHEN cm1.POSTAL_CODE IS NOT NULL AND cm1.POSTAL_CODE != '' THEN ', ' + cm1.POSTAL_CODE ELSE '' END
                                                   ) AS 'FullAddress',
                                   				ISNULL(cm1.CITY, '') AS City,
                                   				ISNULL(cm1.STATE, '') AS State,
                                   				ISNULL(cm1.POSTAL_CODE, '') AS ZipCode
                                               FROM
                                                   DIM_CustomerMaster cm1
                                               WHERE
                                                   cm1.CUSTOMER_NUMBER = cm.CUSTOMER_NUMBER
                                                   AND (cm1.SITE_USE_CODE = 'BILL_TO' OR cm1.SITE_USE_CODE = 'SHIP_TO')
                                                   AND cm1.SITE_USE_STATUS = 'A'
                                                   AND cm1.CUSTOMER_TYPE = 'R'
                                                   AND cm1.CUST_STATUS = 'A'
                                               FOR JSON PATH
                                           ) AS 'Sites'
                                       FROM
                                           DIM_CustomerMaster cm
                                       WHERE
                                           cm.CUSTOMER_NUMBER = @CustomerNumber
                                       GROUP BY
                                           cm.CUSTOMER_NUMBER;
                                   
                                       DECLARE @Json NVARCHAR(MAX);
                                   
                                       SELECT @Json = (
                                           SELECT
                                               CustomerNumber AS 'Customer #',
                                               JSON_QUERY(Sites) AS 'Sites'
                                           FROM
                                               @Sites
                                           FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                                       );
                                   	
                                   	SELECT @Json AS JsonOutput;
                                   END;");

            migrationBuilder.Sql(@"
                                     IF OBJECT_ID('View_TPFCustomerMaster', 'V') IS NOT NULL
                                     BEGIN
                                        DROP VIEW View_TPFCustomerMaster;
                                     END
                                     GO
                                    CREATE VIEW [dbo].[View_TPFCustomerMaster]
                                    AS
                                    SELECT DISTINCT CUSTOMER_NUMBER AS CustomerNumber, CUSTOMER_NAME AS CustomerName
                                    FROM   dbo.DIM_CustomerMaster
                                    WHERE (CUST_STATUS = 'A') AND (CUSTOMER_TYPE = 'R') AND (ORG_ID IN (782)) AND (SITE_USE_CODE = 'BILL_TO') AND (SITE_USE_STATUS = 'A')
                                    GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierInfo_CarrierAddress",
                table: "TPF_Carriers");

            migrationBuilder.Sql(@"USE [csddevapps]
                                   GO
                                   DROP PROCEDURE [dbo].[TPF_GetCustomerActiveSites]");

            migrationBuilder.Sql(@"DROP VIEW [dbo].[View_TPFCustomerMaster]");
        }
    }
}
