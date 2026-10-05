using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [Header("Health and Score")]
        [Tooltip("Hits required to destroy each enemy.")]
        [SerializeField] private int hitPoints = 1;

        [Tooltip("Points awarded once for each destroyed enemy.")]
        [SerializeField] private int scoreReward = 10;

        [Header("Shooting")]
        [Tooltip("Minimum seconds between shots from random surviving enemies.")]
        [SerializeField] private float minFireInterval = 0.7f;

        [Tooltip("Maximum seconds between shots. Must be at least the minimum interval.")]
        [SerializeField] private float maxFireInterval = 1.6f;

        public int HitPoints => hitPoints;
        public int ScoreReward => scoreReward;
        public float MinFireInterval => minFireInterval;
        public float MaxFireInterval => maxFireInterval;

        public void Validate()
        {
            if (hitPoints <= 0)
            {
                throw new InvalidOperationException($"EnemyConfig.hitPoints must be positive: {hitPoints}");
            }

            if (scoreReward < 0)
            {
                throw new InvalidOperationException($"EnemyConfig.scoreReward must be nonnegative: {scoreReward}");
            }

            if (!float.IsFinite(minFireInterval) || minFireInterval <= 0f)
            {
                throw new InvalidOperationException($"EnemyConfig.minFireInterval must be positive: {minFireInterval}");
            }

            if (!float.IsFinite(maxFireInterval) || maxFireInterval < minFireInterval)
            {
                throw new InvalidOperationException($"EnemyConfig.maxFireInterval must be at least {minFireInterval}: {maxFireInterval}");
            }
        }
    }
}
