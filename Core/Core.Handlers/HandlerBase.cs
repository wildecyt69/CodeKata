using System;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public abstract class HandlerBase<TParam> : NotifyPropertyBase, IParamHandler<TParam>
    {
        private TParam _parameter;
        public event EventHandler<object> ItemChanged;

        public TParam Parameter
        {
            get => _parameter;
            private set => SetProperty(ref _parameter, value);
        }

        public bool IsInitialize { get; set; }

        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public void OnItemChanged(object item)
        {
            ItemChanged?.Invoke(this, item);
        }

        public virtual Task SetParameter(TParam param)
        {
            Parameter = param;
            return Task.CompletedTask;
        }

        public virtual Task OnParameterSet()
        {
            return Task.CompletedTask;
        }
    }
}