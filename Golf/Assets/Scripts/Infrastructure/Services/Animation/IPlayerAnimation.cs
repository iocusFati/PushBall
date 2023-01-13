using UnityEngine;

namespace Infrastructure.Services.Animation
{
    public interface IPlayerAnimation : IService
    {
        public void Bounce(Vector3 hitPosition, float movementMagnitude);
        public void Spin(Vector3 direction, float distance, float speed);
    }
}