using System.Threading.Tasks;
using StaticData.Infrastructure;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        Task LoadAsync();
        StaticDataScenes Scenes { get; }
    }
}