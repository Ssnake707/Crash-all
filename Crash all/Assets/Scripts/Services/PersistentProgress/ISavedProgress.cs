using Data;

namespace Services.PersistentProgress
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(DataGame progress);
    }
}