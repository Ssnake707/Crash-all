using System.Collections.Generic;
using Gameplay.BaseEntitiesController;
using Gameplay.BreakdownSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.EntityEditor
{
    [CustomEditor(typeof(EntitiesController))]
    public class EntitiesControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FindAllEntity();
            FindAllDestroyedPiecesWithoutEntity();
        }

        private void FindAllDestroyedPiecesWithoutEntity()
        {
            if (GUILayout.Button("Find all pieces without entity"))
            {
                EntitiesController entitiesController = (EntitiesController)target;
                SerializedProperty pieces = serializedObject.FindProperty("_destroyedPiece");
                serializedObject.Update();
                List<DestroyedPiece> _destroyedPieces = new List<DestroyedPiece>();
                SearchChild(ref _destroyedPieces, entitiesController.transform);

                pieces.arraySize = _destroyedPieces.Count;
                for (int i = 0; i < _destroyedPieces.Count; i++)
                    pieces.GetArrayElementAtIndex(i).objectReferenceValue = _destroyedPieces[i];
                
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void FindAllEntity()
        {
            if (GUILayout.Button("Find all entities"))
            {
                EntitiesController entitiesController = (EntitiesController)target;
                SerializedProperty entities = serializedObject.FindProperty("_entities");
                serializedObject.Update();
                List<Entity> listEntity = new List<Entity>();
                for (int i = 0; i < entitiesController.transform.childCount; i++)
                {
                    if (entitiesController.transform.GetChild(i).TryGetComponent(out Entity entity))
                        listEntity.Add(entity);
                }

                entities.arraySize = listEntity.Count;

                for (int i = 0; i < listEntity.Count; i++)
                    entities.GetArrayElementAtIndex(i).objectReferenceValue = listEntity[i];
                
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void SearchChild(ref List<DestroyedPiece> destroyedPieces, Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent(out Entity entity)) continue;
                if (parent.GetChild(i).TryGetComponent(out DestroyedPiece destroyedPiece))
                    destroyedPieces.Add(destroyedPiece);
                
                SearchChild(ref destroyedPieces, parent.GetChild(i));
            }
        }
    }
}