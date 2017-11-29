using System.Collections;
using UnityEngine;
using System;

namespace NGUITest
{
    public class LivesManager : MonoSingleton<LivesManager>
    {
        #region Unity properties

        [SerializeField]
        private int _maxLivesCount = 5;

        [SerializeField]
        private int nextLiveDelay = 20;

        #endregion

        #region Public properties

        public int LivesCount
        {
            get { return _livesCount; }

            private set
            {
                if (value != _livesCount && value <= MaxLivesCount)
                {
                    _livesCount = value;

                    if (LivesChangedCallback != null)
                        LivesChangedCallback();

                    if (!_timerStarted && _livesCount < MaxLivesCount)
                        StartCoroutine(TimeLeft(nextLiveDelay));
                }
            }
        }

        public int CurrentTime
        {
            get { return _currentTime; }

            set
            {
                if (value != _currentTime)
                {
                    _currentTime = value;
                    if (TimerPerSecondCallback != null)
                        TimerPerSecondCallback();
                }
            }
        }

        public int MaxLivesCount { get { return _maxLivesCount; } }

        public Action TimerPerSecondCallback;
        public Action LivesChangedCallback;

        #endregion

        #region Private fields

        private int _livesCount = 0;

        private bool _timerStarted = false;

        private int _currentTime = 0;

        #endregion

        #region Interface

        public void UseLife()
        {
            if (LivesCount > 0)
                LivesCount--;
            else
                Debug.LogError("Life count may be greater than 0!!!");
        }

        public void AddLife()
        {
            LivesCount++;
        }

        public void RefillLives()
        {
            StopAllCoroutines();
            _timerStarted = false;

            LivesCount = MaxLivesCount;
            CurrentTime = 0;
        }

        #endregion

        #region Private methods

        private void Start()
        {
            LivesCount = MaxLivesCount;
        }

        private IEnumerator TimeLeft(float timeLeft = 30.0f)
        {
            _timerStarted = true;
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                CurrentTime = (int)Mathf.Ceil(timeLeft);
                yield return null;
            }
            _timerStarted = false;

            AddLife();
        }

        #endregion
    }
}
