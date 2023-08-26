using System.Collections.Generic;
using System.Threading.Tasks;
using StaticData.Infrastructure;
using StaticData.Progression;
using StaticData.Weapon;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        Task LoadAsync();
        StaticDataScenes Scenes { get; }
        StaticDataLevels DataLevels { get; }
        BaseProgression DataPriceRotatingSpeed { get; }
        BaseProgression DataPriceSizeWeapon { get; }
        Dictionary<int, StaticDataWeapon> DataWeapons { get; }
    }
}