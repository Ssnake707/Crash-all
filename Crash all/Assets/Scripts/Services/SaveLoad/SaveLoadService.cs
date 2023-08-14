using System.Collections.Generic;
using System.IO;
using Data;
using Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly string _pathJson;
        private readonly IPersistentProgressService _progressService;
        
        [Inject]
        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
#if (UNITY_ANDROID || IOS) && !UNITY_EDITOR
            _pathJson = Application.persistentDataPath + "/SaveData.json";
#else
            _pathJson = Application.dataPath + "/Resources/SaveData.json";
#endif
        }

        public void SaveProgress(List<ISavedProgress> progressWriters)
        {
            foreach (ISavedProgress progressWriter in progressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
            
            SaveJson();
        }

        public void SaveProgress() => 
            SaveJson();

        public DataGame LoadProgress() => 
            LoadJson();

        private void SaveJson()
        {
            string json = JsonUtility.ToJson(_progressService.Progress);
            File.WriteAllText(_pathJson, json);
#if UNITY_EDITOR
            Debug.Log($"Save data game to - {_pathJson}");
#endif
        }
        
        private DataGame LoadJson()
        {
            DataGame dataGame = null;
            if (File.Exists(_pathJson))
            {
                string json = File.ReadAllText(_pathJson);
                dataGame = JsonUtility.FromJson<DataGame>(json);
#if UNITY_EDITOR
                Debug.Log($"Load data game to - {_pathJson}");
#endif
            }

            return dataGame;
        }
    }
}