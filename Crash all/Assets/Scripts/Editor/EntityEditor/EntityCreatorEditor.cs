using Gameplay.BreakdownSystem;
using StaticData.Entity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor.EntityEditor
{
    [CustomEditor(typeof(Entity))]
    public class EntityCreatorEditor : UnityEditor.Editor
    {
        private const string PathStaticDataEntity = "Assets/Static Data/Entity/";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
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
                AddDestroyedPiecesToChild(entity);
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
                Entity entity = (Entity)target;
                SetDataEntity(entity);
            } 
            GUILayout.EndVertical();
            GUI.enabled = true;
        }

        private void SetDataEntity(Entity entity)
        {
            serializedObject.Update();
            SerializedProperty dataEntity = serializedObject.FindProperty("_dataEntity");
            dataEntity.objectReferenceValue = FindOrCreateStaticDataEntity(entity);
            serializedObject.ApplyModifiedProperties();
        }

        private StaticDataEntity FindOrCreateStaticDataEntity(Entity entity)
        {
            StaticDataEntity staticDataEntity = 
                AssetDatabase.LoadAssetAtPath<StaticDataEntity>($"{PathStaticDataEntity}{entity.name}.asset");
            if (staticDataEntity == null)
                staticDataEntity = CreateStaticDataEntityAsset(entity);

            return staticDataEntity;
        }
        
        private StaticDataEntity CreateStaticDataEntityAsset(Entity entity)
        {
            string path = $"{PathStaticDataEntity}{entity.name}.asset";
            StaticDataEntity dataEntity = ScriptableObject.CreateInstance<StaticDataEntity>();
            AssetDatabase.CreateAsset(dataEntity, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return dataEntity;
        }

        private void AddDestroyedPiecesToChild(Entity entity)
        {
            for (int i = 0; i < entity.transform.childCount; i++)
            {
                Transform child = entity.transform.GetChild(i);
                if (child.TryGetComponent<DestroyedPiece>(out DestroyedPiece piece))
                {
                    SetIdPiece(piece, i);
                }
                else
                {
                    DestroyedPiece destroyedPiece = child.gameObject.AddComponent<DestroyedPiece>();
                    SetIdPiece(destroyedPiece, i);
                }

                if (child.TryGetComponent<MeshCollider>(out MeshCollider collider))
                {
                    collider.convex = true;
                }
                else
                {
                    MeshCollider meshCollider = child.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                }
            }
            
            void SetIdPiece(DestroyedPiece destroyedPiece, int id)
            {
                SerializedObject serializedObjectPiece = new SerializedObject(destroyedPiece);
                serializedObjectPiece.Update();
                SerializedProperty idPiece = serializedObjectPiece.FindProperty("_id");
                idPiece.intValue = id;
                serializedObjectPiece.ApplyModifiedProperties();
            }
        }
    }
}