using System;

namespace SpaceInvaders
{
    public sealed class GameController : IDisposable
    {
        private readonly GameSession session;
        private readonly PlayerController player;
        private readonly PlayerDamageReceiver damage;
        private readonly PlayerHitEffect effect;
        private readonly EnemyFormation formation;
        private readonly EnemyShooter shooter;
        private readonly ProjectileSystem projectiles;
        private readonly DefeatZone defeatZone;
        private readonly ResultPanel result;

        public GameController(GameSession session,
            PlayerController player,
            PlayerDamageReceiver damage,
            PlayerHitEffect effect,
            EnemyFormation formation,
            EnemyShooter shooter,
            ProjectileSystem projectiles,
            DefeatZone defeatZone,
            ResultPanel result)
        {
            this.session = session;
            this.player = player;
            this.damage = damage;
            this.effect = effect;
            this.formation = formation;
            this.shooter = shooter;
            this.projectiles = projectiles;
            this.defeatZone = defeatZone;
            this.result = result;
            formation.EnemyRemoved += OnEnemyRemoved;
            damage.Health.Depleted += OnDefeat;
            defeatZone.EnemyEntered += OnDefeat;
            session.Finished += OnFinished;
        }

        public void StartGame()
        {
            result.Hide();
            session.Start();
            projectiles.StartPlaying();
            player.StartPlaying();
            formation.StartMoving();
            shooter.StartShooting();
        }

        private void OnEnemyRemoved(int remaining) => session.RegisterEnemyDeath(remaining);

        private void OnDefeat() => session.Lose();

        private void OnFinished(GameState state, int score)
        {
            StopGame();
            result.Show(state, score);
        }

        public void StopGame()
        {
            damage.StopReceivingDamage();
            player.StopPlaying();
            shooter.StopShooting();
            formation.StopMoving();
            projectiles.StopAndClear();
            effect.StopEffect();
        }

        public void Dispose()
        {
            formation.EnemyRemoved -= OnEnemyRemoved;
            damage.Health.Depleted -= OnDefeat;
            defeatZone.EnemyEntered -= OnDefeat;
            session.Finished -= OnFinished;
            StopGame();
        }
    }
}
