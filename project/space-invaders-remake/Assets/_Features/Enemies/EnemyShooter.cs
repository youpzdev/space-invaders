using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class EnemyShooter : MonoBehaviour
    {
        private EnemyFormation formation;
        private ProjectileSystem projectiles;
        private EnemyConfig config;
        private Action cancelShot;
        private bool playing;

        public void Initialize(EnemyFormation army, ProjectileSystem shots, EnemyConfig settings)
        {
            formation = army;
            projectiles = shots;
            config = settings;
        }

        public void StartShooting()
        {
            StopShooting();
            playing = true;
            ScheduleShot();
        }

        private void ScheduleShot()
        {
            cancelShot = Timer.After(UnityEngine.Random.Range(config.MinFireInterval, config.MaxFireInterval), Shoot, this);
        }

        private void Shoot()
        {
            if (!playing)
            {
                return;
            }

            Enemy enemy = formation.GetRandomAliveEnemy();
            if (enemy == null)
            {
                return;
            }

            projectiles.Fire(enemy.FirePosition, Vector2.down, Faction.Enemy);
            ScheduleShot();
        }

        public void StopShooting()
        {
            playing = false;
            cancelShot?.Invoke();
            cancelShot = null;
        }

        private void OnDestroy() => StopShooting();
    }
}
