using System.Data;
using Bogus;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Domain.Agreements;
using Dapper;

namespace ThirdPartyFreight.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();


        List<object?> agreements = [];

        for (int i = 0; i < 50; i++)
        {
            Status status = faker.PickRandom<Status>();
#pragma warning disable CA1305
            string? mdmTicket = status == Status.PendingReviewMdm ? faker.Random.Number(1000, 9999).ToString() : null;
#pragma warning restore CA1305
            var agreementId = Guid.NewGuid();
            string? email = faker.Person.Email;
            string? name = faker.Person.FullName;
            string? by = faker.Person.UserName;
            agreements.Add(new
            {
                Id = agreementId,
                CompanyName = faker.Company.CompanyName(),
                CustomerNumber = faker.Random.Number(1000, 9999),
                CustomerName = name,
                CustomerEmail = email,
                Status = status,
                AgreementType = faker.PickRandom<AgreementType>(),
                SiteType = faker.PickRandom<SiteType>(),
                CreatedBy = by,
                CreatedOnUtc = DateTime.UtcNow,
                MdmTicket = mdmTicket,
                SiteNumber = faker.Random.Number(1000, 9999),
                SiteStreet = faker.Address.StreetAddress(),
                SiteCity = faker.Address.City(),
                SiteState = faker.Address.State(),
                SiteZip = faker.Address.ZipCode(),
                AgreementId = agreementId
            });

        }

        const string sql = """
                           INSERT INTO TPF_Agreements
                           (Id, ContactInfo_CustomerNumber, ContactInfo_CompanyName, ContactInfo_CustomerName, ContactInfo_CustomerEmail, Ticket, Status, AgreementType, SiteType, CreatedOnUtc, CreatedBy)
                           VALUES(@Id, @CustomerNumber, @CompanyName, @CustomerName, @CustomerEmail, @MdmTicket, @Status, @AgreementType, @SiteType, @CreatedOnUtc, @CreatedBy);
                           """;

        const string sql2 = """
                            INSERT INTO TPF_Sites
                            (Id, AgreementId, SiteNumber, SiteAddress_Street, SiteAddress_City, SiteAddress_State, SiteAddress_ZipCode)
                            VALUES(@Id, @AgreementId, @SiteNumber, @SiteStreet, @SiteCity, @SiteState, @SiteZip);
                            """;

        const string testRecord = """
                            INSERT INTO TPF_Agreements
                            (Id, ContactInfo_CustomerNumber, ContactInfo_CompanyName, ContactInfo_CustomerName, ContactInfo_CustomerEmail, Status, AgreementType, SiteType, CreatedOnUtc, CreatedBy)
                            VALUES('BBEA553F-7C2F-4818-9669-650DB74DF39F', 1234, 'TestUser', 'TestUser', 'Test@Test.com', 1, 2, 2, GETDATE(), 'Test');
                            """;

     
        const string testRecordSite = """
                                  INSERT INTO TPF_Sites
                                  (Id, AgreementId, SiteNumber, SiteAddress_Street, SiteAddress_City, SiteAddress_State, SiteAddress_ZipCode)
                                  VALUES('EF765A0C-20B1-4D67-A718-1DBBC641047C', 'BBEA553F-7C2F-4818-9669-650DB74DF39F', '11111', '1234 No Where St', 'NoWhere', 'CA', '23456'),
                                  ('D0F28609-A175-48B3-9AE2-EDC8F8391AD3', 'BBEA553F-7C2F-4818-9669-650DB74DF39F', '11112', '1235 No Where St', 'NoWhere', 'CA', '23456');
                                  """;

        const string testRecordDocument = """
                                          INSERT INTO [TPF_Documents]
                                                     ([Id]
                                                     ,[AgreementId]
                                                     ,[DocumentDetails_DocumentName]
                                                     ,[DocumentDetails_DocumentData]
                                                     ,[DocumentDetails_Type])
                                               VALUES
                                                     ('B2976BC8-D5CB-4827-80AD-12301909B010', 
                                          		   'BBEA553F-7C2F-4818-9669-650DB74DF39F', 
                                          		   'Test Document Name', 
                                          		   'TESTADATE',
                                          		   1)
                                          """;

        const string testRecordNote = """
                                      INSERT INTO [TPF_Notes]
                                                 ([Id]
                                                 ,[AgreementId]
                                                 ,[Content]
                                                 ,[CreatedOnUtc]
                                                 ,[NoteType])
                                           VALUES
                                                 ('2791DFB1-E1AE-4A1B-8421-6624CF9912E9',
                                      		   'BBEA553F-7C2F-4818-9669-650DB74DF39F',
                                      		   'Test Note For Me',
                                      		   GETDATE(),
                                      		   1)
                                      """;

        const string testRecordEnvelope = """
                                          INSERT INTO [TPF_Envelopes]
                                                ([Id]
                                                ,[EnvelopeStatus]
                                                ,[AgreementId]
                                                ,[EnvelopeId]
                                                ,[CreatedOnUtc])
                                          VALUES
                                                ('F43FFD1B-DC67-46CC-BE02-D033C65A5395'
                                                ,2
                                                ,'BBEA553F-7C2F-4818-9669-650DB74DF39F'
                                                ,'A4B85E41-3D84-4986-BF02-9A3276E347CF'
                                                ,GETDATE())
                                          """;

        const string testRecordCarrier = """
                                          INSERT INTO [TPF_Carriers]
                                                     ([Id]
                                                     ,[AgreementId]
                                                     ,[CarrierInfo_CarrierName]
                                                     ,[CarrierInfo_CarrierAccount]
                                                     ,[CarrierInfo_CarrierType])
                                               VALUES
                                                ('C74AD015-00C1-426F-84D1-5411F5D94D93', 'BBEA553F-7C2F-4818-9669-650DB74DF39F', 'Test Carrier', '1234ABC', 1),
                                                ('A7DFA241-6DE2-4DE5-9C74-0E34F1707563', 'BBEA553F-7C2F-4818-9669-650DB74DF39F', 'Test Carrier 2', '5525ABC', 2)
                                          """;
        
        connection.Execute(sql, agreements);
        connection.Execute(sql2, agreements);
        
        // Define your SQL query
        const string query = "SELECT COUNT(*) FROM TPF_Agreements WHERE Id = 'BBEA553F-7C2F-4818-9669-650DB74DF39F'";

        // Execute the query
        int count = connection.QueryFirstOrDefault<int>(query, new { Id = "BBEA553F-7C2F-4818-9669-650DB74DF39F" });

        if (count != 0)
        {
            return;
        }

        connection.Execute(testRecord);
        connection.Execute(testRecordSite);
        connection.Execute(testRecordDocument);
        connection.Execute(testRecordNote);
        connection.Execute(testRecordEnvelope);
        connection.Execute(testRecordCarrier);



    }
}
