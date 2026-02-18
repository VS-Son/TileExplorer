using System.Collections.Generic;
using Project.Scripts.Manager;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Project.Scripts.UI.Manager
{
    [Serializable]
    public class ListScreen
    {
        public string id;
        public string screenName;
    }
    public class UIManager : Singleton<UIManager>
    {
         public Transform canvasParentTf;

         public List<ListScreen> listScreen;

         private Dictionary<string, ListScreen> _uiScreen = new();
         //dict for UI active
        private readonly Dictionary<Type, UICanvas> _uiCanvas = new();

        //dict for quick query UI prefab
        private readonly Dictionary<Type, UICanvas> _uiCanvasPrefab = new();

        //list from resource
        private UICanvas[] _uiResources;

        #region Canvas

        public T OpenUI<T>() where T : UICanvas
        {
            UICanvas canvas = GetUI<T>();

            canvas.Setup();
            canvas.Open();

            return canvas as T;
        }

        public void CloseUI<T>() where T : UICanvas
        {
            if (IsOpened<T>()) GetUI<T>().CloseDirectly();
        }

        public bool IsOpened<T>() where T : UICanvas
        {
            return IsLoaded<T>() && _uiCanvas[typeof(T)].gameObject.activeInHierarchy;
        }


        public bool IsLoaded<T>() where T : UICanvas
        {
            var type = typeof(T);
            return _uiCanvas.ContainsKey(type) && _uiCanvas[type] != null;
        }

        public T GetUI<T>() where T : UICanvas
        {
            if (!IsLoaded<T>())
            {
                UICanvas canvas = Instantiate(GetUIPrefab<T>(), canvasParentTf);
                _uiCanvas[typeof(T)] = canvas;
            }

            return _uiCanvas[typeof(T)] as T;
        }


        private T GetUIPrefab<T>() where T : UICanvas
        {
            if (!_uiCanvasPrefab.ContainsKey(typeof(T)))
            {
                _uiResources ??= Resources.LoadAll<UICanvas>("UI/Screen");
                foreach (var uiCanvas in _uiResources)
                    if (uiCanvas is T)
                    {
                        _uiCanvasPrefab[typeof(T)] = uiCanvas;
                        break;
                    }
            }

            return _uiCanvasPrefab[typeof(T)] as T;
        }

        #endregion

        #region Back Button

        private readonly Dictionary<UICanvas, UnityAction> _backActionEvents = new();
        private readonly List<UICanvas> _backCanvas = new();

        private UICanvas BackTopUI
        {
            get
            {
                UICanvas canvas = null;
                if (_backCanvas.Count > 0) canvas = _backCanvas[_backCanvas.Count - 1];

                return canvas;
            }
        }


        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Escape) && BackTopUI != null) _backActionEvents[BackTopUI]?.Invoke();
        }

        public void PushBackAction(UICanvas canvas, UnityAction action)
        {
            if (!_backActionEvents.ContainsKey(canvas)) _backActionEvents.Add(canvas, action);
        }

        public void AddBackUI(UICanvas canvas)
        {
            if (!_backCanvas.Contains(canvas)) _backCanvas.Add(canvas);
        }

        public void RemoveBackUI(UICanvas canvas)
        {
            _backCanvas.Remove(canvas);
        }

        /// <summary>
        ///     CLear backey when comeback index UI canvas
        /// </summary>
        public void ClearBackKey()
        {
            _backCanvas.Clear();
        }

        #endregion
    }
}
