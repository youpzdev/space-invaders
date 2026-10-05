using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Formation Config")]
    public sealed class FormationConfig : ScriptableObject
    {
        [Header("Grid")]
        [Tooltip("Number of enemy rows spawned at the start of the round.")]
        [SerializeField] private int rows = 4;

        [Tooltip("Number of enemies in each row. Also defines the initial playfield width.")]
        [SerializeField] private int columns = 8;

        [Tooltip("Horizontal and vertical spacing between enemy centers in local units.")]
        [SerializeField] private Vector2 spacing = new Vector2(1.1f, 0.8f);

        [Header("Descent")]
        [Tooltip("Seconds between downward steps of the whole formation.")]
        [SerializeField] private float descentInterval = 2.5f;

        [Tooltip("Distance moved down per step in world units.")]
        [SerializeField] private float descentStep = 0.3f;

        [Tooltip("Seconds spent moving each downward step. Must not exceed the descent interval.")]
        [SerializeField] private float descentDuration = 0.18f;

        [Header("Appearance")]
        [Tooltip("Sprites assigned from top to bottom and repeated when there are more rows.")]
        [SerializeField] private Sprite[] rowSprites;

        public int Rows => rows;
        public int Columns => columns;
        public Vector2 Spacing => spacing;
        public float DescentInterval => descentInterval;
        public float DescentStep => descentStep;
        public float DescentDuration => descentDuration;
        public Sprite[] RowSprites => rowSprites;

        public void Validate()
        {
            if (rows <= 0 || columns <= 0)
            {
                throw new InvalidOperationException($"FormationConfig.rows and columns must be positive: {rows}, {columns}");
            }

            if (!float.IsFinite(spacing.x) || !float.IsFinite(spacing.y) || spacing.x <= 0f || spacing.y <= 0f)
            {
                throw new InvalidOperationException($"FormationConfig.spacing must be positive: {spacing}");
            }

            if (!float.IsFinite(descentInterval) || descentInterval <= 0f)
            {
                throw new InvalidOperationException($"FormationConfig.descentInterval must be positive: {descentInterval}");
            }

            if (!float.IsFinite(descentStep) || descentStep <= 0f)
            {
                throw new InvalidOperationException($"FormationConfig.descentStep must be positive: {descentStep}");
            }

            if (!float.IsFinite(descentDuration) || descentDuration <= 0f || descentDuration > descentInterval)
            {
                throw new InvalidOperationException($"FormationConfig.descentDuration must be positive and at most {descentInterval}: {descentDuration}");
            }

            if (rowSprites == null || rowSprites.Length == 0)
            {
                throw new InvalidOperationException("FormationConfig.rowSprites must contain at least one sprite");
            }

            for (int i = 0; i < rowSprites.Length; i++)
            {
                if (rowSprites[i] == null)
                {
                    throw new InvalidOperationException($"FormationConfig.rowSprites[{i}] is missing");
                }
            }
        }
    }
}
