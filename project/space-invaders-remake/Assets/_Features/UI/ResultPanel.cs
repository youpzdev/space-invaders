using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public sealed class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text finalScore;
        public void Show(GameState state, int score)
        {
            title.text = state == GameState.Won ? "SECTOR CLEAR" : "GAME OVER";
            finalScore.text = "SCORE " + score.ToString("D5");
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
