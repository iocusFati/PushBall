using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "PLayerData", menuName = "StaticData/PlayerData")]
    public class PlayerStaticData : ScriptableObject
    {
        [Header("Speed")]
        public float Speed;
        public float ReduceSpeedForTick;
        public float ReduceSpeedAfterCollision;

        [Header("Camera")] 
        public float RotationAngleX;
        public float RotationAngleY;
        public float Distance;
        public float OffsetY;

        [Header("Bounce")]
        public Vector3 ForwardScale;
        public Color OnHitColor;
        public float Duration = 0.09f;
        public float DurationKoeficient;

        [Header("ParticleSystem")]
        public float ParticleCountKoeficient;
        public float ParticlePosOffset;
        public float SmokeParticleDuration;

        [Header("Other")] 
        public float Radius;
        public float MinMovement;
    }
}