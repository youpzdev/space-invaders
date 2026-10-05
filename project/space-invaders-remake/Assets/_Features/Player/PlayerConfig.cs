using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [Header("Movement")]
        [Tooltip("Horizontal movement speed in world units per second.")]
        [SerializeField] private float moveSpeed = 8f;

        [Header("Shooting")]
        [Tooltip("Seconds between shots while Space is held.")]
        [SerializeField] private float fireInterval = 0.22f;

        [Header("Damage")]
        [Tooltip("Seconds of protection after a hit. Further hits do not extend it.")]
        [SerializeField] private float invulnerabilityDuration = 0.5f;

        public float MoveSpeed => moveSpeed;
        public float FireInterval => fireInterval;
        public float InvulnerabilityDuration => invulnerabilityDuration;

        public void Validate()
        {
            if (!float.IsFinite(moveSpeed) || moveSpeed <= 0f)
            {
                throw new InvalidOperationException($"PlayerConfig.moveSpeed must be positive: {moveSpeed}");
            }

            if (!float.IsFinite(fireInterval) || fireInterval <= 0f)
            {
                throw new InvalidOperationException($"PlayerConfig.fireInterval must be positive: {fireInterval}");
            }

            if (!float.IsFinite(invulnerabilityDuration) || invulnerabilityDuration < 0f)
            {
                throw new InvalidOperationException($"PlayerConfig.invulnerabilityDuration must be nonnegative: {invulnerabilityDuration}");
            }
        }
    }
}
