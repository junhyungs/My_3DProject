using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public static event Action _onAbilityUI;
    public static event Action<bool> _loadingUI;
    public static event Action<bool> _deathUI;
    public static event Action<bool> _initializeUI;
    public static event Action _onMapNameUI;

    private Dictionary<MVVM, Delegate> _playerUIEvent = new Dictionary<MVVM, Delegate>();

    private void Start()
    {
        ObjectPool.Instance.CreatePool(ObjectName.UseUI, 3);
        ObjectPool.Instance.CreatePool(ObjectName.GetUI, 3);
        ObjectPool.Instance.CreatePool(ObjectName.LadderUI, 3);
        ObjectPool.Instance.CreatePool(ObjectName.OpenUI, 3);
        ObjectPool.Instance.CreatePool(ObjectName.ThankYouUI, 3);
        ObjectPool.Instance.CreatePool(ObjectName.InteractionDialogueUI, 3);
    }

    #region Event
    public void OnAbilityUI()
    {
        _onAbilityUI.Invoke();
    }

    public void OnLoadingUI(bool isStart)
    {
        _loadingUI.Invoke(isStart);
    }

    public void OnDeathUI(bool isActive)
    {
        _deathUI.Invoke(isActive);
    }

    public void OnInitializeImage(bool isActive)
    {
        _initializeUI.Invoke(isActive);
    }

    public void OnMapNameUI()
    {
        _onMapNameUI.Invoke();
    }
    #endregion

    #region Interaction
    //Interaction UI Dic
    private Dictionary<Transform, GameObject> ActiveUIInstance = new Dictionary<Transform, GameObject>();

    public void ItemInteractionUI(Transform itemTransform, Vector3 uiPosition, ObjectName uiName)
    {
        if (!ActiveUIInstance.ContainsKey(itemTransform))
        {
            switch (uiName)
            {
                case ObjectName.UseUI:
                    GameObject useUI = ObjectPool.Instance.DequeueObject(ObjectName.UseUI);
                    InteractionUIPosition(useUI, itemTransform, uiPosition);
                    break;
                case ObjectName.GetUI:
                    GameObject getUI = ObjectPool.Instance.DequeueObject(ObjectName.GetUI);
                    InteractionUIPosition(getUI, itemTransform, uiPosition);
                    break;
                case ObjectName.LadderUI:
                    GameObject ladderUI = ObjectPool.Instance.DequeueObject(ObjectName.LadderUI);
                    InteractionUIPosition(ladderUI, itemTransform, uiPosition);
                    break;
                case ObjectName.OpenUI:
                    GameObject openUI = ObjectPool.Instance.DequeueObject(ObjectName.OpenUI);
                    InteractionUIPosition (openUI, itemTransform, uiPosition);
                    break;
                case ObjectName.ThankYouUI:
                    GameObject thankyouUI = ObjectPool.Instance.DequeueObject(ObjectName.ThankYouUI);
                    InteractionUIPosition(thankyouUI, itemTransform, uiPosition);
                    break;
            }
        }
    }

    public void InteractionDialogueUI(Transform npcTransform, Vector3 uiPosition)
    {
        if (!ActiveUIInstance.ContainsKey(npcTransform))
        {
            GameObject dialogUI = ObjectPool.Instance.DequeueObject(ObjectName.InteractionDialogueUI);

            InteractionUIPosition(dialogUI, npcTransform, uiPosition);
        }
    }

    public void HideItemInteractionUI(Transform itemTransform ,ObjectName uiType)
    {
        if(ActiveUIInstance.ContainsKey(itemTransform))
        {
            ObjectPool.Instance.EnqueueObject(ActiveUIInstance[itemTransform], uiType);
            ActiveUIInstance.Remove(itemTransform);
        }
    }

    private void InteractionUIPosition(GameObject ui, Transform itemTransform, Vector3 position)
    {
        ui.transform.position = itemTransform.position + position;
        ui.transform.rotation = Quaternion.Euler(51f, 0f, 0f);
        ActiveUIInstance.Add(itemTransform, ui);
    }
    #endregion

    #region MVVM
    public void RegisterUIManager<T>(MVVM key, Action<T> callBack)
    {
        if (!_playerUIEvent.ContainsKey(key))
        {
            _playerUIEvent.Add(key, callBack);
        }
    }

    public void UnRegisterUIManager<T>(MVVM key, Action<T> callBack)
    {
        if (_playerUIEvent.ContainsKey(key))
        {
            _playerUIEvent[key] = Delegate.Remove(_playerUIEvent[key], callBack);
        }
    }

    public void TriggerEvent<T>(MVVM key, T value)
    {
        if(_playerUIEvent.TryGetValue(key, out Delegate callBack))
        {
            (callBack as Action<T>)?.Invoke(value);
        }
    }
    #endregion
}
