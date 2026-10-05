using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class DefeatZone : MonoBehaviour
    {
        public event Action EnemyEntered;
        public void CheckEnemy(Enemy enemy)
        {
            if (enemy.IsAlive && enemy.Bottom <= transform.position.y) EnemyEntered?.Invoke();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy) && enemy.IsAlive) EnemyEntered?.Invoke();
        }
    }
}
