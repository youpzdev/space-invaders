using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private FormationConfig formationConfig;
        [SerializeField] private ProjectileConfig projectileConfig;
        [SerializeField] private EnemyFormation formation;
        [SerializeField] private EnemyShooter shooter;
        [SerializeField] private ProjectileSystem projectiles;
        [SerializeField] private DefeatZone defeatZone;
        [SerializeField] private GameHud hud;
        [SerializeField] private ResultPanel result;
        private GameController controller;
        public GameSession Session { get; private set; }

        private void Awake()
        {
            if (gameCamera == null || playerPrefab == null || enemyPrefab == null || playerConfig == null ||
                enemyConfig == null || formationConfig == null || projectileConfig == null || formation == null ||
                shooter == null || projectiles == null || defeatZone == null || hud == null || result == null)
                throw new InvalidOperationException("Bootstrapper: camera, prefabs, configs and scene references must be assigned.");

            playerConfig.Validate();
            enemyConfig.Validate();
            formationConfig.Validate();
            projectileConfig.Validate();

            var score = new Score();

            Session = new GameSession(score, enemyConfig.ScoreReward);

            projectiles.Initialize(projectileConfig, gameCamera);

            Vector3 bottomLeft = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, -gameCamera.transform.position.z));
            Vector3 topRight = gameCamera.ViewportToWorldPoint(new Vector3(1f, 1f, -gameCamera.transform.position.z));

            PlayerController player = Instantiate(playerPrefab, new Vector3(0f, bottomLeft.y + 1.2f, 0f), Quaternion.identity);

            float halfWidth = player.GetComponent<SpriteRenderer>().bounds.extents.x;

            player.Initialize(playerConfig, projectiles, new Vector2(bottomLeft.x + halfWidth, topRight.x - halfWidth));
            PlayerDamageReceiver damage = player.GetComponent<PlayerDamageReceiver>();
            damage.Initialize(new Health(3), playerConfig);

            PlayerHitEffect effect = player.GetComponent<PlayerHitEffect>();
            effect.Initialize(damage);

            formation.transform.position = new Vector3(0f, topRight.y - 1.6f, 0f);

            Enemy[] enemies = new EnemySpawner(enemyPrefab, formation.transform, enemyConfig, formationConfig).SpawnGrid();
            formation.Initialize(enemies, formationConfig, defeatZone);
            shooter.Initialize(formation, projectiles, enemyConfig);

            defeatZone.transform.position = new Vector3(0f, player.transform.position.y, 0f);

            hud.Bind(score, damage.Health);

            controller = new GameController(Session, player, damage, effect, formation, shooter, projectiles, defeatZone, result);
            controller.StartGame();
        }

        private void OnDestroy()
        {
            controller?.Dispose();
            if (hud != null) hud.Unbind();
        }
    }
}
