using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Transform muzzle;
        private Health health;

        public Faction Faction => Faction.Enemy;
        public bool IsAlive => health != null && !health.IsDepleted;
        public Vector3 FirePosition => muzzle.position;
        public float Bottom => sprite.bounds.min.y;
        public event Action<Enemy> Died;
        public event Action<Enemy> Hit;

        public void Initialize(int hitPoints, Sprite appearance)
        {
            if (sprite == null || muzzle == null)
                throw new InvalidOperationException("Enemy: sprite and muzzle must be assigned.");
            health = new Health(hitPoints);
            sprite.sprite = appearance;
            health.Depleted += OnDepleted;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), amount, "Enemy damage must be positive");
            if (!IsAlive) return;
            Hit?.Invoke(this);
            health.TakeDamage(amount);
        }

        private void OnDepleted()
        {
            GetComponent<Collider2D>().enabled = false;
            sprite.enabled = false;
            Died?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
