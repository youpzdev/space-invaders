using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class PlayerDamageReceiver : MonoBehaviour, IDamageable
    {
        private PlayerConfig config;
        private Action cancelProtection;
        private bool receivingDamage;
        private bool invulnerable;

        public Faction Faction => Faction.Player;
        public Health Health { get; private set; }
        public bool IsInvulnerable => invulnerable;

        public event Action<bool> InvulnerabilityChanged;
        public event Action DamageTaken;

        public void Initialize(Health health, PlayerConfig playerConfig)
        {
            if (health == null)
            {
                throw new ArgumentNullException(nameof(health));
            }

            if (playerConfig == null)
            {
                throw new ArgumentNullException(nameof(playerConfig));
            }

            playerConfig.Validate();
            StopReceivingDamage();
            Health = health;
            config = playerConfig;
            receivingDamage = true;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Player damage must be positive");
            }

            if (!receivingDamage || Health.IsDepleted || invulnerable)
            {
                return;
            }

            SetInvulnerable(true);
            cancelProtection = Timer.After(config.InvulnerabilityDuration, EndProtection, this);
            DamageTaken?.Invoke();
            Health.TakeDamage(amount);
        }

        public void StopReceivingDamage()
        {
            receivingDamage = false;
            cancelProtection?.Invoke();
            cancelProtection = null;
            SetInvulnerable(false);
        }

        private void EndProtection()
        {
            cancelProtection = null;
            SetInvulnerable(false);
        }

        private void SetInvulnerable(bool value)
        {
            if (invulnerable == value)
            {
                return;
            }

            invulnerable = value;
            InvulnerabilityChanged?.Invoke(value);
        }

        private void OnDestroy() => StopReceivingDamage();
    }
}
