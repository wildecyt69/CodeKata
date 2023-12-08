using System;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public interface IParamHandler<TParam> : IPropertyChanged, IHandler
    {
        TParam Parameter { get; }
        event EventHandler<object> ItemChanged;
        void OnItemChanged(object item);
        Task OnParameterSet();
        Task SetParameter(TParam param);
    }
}