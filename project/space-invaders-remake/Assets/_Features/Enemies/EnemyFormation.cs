using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class EnemyFormation : MonoBehaviour
    {
        private readonly List<Enemy> aliveEnemies = new List<Enemy>();
        private FormationConfig config;
        private DefeatZone defeatZone;
        private Action cancelDescent;

        public int AliveCount => aliveEnemies.Count;

        public event Action<int> EnemyRemoved;

        public void Initialize(Enemy[] enemies, FormationConfig settings, DefeatZone zone)
        {
            config = settings;
            defeatZone = zone;
            foreach (Enemy enemy in enemies)
            {
                aliveEnemies.Add(enemy);
                enemy.Died += OnEnemyDied;
            }
        }

        public void StartMoving()
        {
            StopMoving();
            cancelDescent = Timer.Repeat(config.DescentInterval, Descend, this);
        }

        private void Descend()
        {
            transform.position += Vector3.down * config.DescentStep;
            foreach (Enemy enemy in aliveEnemies)
            {
                defeatZone.CheckEnemy(enemy);
            }
        }

        public void StopMoving()
        {
            cancelDescent?.Invoke();
            cancelDescent = null;
        }

        public Enemy GetRandomAliveEnemy()
        {
            return aliveEnemies.Count == 0 ? null : aliveEnemies[UnityEngine.Random.Range(0, aliveEnemies.Count)];
        }

        private void OnEnemyDied(Enemy enemy)
        {
            if (!aliveEnemies.Remove(enemy))
            {
                return;
            }

            enemy.Died -= OnEnemyDied;
            EnemyRemoved?.Invoke(aliveEnemies.Count);
        }

        private void OnDestroy()
        {
            StopMoving();
            foreach (Enemy enemy in aliveEnemies)
            {
                if (enemy != null)
                {
                    enemy.Died -= OnEnemyDied;
                }
            }

            aliveEnemies.Clear();
        }
    }
}
