﻿namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class ModalGrid
{
    public List<ModalGridColumn> Columns { get; set; }
    public List<object>? GridData { get; set; }
    public string GridName { get; set; }
}