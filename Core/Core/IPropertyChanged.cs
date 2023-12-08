using System.ComponentModel;
using System.Threading.Tasks;

namespace Core
{
    public interface IPropertyChanged : INotifyPropertyChanged
    {
        Task OnPropertyChanged(string propertyName);
    }
}