using Data;

namespace Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        DataGame Progress { get; set; }   
    }
}