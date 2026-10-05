using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Feedback Config")]
    public sealed class FeedbackConfig : ScriptableObject
    {
        [SerializeField] private int prewarmCount = 16;
        [SerializeField] private int hitParticles = 6;
        [SerializeField] private int deathParticles = 18;
        [SerializeField] private int playerDamageParticles = 14;
        [SerializeField] private float burstLifetime = 0.35f;
        [SerializeField] private float particleSize = 0.07f;
        [SerializeField] private float particleSpeed = 2.8f;
        [SerializeField] private Color hitColor = Color.white;
        [SerializeField] private Color deathColor = new Color(1f, 0.75f, 0.3f);
        [SerializeField] private Color playerColor = new Color(0.45f, 1f, 0.75f);
        [SerializeField] private Color enemyColor = new Color(1f, 0.5f, 0.35f);
        [SerializeField] private float flashDuration = 0.12f;
        [SerializeField] private float flashScale = 0.35f;
        [SerializeField] private float recoilDistance = 0.09f;
        [SerializeField] private float recoilDuration = 0.12f;
        [SerializeField] private float shotShake = 0.04f;
        [SerializeField] private float hitShake = 0.12f;
        [SerializeField] private float deathShake = 0.18f;
        [SerializeField] private float playerDamageShake = 0.25f;
        [SerializeField] private float maxCameraShake = 0.25f;
        [SerializeField] private float shakeDuration = 0.14f;
        [SerializeField] private float playerDamageDuration = 0.25f;
        [SerializeField] private int shakeVibrato = 12;
        [SerializeField] private float damageLightIntensity = 1.7f;
        [SerializeField] private Color damageBackgroundColor = new Color(0.15f, 0.025f, 0.04f);

        public int PrewarmCount => prewarmCount;
        public int HitParticles => hitParticles;
        public int DeathParticles => deathParticles;
        public int PlayerDamageParticles => playerDamageParticles;
        public float BurstLifetime => burstLifetime;
        public float ParticleSize => particleSize;
        public float ParticleSpeed => particleSpeed;
        public Color HitColor => hitColor;
        public Color DeathColor => deathColor;
        public Color PlayerColor => playerColor;
        public Color EnemyColor => enemyColor;
        public float FlashDuration => flashDuration;
        public float FlashScale => flashScale;
        public float RecoilDistance => recoilDistance;
        public float RecoilDuration => recoilDuration;
        public float ShotShake => shotShake;
        public float HitShake => hitShake;
        public float DeathShake => deathShake;
        public float PlayerDamageShake => playerDamageShake;
        public float MaxCameraShake => maxCameraShake;
        public float ShakeDuration => shakeDuration;
        public float PlayerDamageDuration => playerDamageDuration;
        public int ShakeVibrato => shakeVibrato;
        public float DamageLightIntensity => damageLightIntensity;
        public Color DamageBackgroundColor => damageBackgroundColor;

        public void Validate()
        {
            if (prewarmCount < 0) throw new InvalidOperationException($"FeedbackConfig.prewarmCount must be nonnegative: {prewarmCount}");
            if (hitParticles <= 0 || deathParticles <= 0 || playerDamageParticles <= 0)
                throw new InvalidOperationException($"FeedbackConfig particle counts must be positive: hitParticles={hitParticles}, deathParticles={deathParticles}, playerDamageParticles={playerDamageParticles}");
            Positive(burstLifetime, nameof(burstLifetime));
            Positive(particleSize, nameof(particleSize));
            Positive(particleSpeed, nameof(particleSpeed));
            Positive(flashDuration, nameof(flashDuration));
            Positive(flashScale, nameof(flashScale));
            Positive(recoilDistance, nameof(recoilDistance));
            Positive(recoilDuration, nameof(recoilDuration));
            Nonnegative(shotShake, nameof(shotShake));
            Nonnegative(hitShake, nameof(hitShake));
            Nonnegative(deathShake, nameof(deathShake));
            Nonnegative(playerDamageShake, nameof(playerDamageShake));
            Positive(maxCameraShake, nameof(maxCameraShake));
            Positive(shakeDuration, nameof(shakeDuration));
            Positive(playerDamageDuration, nameof(playerDamageDuration));
            Positive(damageLightIntensity, nameof(damageLightIntensity));
            if (shakeVibrato <= 0) throw new InvalidOperationException($"FeedbackConfig.shakeVibrato must be positive: {shakeVibrato}");
        }

        private static void Positive(float value, string field)
        {
            if (!float.IsFinite(value) || value <= 0f)
                throw new InvalidOperationException($"FeedbackConfig.{field} must be positive: {value}");
        }

        private static void Nonnegative(float value, string field)
        {
            if (!float.IsFinite(value) || value < 0f)
                throw new InvalidOperationException($"FeedbackConfig.{field} must be nonnegative: {value}");
        }
    }
}
