using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Domain.UnitTests.Agreements;

internal static class AgreementData
{
    public static readonly ContactInfo ContactInfo = new
        (1111, "Test Labs", "TestName", "Test@Test.com");

    public static readonly Status Status = Status.Creating;
    
    public static readonly AgreementType AgreementType = AgreementType.Add;
    
    public static readonly SiteType SiteType = SiteType.Creating;
    
    public static readonly CreatedBy CreatedBy = new("TestName");
    
    public static readonly DateTime CreatedDate = DateTime.UtcNow;
}

