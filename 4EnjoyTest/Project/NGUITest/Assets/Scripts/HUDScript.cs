using UnityEngine;

namespace NGUITest
{
    public class HUDScript : MonoBehaviour
    {
        #region Unity properties

        [SerializeField]
        private UILabel livesLabel;

        [SerializeField]
        private UILabel timerLabel;  

        [SerializeField]
        private BasePopup livesPopup;

        #endregion

        #region Private fields

        private LivesManager _livesManager;

        #endregion

        #region Interface

        public void OpenLivesPopup()
        {
            livesPopup.OpenPopup();
        }

        #endregion

        #region Private methods

        private void Start()
        {
            _livesManager = LivesManager.Instance;

            _livesManager.TimerPerSecondCallback += UpdateTimer;
            _livesManager.LivesChangedCallback += UpdateLives;

            UpdateLives();
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            var time = _livesManager.CurrentTime;
            if (time == 0)
                timerLabel.text = "Full";
            else
                timerLabel.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
        }

        private void UpdateLives()
        {
            livesLabel.text = _livesManager.LivesCount.ToString();
        }

        #endregion

    }
}
