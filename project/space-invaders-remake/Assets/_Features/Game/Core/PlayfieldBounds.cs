using System;

namespace SpaceInvaders
{
    public static class PlayfieldBounds
    {

        public static (float Min,
            float Max) ForFormation(int columns,
            float spacing,
            float enemyHalfWidth,
            float playerHalfWidth,
            float formationCenter,
            float cameraMin,
            float cameraMax)
        {
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), columns, "Formation columns must be positive.");
            }

            ValidatePositive(spacing, nameof(spacing));
            ValidatePositive(enemyHalfWidth, nameof(enemyHalfWidth));
            ValidatePositive(playerHalfWidth, nameof(playerHalfWidth));
            ValidateFinite(formationCenter, nameof(formationCenter));
            ValidateFinite(cameraMin, nameof(cameraMin));
            ValidateFinite(cameraMax, nameof(cameraMax));
            if (cameraMin >= cameraMax)
            {
                throw new ArgumentOutOfRangeException(nameof(cameraMax), cameraMax, "Camera maximum must exceed its minimum.");
            }

            double formationHalfWidth = (columns - 1) * (double)spacing * 0.5 + enemyHalfWidth;
            double min = Math.Max(formationCenter - formationHalfWidth, cameraMin) + playerHalfWidth;
            double max = Math.Min(formationCenter + formationHalfWidth, cameraMax) - playerHalfWidth;
            if (min > max)
            {
                throw new ArgumentException("The intersection of the initial formation and camera is narrower than the player.");
            }

            return ((float)min, (float)max);
        }

        private static void ValidatePositive(float value, string name)
        {
            ValidateFinite(value, name);
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Width and spacing must be positive.");
            }
        }

        private static void ValidateFinite(float value, string name)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(name, value, "Playfield values must be finite.");
            }
        }
    }
}
