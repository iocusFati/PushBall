using UnityEngine;

namespace CoinFolder
{
    public class Coin : MonoBehaviour
    {
        public CoinAnimator Animator { get; set; }
        public bool Triggered { get; set; }
    }
}