using System;

namespace SpaceInvaders
{
    public sealed class GameSession
    {
        private readonly int enemyReward;

        public GameState State { get; private set; } = GameState.Ready;
        public Score Score { get; }

        public event Action<GameState, int> Finished;

        public GameSession(Score score, int enemyReward)
        {
            Score = score ?? throw new ArgumentNullException(nameof(score));
            if (enemyReward < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(enemyReward), enemyReward, "Enemy reward cannot be negative.");
            }

            this.enemyReward = enemyReward;
        }

        public void Start()
        {
            if (State != GameState.Ready)
            {
                return;
            }

            State = GameState.Playing;
        }

        public void RegisterEnemyDeath(int remaining)
        {
            if (remaining < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(remaining), remaining, "Remaining enemy count cannot be negative.");
            }

            if (State != GameState.Playing)
            {
                return;
            }

            Score.Add(enemyReward);
            if (remaining == 0 && State == GameState.Playing)
            {
                Finish(GameState.Won);
            }
        }

        public void Lose()
        {
            if (State == GameState.Playing)
            {
                Finish(GameState.Lost);
            }
        }

        private void Finish(GameState result)
        {
            State = result;
            Finished?.Invoke(result, Score.Value);
        }
    }
}
