using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Helper
{
    public class EntityCreator : MonoBehaviour
    {
        public List<DestroyedPieceCreator> DestroyedPieces;

        public void FillStaticDataEntity(StaticDataEntity dataEntity, Action onFinish)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            FillDestroyedPieces();
            StartCoroutine(RunPhysicsSteps(10, () => FinishCoroutine(dataEntity, onFinish)));

            if (rb != null)
                rb.isKinematic = false;
        }

        private void FinishCoroutine(StaticDataEntity dataEntity, Action onFinish)
        {
            DestroyedPiecesId[] destroyedPiecesIds = new DestroyedPiecesId[DestroyedPieces.Count];
            for (int i = 0; i < destroyedPiecesIds.Length; i++)
            {
                destroyedPiecesIds[i] = new DestroyedPiecesId();
                destroyedPiecesIds[i].Id = DestroyedPieces[i].Id;
                int[] idPieces = new int[DestroyedPieces[i].ConnectedTo.Count];
                
                for (int j = 0; j < idPieces.Length; j++) 
                    idPieces[j] = DestroyedPieces[i].ConnectedTo[j].Id;

                destroyedPiecesIds[i].IdPieces = idPieces;
            }

            dataEntity.DestroyedPiecesIds = destroyedPiecesIds;
            onFinish?.Invoke();
        }

        public void ClearAll()
        {
            foreach (DestroyedPieceCreator item in DestroyedPieces)
                Destroy(item);

            Destroy(this);
        }

        private void FillDestroyedPieces()
        {
            DestroyedPieces = new List<DestroyedPieceCreator>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                DestroyedPieceCreator destroyedPiece = child.gameObject.AddComponent<DestroyedPieceCreator>();
                destroyedPiece.Id = child.GetComponent<IDestroyedPiece>().Id;
                destroyedPiece.SetDefaultValue();
                Rigidbody rigidBody = child.gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                rigidBody.useGravity = false;
                DestroyedPieces.Add(destroyedPiece);
            }
        }

        private IEnumerator RunPhysicsSteps(int stepCount, Action onFinish)
        {
            for (int i = 0; i < stepCount; i++)
                yield return new WaitForFixedUpdate();

            foreach (DestroyedPieceCreator piece in DestroyedPieces)
                piece.MakeStatic();

            onFinish?.Invoke();
        }
    }
}