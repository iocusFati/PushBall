using System;
using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.Particle
{
    public static class ParticleExtension
    {
        public static IEnumerator WaitForFinish(this ParticleSystem particle, float particleDuration, 
            Action onFinished)
        {
            float time = 0;
            while (particleDuration > time)
            {
                time += Time.deltaTime;
                yield return null;
            }
            
            onFinished?.Invoke();
        }
    }
}