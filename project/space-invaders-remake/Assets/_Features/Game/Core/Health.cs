using System;

namespace SpaceInvaders
{
    public sealed class Health
    {
        public int Current { get; private set; }
        public bool IsDepleted => Current == 0;

        public event Action<int> Changed;
        public event Action Depleted;

        public Health(int max)
        {
            if (max <= 0)
                throw new ArgumentOutOfRangeException(nameof(max), max, "Health must be positive.");

            Current = max;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Damage must be positive.");

            if (IsDepleted)
                return;

            Current = Math.Max(0, Current - amount);
            bool becameDepleted = IsDepleted;
            Changed?.Invoke(Current);

            if (becameDepleted)
                Depleted?.Invoke();
        }
    }
}
