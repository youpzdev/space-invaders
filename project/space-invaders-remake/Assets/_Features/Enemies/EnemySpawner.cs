using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class EnemySpawner
    {
        private readonly Enemy prefab;
        private readonly Transform parent;
        private readonly EnemyConfig enemyConfig;
        private readonly FormationConfig formationConfig;

        public EnemySpawner(Enemy prefab, Transform parent, EnemyConfig enemyConfig, FormationConfig formationConfig)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.enemyConfig = enemyConfig;
            this.formationConfig = formationConfig;
        }

        public Enemy[] SpawnGrid()
        {
            formationConfig.Validate();
            enemyConfig.Validate();
            if (prefab == null || parent == null)
                throw new InvalidOperationException("EnemySpawner: prefab and parent must be assigned.");
            var enemies = new Enemy[formationConfig.Rows * formationConfig.Columns];
            float startX = -(formationConfig.Columns - 1) * formationConfig.Spacing.x * 0.5f;
            for (int row = 0; row < formationConfig.Rows; row++)
            {
                for (int column = 0; column < formationConfig.Columns; column++)
                {
                    Enemy enemy = UnityEngine.Object.Instantiate(prefab, parent);
                    enemy.transform.localPosition = new Vector3(startX + column * formationConfig.Spacing.x,
                        -row * formationConfig.Spacing.y, 0f);
                    enemy.Initialize(enemyConfig.HitPoints, formationConfig.RowSprites[row % formationConfig.RowSprites.Length]);
                    enemies[row * formationConfig.Columns + column] = enemy;
                }
            }
            return enemies;
        }
    }
}
