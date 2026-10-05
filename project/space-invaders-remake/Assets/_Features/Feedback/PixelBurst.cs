using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class PixelBurst : MonoBehaviour
    {
        [Header("Particles")]
        [Tooltip("Particle system emitted once and returned to the pool when finished.")]
        [SerializeField] private ParticleSystem particles;

        private Action<PixelBurst> release;
        private bool active;

        public void Play(FeedbackConfig config, Color color, int count, Action<PixelBurst> returnToPool)
        {
            if (particles == null)
            {
                throw new InvalidOperationException("PixelBurst.particles is missing");
            }

            if (particles.gameObject != gameObject)
            {
                throw new InvalidOperationException("PixelBurst.particles must be on the same object as PixelBurst");
            }

            release = returnToPool;
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = particles.main;
            main.loop = false;
            main.playOnAwake = false;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startLifetime = config.BurstLifetime;
            main.startSpeed = config.ParticleSpeed;
            main.startSize = config.ParticleSize;
            main.startColor = color;
            main.stopAction = ParticleSystemStopAction.Callback;
            var emission = particles.emission;
            emission.enabled = false;
            active = true;
            particles.Play(true);
            particles.Emit(count);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public void ReturnToPool()
        {
            if (!active)
            {
                return;
            }

            active = false;
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var callback = release;
            release = null;
            callback(this);
        }

        private void OnParticleSystemStopped() => ReturnToPool();
    }
}
