namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public interface IModalGrid
{
    string GridName { get; set; }
    List<ModalGridColumn> Columns { get; set; }
    bool IsDocumentGrid { get; set; }
}

public sealed class ModalGrid<T> : IModalGrid
{
    public List<ModalGridColumn> Columns { get; set; }
    public List<T>? GridData { get; set; }
    public string GridName { get; set; }
    public bool IsDocumentGrid { get; set; }
}

