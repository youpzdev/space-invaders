using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(menuName = "Space Invaders/Formation Config")]
    public sealed class FormationConfig : ScriptableObject
    {
        [SerializeField] private int rows = 4;
        [SerializeField] private int columns = 8;
        [SerializeField] private Vector2 spacing = new Vector2(1.1f, 0.8f);
        [SerializeField] private float descentInterval = 2.5f;
        [SerializeField] private float descentStep = 0.3f;
        [SerializeField] private Sprite[] rowSprites;

        public int Rows => rows;
        public int Columns => columns;
        public Vector2 Spacing => spacing;
        public float DescentInterval => descentInterval;
        public float DescentStep => descentStep;
        public Sprite[] RowSprites => rowSprites;

        public void Validate()
        {
            if (rows <= 0 || columns <= 0)
                throw new InvalidOperationException($"FormationConfig.rows and columns must be positive: {rows}, {columns}");
            if (!float.IsFinite(spacing.x) || !float.IsFinite(spacing.y) || spacing.x <= 0f || spacing.y <= 0f)
                throw new InvalidOperationException($"FormationConfig.spacing must be positive: {spacing}");
            if (!float.IsFinite(descentInterval) || descentInterval <= 0f)
                throw new InvalidOperationException($"FormationConfig.descentInterval must be positive: {descentInterval}");
            if (!float.IsFinite(descentStep) || descentStep <= 0f)
                throw new InvalidOperationException($"FormationConfig.descentStep must be positive: {descentStep}");
            if (rowSprites == null || rowSprites.Length == 0)
                throw new InvalidOperationException("FormationConfig.rowSprites must contain at least one sprite");
            for (int i = 0; i < rowSprites.Length; i++)
                if (rowSprites[i] == null)
                    throw new InvalidOperationException($"FormationConfig.rowSprites[{i}] is missing");
        }
    }
}
