using System;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Projectile))]
    public sealed class ProjectileAppearance : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Shared colors and animation settings for combat effects.")]
        [SerializeField] private FeedbackConfig config;

        [Header("Visuals")]
        [Tooltip("Sprite renderer used by this component.")]
        [SerializeField] private SpriteRenderer sprite;

        [Tooltip("Projectile trail cleared between pooled shots.")]
        [SerializeField] private TrailRenderer trail;

        private Projectile projectile;

        private void Awake()
        {
            if (config == null || sprite == null || trail == null)
            {
                throw new InvalidOperationException("ProjectileAppearance.config, sprite and trail must be assigned");
            }

            config.Validate();
            projectile = GetComponent<Projectile>();
            projectile.Launched += OnLaunched;
        }

        private void OnLaunched(Faction faction)
        {
            Color color = faction == Faction.Player ? config.PlayerColor : config.EnemyColor;
            sprite.color = color;
            trail.Clear();
            trail.startColor = color;
            trail.endColor = new Color(color.r, color.g, color.b, 0f);
        }

        private void OnDisable()
        {
            if (trail != null)
            {
                trail.Clear();
            }
        }

        private void OnDestroy()
        {
            if (projectile != null)
            {
                projectile.Launched -= OnLaunched;
            }
        }
    }
}
