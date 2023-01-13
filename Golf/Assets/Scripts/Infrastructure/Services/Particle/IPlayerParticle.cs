using UnityEngine;

namespace Infrastructure.Services.Particle
{
    public interface IPlayerParticle : IService
    {
        public void SmokeBurst(Vector3 collisionPoint, Vector3 collisionNormal, float bounceForce);
    }
}