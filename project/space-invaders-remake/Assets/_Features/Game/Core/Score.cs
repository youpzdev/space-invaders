using System;

namespace SpaceInvaders
{
    public sealed class Score
    {
        public int Value { get; private set; }

        public event Action<int> Changed;

        public void Add(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Score amount cannot be negative.");

            if (amount == 0)
                return;

            Value = checked(Value + amount);
            Changed?.Invoke(Value);
        }
    }
}
