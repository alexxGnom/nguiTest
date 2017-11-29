using UnityEngine;
using System.Collections;

namespace NGUITest
{
    public class LivesPopup : BasePopup
    {
        #region Unity properties

        [SerializeField]
        private UILabel livesLabel;

        [SerializeField]
        private UILabel timerLabel;

        [SerializeField]
        private UIButton useButton;

        [SerializeField]
        private UIButton refillButton;

        [SerializeField]
        private UITable contentTable;

        #endregion

        #region Private fields

        private LivesManager _livesManager;

        private UITweener[] _tweeners = new UITweener[0];

        #endregion

        #region Interface

        public override void OpenPopup()
        {
            base.OpenPopup();

            if (_tweeners.Length > 0)
            {
                foreach (var tw in _tweeners)
                {
                    tw.PlayForward();
                }
            }
        }

        public override void ClosePopup()
        {
            StartCoroutine(ClosePopupRoutine());
        }

        #endregion

        #region Private methods

        private void Awake()
        {
            _tweeners = GetComponentsInChildren<UITweener>();
        }

        private void Start()
        {
            _livesManager = LivesManager.Instance;

            _livesManager.TimerPerSecondCallback += UpdateTimer;
            _livesManager.LivesChangedCallback += UpdateLives;

            EventDelegate.Add(useButton.onClick, _livesManager.UseLife);
            EventDelegate.Add(refillButton.onClick, _livesManager.RefillLives);

            UpdateLives();
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            var time = _livesManager.CurrentTime;
            timerLabel.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
        }

        private void UpdateLives()
        {
            CheckButtons();

            livesLabel.text = _livesManager.LivesCount.ToString();
        }

        private void CheckButtons()
        {
            var lives = _livesManager.LivesCount;

            if (lives == 0)
                NGUITools.SetActive(useButton.gameObject, false);
            else
                NGUITools.SetActive(useButton.gameObject, true);

            if (lives == _livesManager.MaxLivesCount)
            {
                NGUITools.SetActive(refillButton.gameObject, false);
                NGUITools.SetActive(timerLabel.gameObject, false);
            }
            else
            {
                NGUITools.SetActive(refillButton.gameObject, true);
                NGUITools.SetActive(timerLabel.gameObject, true);
            }

            contentTable.Reposition();
        }

        private IEnumerator ClosePopupRoutine()
        {
            var timeWait = 0f;

            if (_tweeners.Length > 0)
            {
                foreach (var tw in _tweeners)
                {
                    timeWait = Mathf.Max(timeWait, tw.duration + tw.delay);
                    tw.PlayReverse();
                }
            }

            yield return new WaitForSeconds(timeWait);

            base.ClosePopup();
        }

        #endregion
    }
}
