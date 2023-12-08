using Microsoft.Extensions.Localization;

namespace Core.Components
{
    public class StringLocalization<TResource> : IStringLocalization where TResource : class
    {
        public StringLocalization(IStringLocalizer<TResource> stringLocalizer)
        {
            Loc = stringLocalizer;
        }

        private IStringLocalizer<TResource> Loc { get; }

        public string this[string name] => Loc[name];
    }
}