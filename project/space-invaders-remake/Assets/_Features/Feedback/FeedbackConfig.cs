using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Feedback Config")]
    public sealed class FeedbackConfig : ScriptableObject
    {
        [Header("Pooling")]
        [Tooltip("Number of bursts and flashes prepared for each effect prefab.")]
        [SerializeField] private int prewarmCount = 16;

        [Header("Particles")]
        [Tooltip("Particles emitted when an enemy is hit.")]
        [SerializeField] private int hitParticles = 6;

        [Tooltip("Additional particles emitted when an enemy dies.")]
        [SerializeField] private int deathParticles = 18;

        [Tooltip("Particles emitted when the player loses health.")]
        [SerializeField] private int playerDamageParticles = 14;

        [Tooltip("Particle lifetime in seconds.")]
        [SerializeField] private float burstLifetime = 0.35f;

        [Tooltip("Particle size in world units.")]
        [SerializeField] private float particleSize = 0.07f;

        [Tooltip("Initial particle speed in world units per second.")]
        [SerializeField] private float particleSpeed = 2.8f;

        [Header("Colors")]
        [Tooltip("Color of particles emitted on an enemy hit.")]
        [SerializeField] private Color hitColor = Color.white;

        [Tooltip("Color of particles emitted on an enemy death.")]
        [SerializeField] private Color deathColor = new Color(1f, 0.75f, 0.3f);

        [Tooltip("Color of player shots, trails, flashes and damage particles.")]
        [SerializeField] private Color playerColor = new Color(0.45f, 1f, 0.75f);

        [Tooltip("Color of enemy shots, trails and flashes.")]
        [SerializeField] private Color enemyColor = new Color(1f, 0.5f, 0.35f);

        [Header("Muzzle Flash")]
        [Tooltip("Seconds before the flash fades and returns to the pool.")]
        [SerializeField] private float flashDuration = 0.12f;

        [Tooltip("Target local scale of the muzzle flash.")]
        [SerializeField] private float flashScale = 0.35f;

        [Header("Recoil")]
        [Tooltip("Downward displacement of the player visual in local units. Does not move the collider.")]
        [SerializeField] private float recoilDistance = 0.09f;

        [Tooltip("Seconds for each leg of the recoil and return animation.")]
        [SerializeField] private float recoilDuration = 0.12f;

        [Header("Camera Shake")]
        [Tooltip("Camera displacement for a player shot in world units.")]
        [SerializeField] private float shotShake = 0.04f;

        [Tooltip("Camera displacement for an enemy hit in world units.")]
        [SerializeField] private float hitShake = 0.12f;

        [Tooltip("Camera displacement for an enemy death in world units.")]
        [SerializeField] private float deathShake = 0.18f;

        [Tooltip("Camera displacement when the player takes damage in world units.")]
        [SerializeField] private float playerDamageShake = 0.25f;

        [Tooltip("Maximum camera displacement in world units.")]
        [SerializeField] private float maxCameraShake = 0.25f;

        [Tooltip("Duration of shot, hit and death shakes in seconds.")]
        [SerializeField] private float shakeDuration = 0.14f;

        [Tooltip("Duration of the damage shake, background pulse and light pulse in seconds.")]
        [SerializeField] private float playerDamageDuration = 0.25f;

        [Tooltip("Number of direction changes during a camera shake.")]
        [SerializeField] private int shakeVibrato = 12;

        [Header("Damage Lighting")]
        [Tooltip("Peak intensity of the player damage light.")]
        [SerializeField] private float damageLightIntensity = 1.7f;

        [Tooltip("Camera background color at the start of a damage pulse.")]
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
            if (prewarmCount < 0)
            {
                throw new InvalidOperationException($"FeedbackConfig.prewarmCount must be nonnegative: {prewarmCount}");
            }

            if (hitParticles <= 0 || deathParticles <= 0 || playerDamageParticles <= 0)
            {
                throw new InvalidOperationException($"FeedbackConfig particle counts must be positive: hitParticles={hitParticles}, deathParticles={deathParticles}, playerDamageParticles={playerDamageParticles}");
            }

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
            if (shakeVibrato <= 0)
            {
                throw new InvalidOperationException($"FeedbackConfig.shakeVibrato must be positive: {shakeVibrato}");
            }
        }

        private static void Positive(float value, string field)
        {
            if (!float.IsFinite(value) || value <= 0f)
            {
                throw new InvalidOperationException($"FeedbackConfig.{field} must be positive: {value}");
            }
        }

        private static void Nonnegative(float value, string field)
        {
            if (!float.IsFinite(value) || value < 0f)
            {
                throw new InvalidOperationException($"FeedbackConfig.{field} must be nonnegative: {value}");
            }
        }
    }
}
