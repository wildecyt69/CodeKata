using Microsoft.AspNetCore.Components;

namespace Core.Components.Common;

public partial class DataGrid<TItem> where TItem : class
{
    [Parameter]
    public RenderFragment ColumnDefinitions { get; set; }

    [Parameter]
    public IEnumerable<TItem> Items { get; set; }
}