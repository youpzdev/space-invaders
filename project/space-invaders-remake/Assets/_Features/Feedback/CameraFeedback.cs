using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SpaceInvaders
{
    public sealed class CameraFeedback : MonoBehaviour
    {
        [Header("Lighting")]
        [Tooltip("Point light pulsed when the player takes damage.")]
        [SerializeField] private Light2D damageLight;

        private Camera gameCamera;
        private FeedbackConfig config;
        private Vector3 basePosition;
        private Color baseBackground;
        private float baseLightIntensity;
        private Tween shake;
        private float currentShakeStrength;
        private Tween backgroundPulse;
        private Tween lightPulse;

        public void Initialize(Camera camera, FeedbackConfig feedbackConfig)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            if (feedbackConfig == null)
            {
                throw new ArgumentNullException(nameof(feedbackConfig));
            }

            if (damageLight == null)
            {
                throw new InvalidOperationException("CameraFeedback.damageLight is missing");
            }

            config = feedbackConfig;
            gameCamera = camera;
            basePosition = camera.transform.position;
            baseBackground = camera.backgroundColor;
            baseLightIntensity = damageLight.intensity;
        }

        public void PlayShot() => Shake(config.ShotShake, config.ShakeDuration);

        public void PlayHit() => Shake(config.HitShake, config.ShakeDuration);

        public void PlayDeath() => Shake(config.DeathShake, config.ShakeDuration);

        public void PlayPlayerDamage()
        {
            Shake(config.PlayerDamageShake, config.PlayerDamageDuration);
            backgroundPulse?.Kill();
            lightPulse?.Kill();
            gameCamera.backgroundColor = config.DamageBackgroundColor;
            backgroundPulse = DOTween.To(() => gameCamera.backgroundColor,
                color => gameCamera.backgroundColor = color,
                baseBackground,
                config.PlayerDamageDuration);
            damageLight.intensity = config.DamageLightIntensity;
            lightPulse = DOTween.To(() => damageLight.intensity,
                value => damageLight.intensity = value,
                baseLightIntensity,
                config.PlayerDamageDuration);
        }

        public void StopFeedback()
        {
            shake?.Kill();
            backgroundPulse?.Kill();
            lightPulse?.Kill();
            shake = null;
            currentShakeStrength = 0f;
            backgroundPulse = null;
            lightPulse = null;
            if (gameCamera != null)
            {
                gameCamera.transform.position = basePosition;
                gameCamera.backgroundColor = baseBackground;
            }

            if (damageLight != null)
            {
                damageLight.intensity = baseLightIntensity;
            }
        }

        private void Shake(float strength, float duration)
        {
            if (shake != null && shake.IsActive() && strength < currentShakeStrength)
            {
                return;
            }

            shake?.Kill();
            currentShakeStrength = strength;
            gameCamera.transform.position = basePosition;
            float bounded = Mathf.Min(strength, config.MaxCameraShake) / Mathf.Sqrt(2f);
            shake = gameCamera.transform.DOShakePosition(duration, new Vector3(bounded, bounded, 0f), config.ShakeVibrato).OnComplete(() =>
            {
                gameCamera.transform.position = basePosition;
                currentShakeStrength = 0f;
            });
        }

        private void OnDestroy() => StopFeedback();
    }
}
