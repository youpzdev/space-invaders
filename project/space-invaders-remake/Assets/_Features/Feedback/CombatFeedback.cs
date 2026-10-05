using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class CombatFeedback : MonoBehaviour, IDisposable
    {
        [SerializeField] private FeedbackConfig config;
        [SerializeField] private PixelBurst burstPrefab;
        [SerializeField] private MuzzleFlash flashPrefab;
        [SerializeField] private CameraFeedback cameraFeedback;
        private readonly HashSet<PixelBurst> bursts = new HashSet<PixelBurst>();
        private readonly HashSet<MuzzleFlash> flashes = new HashSet<MuzzleFlash>();
        private readonly List<Enemy> enemies = new List<Enemy>();
        private ProjectileSystem projectiles;
        private PlayerDamageReceiver playerDamage;
        private Transform playerPresentation;
        private Vector3 basePlayerPosition;
        private Tween recoil;
        private bool initialized;

        public void Initialize(ProjectileSystem projectileSystem, IReadOnlyList<Enemy> enemyList, PlayerDamageReceiver damageReceiver, Camera camera, Transform presentation)
        {
            if (initialized) throw new InvalidOperationException("CombatFeedback.Initialize was called twice");
            if (projectileSystem == null) throw new ArgumentNullException(nameof(projectileSystem));
            if (enemyList == null) throw new ArgumentNullException(nameof(enemyList));
            if (damageReceiver == null) throw new ArgumentNullException(nameof(damageReceiver));
            if (presentation == null || presentation == damageReceiver.transform || !presentation.IsChildOf(damageReceiver.transform))
                throw new ArgumentException("CombatFeedback.presentation must be a visual child of the player", nameof(presentation));
            if (config == null || burstPrefab == null || flashPrefab == null || cameraFeedback == null)
                throw new InvalidOperationException("CombatFeedback.config, burstPrefab, flashPrefab and cameraFeedback must be assigned");
            foreach (Enemy enemy in enemyList)
                if (enemy == null) throw new ArgumentException("CombatFeedback.enemyList contains a missing enemy", nameof(enemyList));
            config.Validate();
            cameraFeedback.Initialize(camera, config);
            Pooling.Prewarm(burstPrefab.gameObject, config.PrewarmCount);
            Pooling.Prewarm(flashPrefab.gameObject, config.PrewarmCount);
            projectiles = projectileSystem;
            playerDamage = damageReceiver;
            playerPresentation = presentation;
            basePlayerPosition = presentation.localPosition;
            foreach (Enemy enemy in enemyList)
            {
                enemies.Add(enemy);
                enemy.Hit += OnEnemyHit;
                enemy.Died += OnEnemyDied;
            }
            projectiles.ShotFired += OnShotFired;
            playerDamage.DamageTaken += OnPlayerDamage;
            initialized = true;
        }

        public void Dispose()
        {
            if (!initialized) return;
            initialized = false;
            projectiles.ShotFired -= OnShotFired;
            playerDamage.DamageTaken -= OnPlayerDamage;
            foreach (Enemy enemy in enemies)
            {
                if (enemy == null) continue;
                enemy.Hit -= OnEnemyHit;
                enemy.Died -= OnEnemyDied;
            }
            enemies.Clear();
            recoil?.Kill();
            recoil = null;
            if (playerPresentation != null) playerPresentation.localPosition = basePlayerPosition;
            if (cameraFeedback != null) cameraFeedback.StopFeedback();
            foreach (PixelBurst burst in new List<PixelBurst>(bursts))
                if (burst != null) burst.ReturnToPool();
            foreach (MuzzleFlash flash in new List<MuzzleFlash>(flashes))
                if (flash != null) flash.ReturnToPool();
            bursts.Clear();
            flashes.Clear();
            Pooling.Clear(burstPrefab.gameObject);
            Pooling.Clear(flashPrefab.gameObject);
        }

        private void OnShotFired(Vector3 position, Faction faction)
        {
            GameObject instance = Pooling.Instantiate(flashPrefab.gameObject, position, Quaternion.identity);
            MuzzleFlash flash = instance.GetComponent<MuzzleFlash>();
            flashes.Add(flash);
            flash.Play(config, faction == Faction.Player ? config.PlayerColor : config.EnemyColor, ReleaseFlash);
            if (faction != Faction.Player) return;
            cameraFeedback.PlayShot();
            recoil?.Kill();
            playerPresentation.localPosition = basePlayerPosition;
            recoil = DOTween.Sequence()
                .Append(playerPresentation.DOLocalMoveY(basePlayerPosition.y - config.RecoilDistance, config.RecoilDuration / 2f))
                .Append(playerPresentation.DOLocalMoveY(basePlayerPosition.y, config.RecoilDuration / 2f));
        }

        private void OnEnemyHit(Enemy enemy)
        {
            Burst(enemy.transform.position, config.HitColor, config.HitParticles);
            cameraFeedback.PlayHit();
        }

        private void OnEnemyDied(Enemy enemy)
        {
            Burst(enemy.transform.position, config.DeathColor, config.DeathParticles);
            cameraFeedback.PlayDeath();
        }

        private void OnPlayerDamage()
        {
            Burst(playerDamage.transform.position, config.PlayerColor, config.PlayerDamageParticles);
            cameraFeedback.PlayPlayerDamage();
        }

        private void Burst(Vector3 position, Color color, int count)
        {
            GameObject instance = Pooling.Instantiate(burstPrefab.gameObject, position, Quaternion.identity);
            PixelBurst burst = instance.GetComponent<PixelBurst>();
            bursts.Add(burst);
            burst.Play(config, color, count, ReleaseBurst);
        }

        private void ReleaseBurst(PixelBurst burst)
        {
            bursts.Remove(burst);
            Pooling.Destroy(burst.gameObject);
        }

        private void ReleaseFlash(MuzzleFlash flash)
        {
            flashes.Remove(flash);
            Pooling.Destroy(flash.gameObject);
        }

        private void OnDestroy() => Dispose();
    }
}
