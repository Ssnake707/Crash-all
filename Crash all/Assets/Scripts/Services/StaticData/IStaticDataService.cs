using System.Threading.Tasks;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        Task LoadAsync();
    }
}