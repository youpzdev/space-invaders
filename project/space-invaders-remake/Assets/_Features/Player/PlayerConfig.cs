using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float fireInterval = 0.22f;
        [SerializeField] private float invulnerabilityDuration = 0.5f;

        public float MoveSpeed => moveSpeed;
        public float FireInterval => fireInterval;
        public float InvulnerabilityDuration => invulnerabilityDuration;

        public void Validate()
        {
            if (!float.IsFinite(moveSpeed) || moveSpeed <= 0f)
                throw new InvalidOperationException($"PlayerConfig.moveSpeed must be positive: {moveSpeed}");
            if (!float.IsFinite(fireInterval) || fireInterval <= 0f)
                throw new InvalidOperationException($"PlayerConfig.fireInterval must be positive: {fireInterval}");
            if (!float.IsFinite(invulnerabilityDuration) || invulnerabilityDuration < 0f)
                throw new InvalidOperationException($"PlayerConfig.invulnerabilityDuration must be nonnegative: {invulnerabilityDuration}");
        }
    }
}
