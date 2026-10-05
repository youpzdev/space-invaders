using System;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class Projectile : MonoBehaviour
    {
        private Rigidbody2D body;
        private Camera gameCamera;
        private Action<Projectile> release;
        private Vector2 velocity;
        private Faction faction;
        private bool flying;

        public event Action<Faction> Launched;

        public void Initialize(Vector2 direction, Faction owner, float speed, Camera camera, Action<Projectile> returnToPool)
        {
            body = GetComponent<Rigidbody2D>();
            gameCamera = camera;
            release = returnToPool;
            faction = owner;
            velocity = direction * speed;
            body.position = transform.position;
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
            flying = true;
            Launched?.Invoke(owner);
        }

        public void ReturnToPool()
        {
            if (!flying) return;
            flying = false;
            body.linearVelocity = Vector2.zero;
            Action<Projectile> callback = release;
            release = null;
            callback(this);
        }

        private void FixedUpdate()
        {
            if (!flying) return;
            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!flying) return;
            Vector3 viewport = gameCamera.WorldToViewportPoint(transform.position);
            if (viewport.y < -0.05f || viewport.y > 1.05f || viewport.x < -0.05f || viewport.x > 1.05f)
                ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!flying) return;
            IDamageable target = other.GetComponentInParent<IDamageable>();
            if (target == null || target.Faction == faction) return;
            flying = false;
            body.linearVelocity = Vector2.zero;
            Action<Projectile> callback = release;
            release = null;
            callback(this);
            target.TakeDamage(1);
        }
    }
}
