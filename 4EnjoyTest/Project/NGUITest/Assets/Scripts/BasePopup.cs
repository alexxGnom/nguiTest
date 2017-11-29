using UnityEngine;

namespace NGUITest
{
    public class BasePopup : MonoBehaviour
    {
        #region Interface

        public virtual void OpenPopup()
        {
            NGUITools.SetActive(this.gameObject, true);
        }

        public virtual void ClosePopup()
        {
            NGUITools.SetActive(this.gameObject, false);
        }

        #endregion
    }
}
