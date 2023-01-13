using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "Coin", menuName = "StaticData/CoinData")]
    public class CoinStaticData : ScriptableObject
    {
        [Header("Initial")]
        public float Radius;
        public float RotationAngleX;
        
        [Header("Idle")]
        public float IdleRotateByZ;
        public float MaxIdleHeight;
        [FormerlySerializedAs("IdleRaiseCoinBy")] public float IdleRaiseDuration;
        public float InitialHeight;


        [Header("OnPicked")] 
        public float OnPickedRotateByZ;
        public float MaxOnPickedHeight;
        [FormerlySerializedAs("OnPickedRaiseCoinBy")] public float onPickedRaiseDuration;
    }
}