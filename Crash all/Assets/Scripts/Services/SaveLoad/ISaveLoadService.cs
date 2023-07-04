using System.Collections.Generic;
using Data;
using Services.PersistentProgress;

namespace Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress(List<ISavedProgress> progressWriters);
        DataGame LoadProgress();
    }
}