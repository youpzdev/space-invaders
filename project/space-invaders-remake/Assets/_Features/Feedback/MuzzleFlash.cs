using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SpaceInvaders
{
    public sealed class MuzzleFlash : MonoBehaviour
    {
        [Header("Visuals")]
        [Tooltip("Sprite renderer used by this component.")]
        [SerializeField] private SpriteRenderer sprite;

        [Header("Lighting")]
        [Tooltip("Point light faded out with the muzzle flash.")]
        [SerializeField] private Light2D flashLight;

        private Tween flashTween;
        private Action<MuzzleFlash> release;
        private bool active;

        public void Play(FeedbackConfig config, Color color, Action<MuzzleFlash> returnToPool)
        {
            if (sprite == null)
            {
                throw new InvalidOperationException("MuzzleFlash.sprite is missing");
            }

            if (flashLight == null)
            {
                throw new InvalidOperationException("MuzzleFlash.flashLight is missing");
            }

            flashTween?.Kill();
            release = returnToPool;
            active = true;
            sprite.color = color;
            flashLight.color = color;
            flashLight.intensity = config.DamageLightIntensity * 0.5f;
            transform.localScale = Vector3.zero;
            flashTween = DOTween.Sequence()
                .Append(transform.DOScale(config.FlashScale, config.FlashDuration).SetEase(Ease.OutQuad))
                .Join(sprite.DOFade(0f, config.FlashDuration))
                .Join(DOTween.To(() => flashLight.intensity, value => flashLight.intensity = value, 0f, config.FlashDuration))
                .OnComplete(ReturnToPool);
        }

        public void ReturnToPool()
        {
            if (!active)
            {
                return;
            }

            active = false;
            flashTween?.Kill();
            flashTween = null;
            flashLight.intensity = 0f;
            var callback = release;
            release = null;
            callback(this);
        }

        private void OnDestroy() => flashTween?.Kill();
    }
}
