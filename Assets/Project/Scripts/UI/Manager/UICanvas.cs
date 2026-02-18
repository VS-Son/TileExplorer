using UnityEngine;

namespace Project.Scripts.UI.Manager
{
    public class UICanvas : MonoBehaviour
    {
        public bool isDestroyOnClose = false;

        protected RectTransform m_RectTransform;

        private void Start()
        {
            OnInit();
        }

        protected void OnInit()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        public virtual void Setup()
        {
            UIManager.Instance.AddBackUI(this);
            UIManager.Instance.PushBackAction(this, BackKey);
        }

        public virtual void BackKey()
        {
            // Mặc định không làm gì, override ở UI con
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void CloseDirectly()
        {
            UIManager.Instance.RemoveBackUI(this);
            gameObject.SetActive(false);
            if (isDestroyOnClose)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Close(float delayTime)
        {
            Invoke(nameof(CloseDirectly), delayTime);
        }
    }
}
