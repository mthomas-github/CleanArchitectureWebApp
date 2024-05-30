using ClosedXML.Excel;
using ThirdPartyFreight.Application.Abstractions.Excel;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Infrastructure.Excel;

public class ExcelService : IExcelService
{
    public string CreateRoutingGuide(List<RoutingGuideData> data)
    {
        using var workbook = new XLWorkbook();
        IXLWorksheet? worksheet = workbook.Worksheets.Add("Sheet1");

        // Set headers
        worksheet.Cell("A1").Value = "Customer Name";
        worksheet.Cell("A1").Style.Font.SetBold(true);

        worksheet.Cell("B1").Value = "Customer Number";
        worksheet.Cell("B1").Style.Font.SetBold(true);
        worksheet.Column("B").Style.Fill.BackgroundColor = XLColor.FromHtml("#E4DFEC");

        worksheet.Cell("C1").Value = "Site Number";
        worksheet.Cell("C1").Style.Font.SetBold(true);
        worksheet.Column("C").Style.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");

        worksheet.Cell("E1").Value = "Account Number";
        worksheet.Cell("E1").Style.Font.SetBold(true);

        worksheet.Column("D").Width = 96.75;

        const string billToHeader = "Send Freight Invoice To: ";
   

        for (int i = 0; i < data.Count; i++)
        {
            RoutingGuideData currentObj = data[i];
            
            string[] shipToSitesNumbers = currentObj.ShipToSites.Split(',');

            bool multiple = shipToSitesNumbers.Length > 1;

            string initialShipTo = shipToSitesNumbers[0];

            // Check Site Number Length
            if (i < 1)
            {
                string billTo =
                    $"{billToHeader}\n{currentObj.LtlAddress}\n{currentObj.LtlCity} {currentObj.LtlState} {currentObj.LtlZipcode}";

                worksheet.Cell("D2").Value = "0-150-- " + currentObj.SecondaryParcelCarrierName;

                if (string.IsNullOrEmpty(currentObj.SecondaryParcelCarrierName))
                {
                    worksheet.Cell("D3").Value = "151+ -- " + currentObj.LtlCarrierName;
                    worksheet.Cell("E3").Value = currentObj.LtlBillTo;
                }
                else
                {
                    worksheet.Cell("D3").Value = "2nd 0-150-- " + currentObj.SecondaryParcelCarrierName;
                    worksheet.Cell("E3").Value = currentObj.SecondaryParcelCarrierAcct;
                    worksheet.Cell("D4").Value = "151+ -- " + currentObj.LtlCarrierName;
                    worksheet.Cell("E4").Value = currentObj.LtlBillTo;
                }

                worksheet.Cell("D1").Value = billTo;
                worksheet.Cell("A2").Value = currentObj.CustomerName;
                worksheet.Cell("B2").Value = currentObj.CustomerNumber;
                worksheet.Cell("C2").Value = initialShipTo;
                worksheet.Cell("E2").Value = currentObj.ParcelCarrierAcct;
            }

            if (!multiple)
            {
                continue;
            }
            
            int startRow = 4;
            foreach (string shipToSiteNumber in shipToSitesNumbers)
            {
                if (Array.IndexOf(shipToSitesNumbers, shipToSiteNumber) <= 0)
                {
                    continue;
                }

                worksheet.Cell($"C{startRow}").Value = shipToSiteNumber;
                startRow++;
            }
        }

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        byte[] byteArray = memoryStream.ToArray();
        return Convert.ToBase64String(byteArray);
    }
}
