using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Projectile Config")]
    public sealed class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private float playerSpeed = 14f;
        [SerializeField] private float enemySpeed = 7f;
        [SerializeField] private int prewarmCount = 24;

        public float PlayerSpeed => playerSpeed;
        public float EnemySpeed => enemySpeed;
        public int PrewarmCount => prewarmCount;

        public void Validate()
        {
            if (!float.IsFinite(playerSpeed) || playerSpeed <= 0f)
                throw new InvalidOperationException($"ProjectileConfig.playerSpeed must be positive: {playerSpeed}");
            if (!float.IsFinite(enemySpeed) || enemySpeed <= 0f)
                throw new InvalidOperationException($"ProjectileConfig.enemySpeed must be positive: {enemySpeed}");
            if (prewarmCount < 0)
                throw new InvalidOperationException($"ProjectileConfig.prewarmCount must be nonnegative: {prewarmCount}");
        }
    }
}
