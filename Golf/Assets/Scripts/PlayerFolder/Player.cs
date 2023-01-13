using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerFolder
{
    public class Player : MonoBehaviour, ICollision, IPlayerTrigger
    {
        public Transform Target;
        public Transform PlayerMeshRotation;
        public Transform PlayerMeshScale;
        public Transform PivotAdjuster;
        public MeshRenderer MeshRenderer;
        public ParticleSystem SmokeParticle;
        public Camera CameraFollower { get; set; }
        public PlayerCoinPicker CoinPicker { get; set; }

        public event Action<Collision> OnWallCollisionEnterAction;
        public event Action<Collider> OnCoinTriggerEnter;
        public event Action OnPlayerDestroy;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(TagHolder.Wall))
                OnWallCollisionEnterAction.Invoke(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagHolder.Coin)) 
                OnCoinTriggerEnter.Invoke(other);
        }

        private void OnDestroy() => 
            OnPlayerDestroy.Invoke();
    }
}