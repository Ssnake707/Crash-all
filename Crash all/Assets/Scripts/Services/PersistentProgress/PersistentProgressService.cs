using Data;

namespace Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public DataGame Progress { get; set; }
    }
}