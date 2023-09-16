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
        private const bool UsePlayerPrefs = false;
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
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (UsePlayerPrefs)
            {
                PlayerPrefs.SetString(_prefsKey, json);
                PlayerPrefs.Save();  
            }
            else
            {
                File.WriteAllText(_pathJson, json);    
            }
#else
            File.WriteAllText(_pathJson, json);
            Debug.Log($"Save data game to - {_pathJson}");
#endif
        }
        
        private DataGame LoadJson()
        {
            DataGame dataGame = null;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (UsePlayerPrefs)
            {
                string json = PlayerPrefs.GetString(_prefsKey, "");
                if (!string.IsNullOrEmpty(json))
                    dataGame = JsonUtility.FromJson<DataGame>(json);
            }
            else
            {
                dataGame = LoadFromFile();
            }
#else
            dataGame = LoadFromFile();
            Debug.Log($"Load data game to - {_pathJson}");
#endif

            return dataGame;
        }
        
        private DataGame LoadFromFile()
        {
            if (!File.Exists(_pathJson)) return null;
            string json = File.ReadAllText(_pathJson);
            return JsonUtility.FromJson<DataGame>(json);
        }
    }
}