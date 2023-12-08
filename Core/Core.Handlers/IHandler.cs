using System.Threading.Tasks;

namespace Core.Handlers
{
    public interface IHandler
    {
        bool IsInitialize { get; set; }
        Task Initialize();
    }
}