using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UICanvas[] uiResources;

    public Transform CanvasParentTF;

    [SerializeField] private bool allowInstantiate = false;

    private readonly Dictionary<System.Type, UICanvas> uiCanvasPrefab = new();
    private readonly Dictionary<System.Type, UICanvas> uiCanvas = new();

    #region Unity Methods
    protected  void Awake()
    {
        
        var existingCanvases = CanvasParentTF.GetComponentsInChildren<UICanvas>(true);
        foreach (var ui in existingCanvases)
        {
            var type = ui.GetType();
            if (!uiCanvas.ContainsKey(type))
            {
                uiCanvas.Add(type, ui);
            }
        }

        foreach (var ui in uiResources)
        {
            if (ui == null) continue;
            var type = ui.GetType();
            if (!uiCanvasPrefab.ContainsKey(type))
                uiCanvasPrefab.Add(type, ui);
        }
    }
    #endregion

    #region Canvas Control
    public T OpenUI<T>() where T : UICanvas
    {
        var canvas = GetUI<T>();
        if (canvas == null)
        {
            return null;
        }

        canvas.Setup();
        canvas.Open();
        return canvas as T;
    }

    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>())
            GetUI<T>().CloseDirectly();
    }

    public void CloseUI<T>(float delayTime) where T : UICanvas
    {
        if (IsOpened<T>())
            GetUI<T>().Close(delayTime);
    }

    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && uiCanvas[typeof(T)].gameObject.activeInHierarchy;
    }

    public bool IsLoaded<T>() where T : UICanvas
    {
        var type = typeof(T);
        return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
    }

    public T GetUI<T>() where T : UICanvas
    {
        System.Type type = typeof(T);

        if (!IsLoaded<T>())
        {
            var prefab = GetUIPrefab<T>();
            if (prefab == null)
            {
               
                return null;
            }
            UICanvas canvas = Instantiate(prefab, CanvasParentTF);
            uiCanvas[type] = canvas;
        }

        return uiCanvas[type] as T;
    }

    private T GetUIPrefab<T>() where T : UICanvas
    {
        var type = typeof(T);

        if (!uiCanvasPrefab.ContainsKey(type))
        {
            for (int i = 0; i < uiResources.Length; i++)
            {
                if (uiResources[i] is T)
                {
                    uiCanvasPrefab[type] = uiResources[i];
                    break;
                }
            }
        }

        return uiCanvasPrefab.ContainsKey(type) ? uiCanvasPrefab[type] as T : null;
    }

    public void CloseAll()
    {
        foreach (var item in uiCanvas)
        {
            if (item.Value != null && item.Value.gameObject.activeInHierarchy)
                item.Value.CloseDirectly();
        }
    }
    #endregion

    #region Back Button
    private readonly Dictionary<UICanvas, UnityAction> BackActionEvents = new();
    private readonly List<UICanvas> backCanvas = new();

    private UICanvas BackTopUI => backCanvas.Count > 0 ? backCanvas[^1] : null;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && BackTopUI != null)
            BackActionEvents[BackTopUI]?.Invoke();
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!BackActionEvents.ContainsKey(canvas))
            BackActionEvents.Add(canvas, action);
    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
            backCanvas.Add(canvas);
    }

    public void RemoveBackUI(UICanvas canvas) => backCanvas.Remove(canvas);

    public void ClearBackKey() => backCanvas.Clear();
    #endregion
}
