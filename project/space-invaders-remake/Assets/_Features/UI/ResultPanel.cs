using TMPro;
using UnityEngine;
using DG.Tweening;

namespace SpaceInvaders
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class ResultPanel : MonoBehaviour
    {
        [Header("Result Labels")]
        [Tooltip("Label displaying the victory or defeat title.")]
        [SerializeField] private TMP_Text title;

        [Tooltip("Label displaying the score at the end of the round.")]
        [SerializeField] private TMP_Text finalScore;

        private CanvasGroup canvasGroup;
        private Tween entrance;
        private Vector3 titleScale;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            titleScale = title.transform.localScale;
        }

        public void Show(GameState state, int score)
        {
            title.text = state == GameState.Won ? "SECTOR CLEAR" : "GAME OVER";
            finalScore.text = "SCORE " + score.ToString("D5");
            gameObject.SetActive(true);
            entrance?.Kill();
            canvasGroup.alpha = 0f;
            title.transform.localScale = titleScale * 0.88f;
            entrance = DOTween.Sequence()
                .AppendInterval(0.1f)
                .Append(canvasGroup.DOFade(1f, 0.22f))
                .Join(title.transform.DOScale(titleScale, 0.22f).SetEase(Ease.OutCubic));
        }

        public void Hide() => gameObject.SetActive(false);

        private void OnDisable()
        {
            entrance?.Kill();
            entrance = null;
        }
    }
}
