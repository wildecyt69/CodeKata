using Microsoft.AspNetCore.Components;

namespace Core.Components
{
    public abstract class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Inject]
        protected IStringLocalization Loc { get; private set; }

        protected string GetLocalizationString(string resource)
        {
            if (!string.IsNullOrEmpty(resource) && resource.StartsWith('#'))
            {
                return Loc[resource.Trim('#')];
            }

            return resource;
        }

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }

        protected virtual void OnItemChanged(object sender, object e)
        {
            StateHasChanged();
        }
    }
}