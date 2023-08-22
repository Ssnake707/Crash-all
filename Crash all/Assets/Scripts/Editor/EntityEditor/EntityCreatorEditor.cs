using System.Collections.Generic;
using System.Linq;
using Gameplay.BreakdownSystem;
using Gameplay.BreakdownSystem.HelperForEditor;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor.EntityEditor
{
    [CustomEditor(typeof(Entity))]
    [CanEditMultipleObjects]
    public class EntityCreatorEditor : UnityEditor.Editor
    {
        private const string PathStaticDataEntity = "Assets/Static Data/Entity/";
        private const string _pathToEntitySettingsAsset = "Assets/Static data/Entity/EntitySettings.asset";
        private bool _isShowGraph;
        private bool _isShowGraphPlayMode;
        private Entity[] _entitiesTarget;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SetId();
            RenameChild();
            FillStaticDataEntity();
            AddRigidBody();
            CheckboxShowGraph();
        }

        public void OnSceneGUI()
        {
            DrawGraphFromStaticDataEntity();
            DrawGraphFromEntity();
        }

        private void OnEnable() =>
            _entitiesTarget = targets.Cast<Entity>().ToArray();

        private void DrawGraphFromEntity()
        {
            if (!Application.isPlaying) return;
            if (!_isShowGraphPlayMode) return;

            foreach (Entity entity in _entitiesTarget)
            {
                Color color = Color.green;
                Color colorEnd = Color.black;
                float step = 1;
                foreach (IDestroyedPiece item in entity.GetDestroyedPieces())
                {
                    Handles.color = Color.Lerp(color, colorEnd, step / entity.GetDestroyedPieces().Count);
                    Handles.DrawWireDisc(item.Transform.position, Vector3.forward, .2f, 2f);
                    Handles.Label(item.Transform.position, item.Id.ToString());

                    foreach (IDestroyedPiece destroyedPiece in item.ConnectedTo)
                        if (!destroyedPiece.IsDisconnect)
                            Handles.DrawLine(item.Transform.position, destroyedPiece.Transform.position, 2f);

                    step++;
                }
            }
        }

        private void DrawGraphFromStaticDataEntity()
        {
            if (!_isShowGraph) return;
            if (Application.isPlaying) return;
            foreach (Entity entity in _entitiesTarget)
            {
                var serializedObj = new SerializedObject(entity);
                SerializedProperty serializedDataEntity = serializedObj.FindProperty("_dataEntity");
                StaticDataEntity dataEntity = (StaticDataEntity)serializedDataEntity.objectReferenceValue;
                if (dataEntity == null) return;

                Dictionary<int, Transform> pieces = entity.transform.GetComponentsInChildren<DestroyedPiece>()
                    .ToDictionary((x) => x.Id, (x) => x.transform);

                Color color = Color.green;
                Color colorEnd = Color.black;
                float step = 1;
                foreach (DestroyedPiecesId item in dataEntity.DestroyedPiecesIds)
                {
                    Handles.color = Color.Lerp(color, colorEnd, step / dataEntity.DestroyedPiecesIds.Length);
                    Transform transform = pieces[item.Id];
                    Handles.DrawWireDisc(transform.position, Vector3.forward, .2f, 2f);
                    Handles.Label(transform.position, item.Id.ToString());
                    foreach (int idPiece in item.IdPieces)
                        Handles.DrawLine(pieces[item.Id].position, pieces[idPiece].position, 2f);

                    step++;
                }
            }
        }

        private void AddRigidBody()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Rigid body");
            if (GUILayout.Button("Create"))
            {
                foreach (Entity entity in _entitiesTarget)
                {
                    entity.transform.AddComponent<Rigidbody>();
                    EditorUtility.SetDirty(entity.transform);
                }
            }

            GUILayout.EndVertical();
        }

        private void CheckboxShowGraph()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Debug");
            if (Application.isPlaying)
            {
                _isShowGraph = false;
                GUI.enabled = false;
            }

            _isShowGraph = EditorGUILayout.Toggle("Show graph", _isShowGraph);
            GUI.enabled = true;

            if (!Application.isPlaying)
            {
                _isShowGraphPlayMode = false;
                GUI.enabled = false;
            }

            _isShowGraphPlayMode = EditorGUILayout.Toggle("Show graph", _isShowGraphPlayMode);
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        private void SetId()
        {
            EditorGUILayout.Space(15);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Pieces and id creator");
            if (GUILayout.Button("Create"))
            {
                foreach (Entity entity in _entitiesTarget)
                {
                    AddDestroyedPiecesToChild(entity);
                    EditorUtility.SetDirty(entity);
                }
            }

            GUILayout.EndVertical();
        }

        private void RenameChild()
        {
            EditorGUILayout.Space(15);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Rename child from id piece");
            if (GUILayout.Button("Rename child"))
            {
                foreach (Entity entity in _entitiesTarget)
                {
                    RenameChildFromPiecesId(entity);
                    EditorUtility.SetDirty(entity);
                }
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
                foreach (Entity entity in _entitiesTarget)
                {
                    SetDataEntity(entity);
                    FillStaticDataEntity(entity);
                }
            }

            GUILayout.EndVertical();
            GUI.enabled = true;
        }

        private void FillStaticDataEntity(Entity entity)
        {
            if (entity.TryGetComponent<Rigidbody>(out Rigidbody rb))
                Destroy(rb);

            SerializedObject serializedObj = new SerializedObject(entity);
            serializedObj.Update();
            SerializedProperty serializedDataEntity = serializedObj.FindProperty("_dataEntity");
            StaticDataEntity dataEntity = (StaticDataEntity)serializedDataEntity.objectReferenceValue;
            EntityCreator entityCreator = entity.transform.AddComponent<EntityCreator>();
            entityCreator.EntitySettings =
                AssetDatabase.LoadAssetAtPath<StaticDataEntitySettings>(_pathToEntitySettingsAsset);
            entityCreator.FillStaticDataEntity(dataEntity, () =>
            {
                EditorUtility.SetDirty(dataEntity);
                Debug.Log(
                    $"<color=green>Fill</color> " +
                    $"<color=yellow>{PathStaticDataEntity}{entity.name}.asset</color> <color=green>is SUCCESS</color>");
                entityCreator.ClearAll();
            });
        }

        private void SetDataEntity(Entity entity)
        {
            SerializedObject serializedObj = new SerializedObject(entity);
            serializedObj.Update();
            SerializedProperty dataEntity = serializedObj.FindProperty("_dataEntity");
            dataEntity.objectReferenceValue = FindOrCreateStaticDataEntity(entity);
            serializedObj.ApplyModifiedProperties();
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

        private void RenameChildFromPiecesId(Entity entity)
        {
            for (int i = 0; i < entity.transform.childCount; i++)
            {
                Transform child = entity.transform.GetChild(i);
                if (child.TryGetComponent<DestroyedPiece>(out DestroyedPiece piece))
                    child.gameObject.name = piece.Id.ToString();
            }
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