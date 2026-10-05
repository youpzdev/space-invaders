using System;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class PlayfieldView : MonoBehaviour
    {
        [Header("Borders")]
        [Tooltip("Left visual border positioned from the initial enemy formation.")]
        [SerializeField] private SpriteRenderer leftBorder;

        [Tooltip("Right visual border positioned from the initial enemy formation.")]
        [SerializeField] private SpriteRenderer rightBorder;

        [Tooltip("Bottom visual border below the player line.")]
        [SerializeField] private SpriteRenderer bottomBorder;

        public void Initialize(float left, float right, float bottom, float top)
        {
            if (leftBorder == null || rightBorder == null || bottomBorder == null)
            {
                throw new InvalidOperationException("PlayfieldView borders must be assigned");
            }

            if (left >= right || bottom >= top)
            {
                throw new ArgumentException($"PlayfieldView invalid edges: {left},{right},{bottom},{top}");
            }

            Place(leftBorder, new Vector2(left, (bottom + top) * 0.5f), new Vector2(0.035f, top - bottom));
            Place(rightBorder, new Vector2(right, (bottom + top) * 0.5f), new Vector2(0.035f, top - bottom));
            Place(bottomBorder, new Vector2((left + right) * 0.5f, bottom), new Vector2(right - left, 0.035f));
        }

        private static void Place(SpriteRenderer border, Vector2 position, Vector2 size)
        {
            if (border.sprite == null)
            {
                throw new InvalidOperationException("PlayfieldView border sprite is missing: " + border.name);
            }

            border.transform.position = new Vector3(position.x, position.y, border.transform.position.z);
            Vector2 spriteSize = border.sprite.bounds.size;
            border.transform.localScale = new Vector3(size.x / spriteSize.x, size.y / spriteSize.y, 1f);
        }
    }
}
