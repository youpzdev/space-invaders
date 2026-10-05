using System;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class PlayerHitEffect : MonoBehaviour
    {
        [Header("Visuals")]
        [Tooltip("Sprite renderer used by this component.")]
        [SerializeField] private SpriteRenderer sprite;

        private PlayerDamageReceiver receiver;
        private Color initialColor;
        private Tween blink;
        private Tween flash;

        public void Initialize(PlayerDamageReceiver damageReceiver)
        {
            if (damageReceiver == null)
            {
                throw new ArgumentNullException(nameof(damageReceiver));
            }

            if (sprite == null)
            {
                throw new InvalidOperationException("PlayerHitEffect.sprite is missing");
            }

            if (receiver != null)
            {
                Unsubscribe();
            }

            receiver = damageReceiver;
            initialColor = sprite.color;
            receiver.DamageTaken += OnDamageTaken;
            receiver.InvulnerabilityChanged += OnInvulnerabilityChanged;
        }

        public void StopEffect()
        {
            blink?.Kill();
            flash?.Kill();
            blink = null;
            flash = null;
            if (sprite != null)
            {
                sprite.color = initialColor;
            }
        }

        private void OnDamageTaken()
        {
            blink?.Kill();
            flash?.Kill();
            sprite.color = Color.white;
            flash = sprite.DOColor(initialColor, 0.08f).OnComplete(() =>
            {
                if (receiver.IsInvulnerable)
                {
                    StartBlinking();
                }
            });
        }

        private void OnInvulnerabilityChanged(bool value)
        {
            if (!value)
            {
                StopEffect();
                return;
            }
        }

        private void StartBlinking()
        {
            blink?.Kill();
            blink = sprite.DOFade(0.25f, 0.08f).SetLoops(-1, LoopType.Yoyo);
        }

        private void Unsubscribe()
        {
            receiver.DamageTaken -= OnDamageTaken;
            receiver.InvulnerabilityChanged -= OnInvulnerabilityChanged;
        }

        private void OnDestroy()
        {
            if (receiver != null)
            {
                Unsubscribe();
            }

            StopEffect();
        }
    }
}
