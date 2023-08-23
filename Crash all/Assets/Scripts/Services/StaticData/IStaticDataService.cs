using System.Threading.Tasks;
using StaticData.Infrastructure;
using StaticData.Progression;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        Task LoadAsync();
        StaticDataScenes Scenes { get; }
        StaticDataLevels DataLevels { get; }
        BaseProgression DataPriceRotatingSpeed { get; }
        BaseProgression DataPriceSizeWeapon { get; }
    }
}