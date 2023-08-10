using Gameplay.BreakdownSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.EntityEditor
{
    [CustomEditor(typeof(Entity))]
    public class EntityCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            SetId();
            FillStaticDataEntity();
        }

        private void SetId()
        {
            EditorGUILayout.Space(15);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Set id");
            if (GUILayout.Button("Set id for all object"))
            {
                Entity entity = (Entity)target;
                SerializedProperty _dataEntity = serializedObject.FindProperty("_dataEntity");
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(_dataEntity.objectReferenceInstanceIDValue);
                UnityEditor.AssetDatabase.RenameAsset(assetPath, entity.name);
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.EndVertical();
        }

        private void FillStaticDataEntity()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Static data entity");
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Fill static data entity"))
            {
                
            } 
            GUILayout.EndVertical();
            GUI.enabled = true;
        }
    }
}