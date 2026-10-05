using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField] private int hitPoints = 1;
        [SerializeField] private int scoreReward = 10;
        [SerializeField] private float minFireInterval = 0.7f;
        [SerializeField] private float maxFireInterval = 1.6f;

        public int HitPoints => hitPoints;
        public int ScoreReward => scoreReward;
        public float MinFireInterval => minFireInterval;
        public float MaxFireInterval => maxFireInterval;

        public void Validate()
        {
            if (hitPoints <= 0)
                throw new InvalidOperationException($"EnemyConfig.hitPoints must be positive: {hitPoints}");
            if (scoreReward < 0)
                throw new InvalidOperationException($"EnemyConfig.scoreReward must be nonnegative: {scoreReward}");
            if (!float.IsFinite(minFireInterval) || minFireInterval <= 0f)
                throw new InvalidOperationException($"EnemyConfig.minFireInterval must be positive: {minFireInterval}");
            if (!float.IsFinite(maxFireInterval) || maxFireInterval < minFireInterval)
                throw new InvalidOperationException($"EnemyConfig.maxFireInterval must be at least {minFireInterval}: {maxFireInterval}");
        }
    }
}
