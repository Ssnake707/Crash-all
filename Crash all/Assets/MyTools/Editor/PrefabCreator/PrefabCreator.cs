using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyTools.Editor.PrefabCreator
{
    public class PrefabCreator : EditorWindow
    {
        private const string WINDOW_TITLE = "Model Processor";
        private const string FIRST_LABEL_TEXT = "Target Folder";
        private const string USE_SELECTED_DIRECTORY_BUTTON_TEXT = "Use Selected Folder";
        private const string PROCESS_BUTTON_TEXT = "Process Model";

        public Object go;
        public Object folder;
        string prefabName = "Enter name";
        string createAt = "";
        public bool isZeroSmoothness;
        public bool hasTextures = false;
        public int additionalAnimations;
        public Object[] animations;

        [MenuItem("Tools/Art/Model Processor")]

        public static void ShowWindow() => 
            GetWindow<PrefabCreator>(false, WINDOW_TITLE, true);

        void OnGUI()
        {
            SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            serializedObject.Update();

            ShowAutoFolderButtons();

            if (folder != null && folder is DefaultAsset)
            {
                MainModel();

                if (go != null)
                {
                    string goOldName = go.name;

                    SettingsDraw();

                    string path = $"{createAt}/{prefabName}";

                    bool isPrefabExist = AssetDatabase.IsValidFolder(path);
                    if (!isPrefabExist)
                        CreateButton(path, goOldName);
                    else
                        EditorGUILayout.LabelField("Asset already exists", EditorStyles.boldLabel);

                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ExtractMaterials(string assetPath, string destinationPath, string prefix)
        {
            HashSet<string> hashSet = new HashSet<string>();
            IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                where x.GetType() == typeof(Material)
                select x;

            int id = 0;
            foreach (Object item in enumerable)
            {
                string path = System.IO.Path.Combine(destinationPath, $"{prefix}Material{id}") + ".mat";
                id++;
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                string value = AssetDatabase.ExtractAsset(item, path);
                if (string.IsNullOrEmpty(value))
                {
                    hashSet.Add(path);
                }
            }

            WriteAndImportAsset(assetPath);

            foreach (string item2 in hashSet)
            {
                WriteAndImportAsset(item2);
                Object asset = AssetDatabase.LoadAssetAtPath(item2, typeof(Material));
                if (isZeroSmoothness)
                    ((Material)asset).SetFloat("_Smoothness", 0f);
            }
        }

        private void ExtractAnimations(string assetPath, string destinationPath, string defaultName = "", bool isOneF = true)
        {
            HashSet<string> hashSet = new HashSet<string>();
            IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                where x.GetType() == typeof(AnimationClip)
                select x;

            bool isOne = enumerable.Count() <= 2 && isOneF;
            int id = 0;
            foreach (Object item in enumerable)
            {
                if (!item.name.Contains("__preview__"))
                {
                    AnimationClip ac = new AnimationClip();

                    EditorUtility.CopySerialized(item, ac);
                    if (string.IsNullOrEmpty(defaultName))
                    {
                        if (isOne)
                        {
                            if (id == 0)
                                AssetDatabase.CreateAsset(ac, $"{destinationPath}/{prefabName}.anim");
                            else
                                AssetDatabase.CreateAsset(ac, $"{destinationPath}/{prefabName}{id}.anim");
                        }
                        else
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                    }
                    else
                    {
                        if (isOne)
                        {
                            if (id == 0)
                                AssetDatabase.CreateAsset(ac, $"{destinationPath}/{defaultName}.anim");
                            else
                                AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                        }
                        else
                        {
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                        }
                    }
                    id++;
                }
            }
        }

        public void ExtractTextures(string assetPath, string destinationPath)
        {
            HashSet<string> hashSet = new HashSet<string>();
            IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                where x.GetType() == typeof(Texture)
                select x;

            foreach (Object item in enumerable)
            {
                string path = System.IO.Path.Combine(destinationPath, item.name) + $".{AssetDatabase.GetAssetPath(item).Split('.')[1]}";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                string value = AssetDatabase.ExtractAsset(item, path);
                if (string.IsNullOrEmpty(value))
                {
                    hashSet.Add(assetPath);
                }
            }

            foreach (string item2 in hashSet)
                WriteAndImportAsset(item2);
        }

        private static void WriteAndImportAsset(string assetPath)
        {
            AssetDatabase.WriteImportSettingsIfDirty(assetPath);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        private void ShowAutoFolderButtons()
        {
            GUILayout.Label(FIRST_LABEL_TEXT, EditorStyles.boldLabel);
            folder = EditorGUILayout.ObjectField(folder, typeof(Object), true);
            createAt = AssetDatabase.GetAssetPath(folder);

            if (GUILayout.Button(USE_SELECTED_DIRECTORY_BUTTON_TEXT))
            {
                Object autoObj = AssetDatabase.LoadAssetAtPath(GetCurrentlyOpenedDirectoryPath(), typeof(Object));

                if (autoObj != null)
                {
                    folder = autoObj;
                }
            }
        }

        private void MainModel()
        {
            GUILayout.Space(20);
            GUILayout.Label("Object Main Model", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            go = EditorGUILayout.ObjectField(go, typeof(Object), true);

            if (EditorGUI.EndChangeCheck())
            {
                prefabName = go.name;
            }
        }

        private void SettingsDraw()
        {
            GUILayout.Space(20);
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            prefabName = EditorGUILayout.TextField("Model Name", prefabName);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Is Zero Material Smoothness", EditorStyles.label);
            isZeroSmoothness = EditorGUILayout.Toggle(isZeroSmoothness);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Has External Textures", EditorStyles.label);
            hasTextures = EditorGUILayout.Toggle(hasTextures);
            GUILayout.EndHorizontal();
        }

        private void CreateButton(string path, string goOldName)
        {
            if (GUILayout.Button(PROCESS_BUTTON_TEXT))
            {
                AssetDatabase.CreateFolder($"{createAt}", prefabName);
                string modelPath = AssetDatabase.GetAssetPath(go);
                string modelExtension = modelPath.Split('.')[1];
                Debug.Log($"Move from {modelPath} to {path}");
                string movedModelPath = $"{path}/{prefabName}_Model.{modelExtension}";
                AssetDatabase.MoveAsset(modelPath, movedModelPath);

                AssetDatabase.CreateFolder($"{path}", "Materials");
                ExtractMaterials(movedModelPath, $"{path}/Materials", prefabName);
                AssetDatabase.CreateFolder($"{path}", "Animations");
                ExtractAnimations(movedModelPath, $"{path}/Animations", goOldName);

                if (animations != null)
                {
                    foreach (Object additionalAnim in animations)
                    {
                        if (additionalAnim != null)
                        {
                            ExtractAnimations(AssetDatabase.GetAssetPath(additionalAnim), $"{path}/Animations", additionalAnim.name, false);
                        }
                    }
                }

                try
                {
                    AssetDatabase.CreateFolder($"{path}", "Textures");
                    ExtractTextures(movedModelPath, $"{path}/Textures");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Texture bug: {e}");
                }

                TryDeleteEmptyDirectory($"{path}/Animations");

                if (!hasTextures)
                {
                    TryDeleteEmptyDirectory($"{path}/Textures");
                }

                AssetDatabase.Refresh();
            }
        }

        private bool TryDeleteEmptyDirectory(string directoryPath)
        {
            if (System.IO.Directory.GetFiles(directoryPath).Length == 0)
            {
                FileUtil.DeleteFileOrDirectory(directoryPath);
                FileUtil.DeleteFileOrDirectory($"{directoryPath}.meta");
                return true;
            }

            return false;
        }

        private string GetCurrentlyOpenedDirectoryPath()
        {
            System.Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
}