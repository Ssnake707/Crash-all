using System;

namespace Infrastructure.SceneLoaders
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
    }
}