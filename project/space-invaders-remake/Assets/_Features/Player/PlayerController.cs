using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvaders
{
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Shooting")]
        [Tooltip("World position where projectiles are spawned.")]
        [SerializeField] private Transform muzzle;

        private PlayerConfig config;
        private ProjectileSystem projectiles;
        private Vector2 horizontalBounds;
        private float nextShotTime;
        private bool playing;

        public void Initialize(PlayerConfig playerConfig, ProjectileSystem projectileSystem, Vector2 bounds)
        {
            if (playerConfig == null)
            {
                throw new ArgumentNullException(nameof(playerConfig));
            }

            if (projectileSystem == null)
            {
                throw new ArgumentNullException(nameof(projectileSystem));
            }

            if (muzzle == null)
            {
                throw new InvalidOperationException("PlayerController.muzzle is missing");
            }

            if (!float.IsFinite(bounds.x) || !float.IsFinite(bounds.y) || bounds.x > bounds.y)
            {
                throw new ArgumentException($"PlayerController.horizontalBounds is invalid: {bounds}", nameof(bounds));
            }

            playerConfig.Validate();
            config = playerConfig;
            projectiles = projectileSystem;
            horizontalBounds = bounds;
        }

        public void StartPlaying()
        {
            if (config == null)
            {
                throw new InvalidOperationException("PlayerController.Initialize must run before StartPlaying");
            }

            nextShotTime = Time.time;
            playing = true;
        }

        public void StopPlaying() => playing = false;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (!playing || keyboard == null)
            {
                return;
            }

            int movement = 0;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                movement--;
            }

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                movement++;
            }

            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x + movement * config.MoveSpeed * Time.deltaTime, horizontalBounds.x, horizontalBounds.y);
            transform.position = position;
            if (keyboard.spaceKey.isPressed && Time.time >= nextShotTime)
            {
                nextShotTime = Time.time + config.FireInterval;
                projectiles.Fire(muzzle.position, Vector2.up, Faction.Player);
            }
        }
    }
}
