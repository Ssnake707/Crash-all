using Gameplay.BreakdownSystem;
using Gameplay.BreakdownSystem.HelperForEditor;
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
            GUILayout.Label("Components and id creator");
            if (GUILayout.Button("Create"))
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
            GUILayout.Label("Static data entity creator");
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Create"))
            {
                Entity entity = (Entity)target;
                SetDataEntity(entity);
                FillStaticDataEntity(entity);
            }

            GUILayout.EndVertical();
            GUI.enabled = true;
        }

        private void FillStaticDataEntity(Entity entity)
        {
            serializedObject.Update();
            SerializedProperty serializedDataEntity = serializedObject.FindProperty("_dataEntity");
            StaticDataEntity dataEntity = (StaticDataEntity)serializedDataEntity.objectReferenceValue;
            EntityCreator entityCreator = entity.transform.AddComponent<EntityCreator>();
            entityCreator.FillStaticDataEntity(dataEntity, () =>
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log(
                    $"<color=green>Fill</color> " +
                    $"<color=yellow>{PathStaticDataEntity}{entity.name}.asset</color> <color=green>is SUCCESS</color>");
                entityCreator.ClearAll();
            });
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