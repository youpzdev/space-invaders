using System;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Projectile))]
    public sealed class ProjectileAppearance : MonoBehaviour
    {
        [SerializeField] private FeedbackConfig config;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TrailRenderer trail;
        private Projectile projectile;

        private void Awake()
        {
            if (config == null || sprite == null || trail == null)
                throw new InvalidOperationException("ProjectileAppearance.config, sprite and trail must be assigned");
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
            if (trail != null) trail.Clear();
        }

        private void OnDestroy()
        {
            if (projectile != null) projectile.Launched -= OnLaunched;
        }
    }
}
