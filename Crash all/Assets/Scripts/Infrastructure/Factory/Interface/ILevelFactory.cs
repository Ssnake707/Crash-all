using System.Threading.Tasks;

namespace Infrastructure.Factory.Interface
{
    public interface ILevelFactory
    {
        void Init();
        void Cleanup();
        void SaveProgress();
        void WarmUp();
    }
}