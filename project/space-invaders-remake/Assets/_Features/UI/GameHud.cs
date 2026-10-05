using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SpaceInvaders
{
    public sealed class GameHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image[] healthIcons;
        private Score score;
        private Health health;
        private Tween scorePulse;
        private Vector3 scoreScale;

        public void Bind(Score currentScore, Health playerHealth)
        {
            if (scoreText == null || healthIcons == null || healthIcons.Length != 3)
                throw new InvalidOperationException("GameHud: scoreText and three healthIcons must be assigned.");
            Unbind();
            score = currentScore;
            health = playerHealth;
            scoreScale = scoreText.transform.localScale;
            score.Changed += UpdateScore;
            health.Changed += UpdateHealth;
            UpdateScore(score.Value);
            UpdateHealth(health.Current);
        }

        private void UpdateScore(int value)
        {
            scoreText.text = "SCORE " + value.ToString("D5");
            scorePulse?.Kill();
            scoreText.transform.localScale = scoreScale;
            if (value > 0) scorePulse = scoreText.transform.DOPunchScale(Vector3.one * 0.08f, 0.18f, 1, 0.3f);
        }
        private void UpdateHealth(int value)
        {
            for (int i = 0; i < healthIcons.Length; i++)
                healthIcons[i].color = i < value ? Color.white : new Color(1f, 1f, 1f, 0.15f);
        }

        public void Unbind()
        {
            scorePulse?.Kill();
            scorePulse = null;
            if (scoreText != null && score != null) scoreText.transform.localScale = scoreScale;
            if (score != null) score.Changed -= UpdateScore;
            if (health != null) health.Changed -= UpdateHealth;
            score = null;
            health = null;
        }

        private void OnDestroy() => Unbind();
    }
}
