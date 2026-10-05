using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [Header("Camera")]
        [Tooltip("Gameplay camera used for spawn positions and screen boundaries.")]
        [SerializeField] private Camera gameCamera;

        [Header("Prefabs")]
        [Tooltip("Player prefab created when the scene loads.")]
        [SerializeField] private PlayerController playerPrefab;

        [Tooltip("Enemy prefab used to create the formation.")]
        [SerializeField] private Enemy enemyPrefab;

        [Header("Settings")]
        [Tooltip("Player movement, shooting and damage settings.")]
        [SerializeField] private PlayerConfig playerConfig;

        [Tooltip("Enemy health, reward and shooting settings.")]
        [SerializeField] private EnemyConfig enemyConfig;

        [Tooltip("Enemy grid, row sprites and descent settings.")]
        [SerializeField] private FormationConfig formationConfig;

        [Tooltip("Projectile speed and pool settings.")]
        [SerializeField] private ProjectileConfig projectileConfig;

        [Header("Game Systems")]
        [Tooltip("Scene component that tracks living enemies and moves the formation.")]
        [SerializeField] private EnemyFormation formation;

        [Tooltip("Scene component that schedules shots from surviving enemies.")]
        [SerializeField] private EnemyShooter shooter;

        [Tooltip("Scene component that creates, tracks and clears pooled projectiles.")]
        [SerializeField] private ProjectileSystem projectiles;

        [Tooltip("Trigger marking the line enemies must not reach.")]
        [SerializeField] private DefeatZone defeatZone;

        [Header("Interface")]
        [Tooltip("Score and player health display.")]
        [SerializeField] private GameHud hud;

        [Tooltip("Panel shown after victory or defeat.")]
        [SerializeField] private ResultPanel result;

        [Header("Presentation")]
        [Tooltip("Combat effects subscribed to shots, hits and deaths.")]
        [SerializeField] private CombatFeedback feedback;

        [Tooltip("Visual borders placed around the initial playfield.")]
        [SerializeField] private PlayfieldView playfield;

        private GameController controller;

        public GameSession Session { get; private set; }

        private void Awake()
        {
            if (gameCamera == null || playerPrefab == null || enemyPrefab == null ||
                playerConfig == null || enemyConfig == null || formationConfig == null ||
                projectileConfig == null || formation == null || shooter == null ||
                projectiles == null || defeatZone == null || hud == null ||
                result == null || feedback == null || playfield == null)
            {
                throw new InvalidOperationException("Bootstrapper: camera, prefabs, configs and scene references must be assigned.");
            }

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
            float halfWidth = player.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
            PlayerDamageReceiver damage = player.GetComponent<PlayerDamageReceiver>();
            damage.Initialize(new Health(3), playerConfig);


            PlayerHitEffect effect = player.GetComponent<PlayerHitEffect>();
            effect.Initialize(damage);


            formation.transform.position = new Vector3(0f, topRight.y - 1.6f, 0f);


            Enemy[] enemies = new EnemySpawner(enemyPrefab, formation.transform, enemyConfig, formationConfig).SpawnGrid();
            float enemyHalfWidth = 0f;
            foreach (Enemy enemy in enemies)
            {
                enemyHalfWidth = Mathf.Max(enemyHalfWidth, enemy.GetComponentInChildren<SpriteRenderer>().bounds.extents.x);
            }



            var bounds = PlayfieldBounds.ForFormation(formationConfig.Columns,
                formationConfig.Spacing.x * Mathf.Abs(formation.transform.lossyScale.x),
                enemyHalfWidth,
                halfWidth,
                formation.transform.position.x,
                bottomLeft.x,
                topRight.x);
            player.Initialize(playerConfig, projectiles, new Vector2(bounds.Min, bounds.Max));
            playfield.Initialize(bounds.Min - halfWidth, bounds.Max + halfWidth, player.transform.position.y - 0.45f, topRight.y - 0.8f);
            formation.Initialize(enemies, formationConfig, defeatZone);
            shooter.Initialize(formation, projectiles, enemyConfig);


            float playerTop = player.GetComponentInChildren<SpriteRenderer>().bounds.max.y;
            defeatZone.transform.position = new Vector3(0f, playerTop, 0f);


            hud.Bind(score, damage.Health);
            feedback.Initialize(projectiles, enemies, damage, gameCamera, player.GetComponentInChildren<SpriteRenderer>().transform);


            controller = new GameController(Session, player, damage, effect, formation, shooter, projectiles, defeatZone, result);
            controller.StartGame();
        }

        private void OnDestroy()
        {
            controller?.Dispose();
            if (feedback != null)
            {
                feedback.Dispose();
            }

            if (hud != null)
            {
                hud.Unbind();
            }
        }
    }
}
