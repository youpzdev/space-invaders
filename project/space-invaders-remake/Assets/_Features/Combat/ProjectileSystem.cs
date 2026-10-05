using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class ProjectileSystem : MonoBehaviour
    {
        [Header("Prefab")]
        [Tooltip("Projectile prefab shared by player and enemy shots.")]
        [SerializeField] private Projectile prefab;

        private readonly HashSet<Projectile> active = new HashSet<Projectile>();
        private ProjectileConfig config;
        private Camera gameCamera;
        private bool playing;

        public event Action<Vector3, Faction> ShotFired;

        public void Initialize(ProjectileConfig projectileConfig, Camera camera)
        {
            if (projectileConfig == null)
            {
                throw new ArgumentNullException(nameof(projectileConfig));
            }

            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            if (prefab == null)
            {
                throw new InvalidOperationException("ProjectileSystem.prefab is missing");
            }

            projectileConfig.Validate();
            config = projectileConfig;
            gameCamera = camera;
            Pooling.Prewarm(prefab.gameObject, config.PrewarmCount);
        }

        public void StartPlaying()
        {
            if (config == null)
            {
                throw new InvalidOperationException("ProjectileSystem.Initialize must run before StartPlaying");
            }

            playing = true;
        }

        public void Fire(Vector3 position, Vector2 direction, Faction faction)
        {
            if (faction != Faction.Player && faction != Faction.Enemy)
            {
                throw new ArgumentOutOfRangeException(nameof(faction), faction, "Projectile faction must be Player or Enemy");
            }

            if (!playing)
            {
                return;
            }

            if (direction != Vector2.up && direction != Vector2.down)
            {
                throw new ArgumentException($"Projectile direction must be vertical: {direction}", nameof(direction));
            }

            GameObject instance = Pooling.Instantiate(prefab.gameObject, position, Quaternion.identity);
            if (instance.transform.parent == null)
            {
                DontDestroyOnLoad(instance);
            }

            Projectile projectile = instance.GetComponent<Projectile>();
            active.Add(projectile);
            float speed = faction == Faction.Player ? config.PlayerSpeed : config.EnemySpeed;
            projectile.Initialize(direction, faction, speed, gameCamera, Release);
            ShotFired?.Invoke(position, faction);
        }

        public void StopAndClear()
        {
            playing = false;
            foreach (Projectile projectile in new List<Projectile>(active))
            {
                if (projectile != null)
                {
                    projectile.ReturnToPool();
                }
            }

            active.Clear();
        }

        private void Release(Projectile projectile)
        {
            active.Remove(projectile);
            Pooling.Destroy(projectile.gameObject);
        }

        private void OnDestroy()
        {
            StopAndClear();
            if (prefab != null)
            {
                Pooling.Clear(prefab.gameObject);
            }
        }
    }
}
