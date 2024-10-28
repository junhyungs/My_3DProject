using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public static event Action _onAbilityUI;
    public static event Action<ResourceRequest> _loadingUI;
    public static event Action<bool> _deathUI;
    public static event Action<bool> _initializeUI;
    public static event Action _onMapNameUI;


    private PlayerSkill m_currentSkill;
    private Action<PlayerSkill> SkillChangeCallBack;
    private Action<int> PlayerSkillCountCallBack;
    private Action<int> PlayerHpIconChangeCallBack;
    private Func<IEnumerator> _globalExitDoorFunc;
    private IExitDoor _exitEvent;

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

    public void OnLoadingUI(ResourceRequest request = null)
    {
        _loadingUI.Invoke(request);
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

    public void RegisterExitEvent(IExitDoor exitDoor)
    {
        _exitEvent = exitDoor;

        if(_globalExitDoorFunc != null)
        {
            exitDoor.ExitUIEvent(_globalExitDoorFunc, true);
        }
    }

    public void AddExitEvent(Func<IEnumerator> coroutineCallBack, bool register)
    {
        _globalExitDoorFunc = coroutineCallBack;

        if(_exitEvent != null)
        {
            _exitEvent.ExitUIEvent(coroutineCallBack, register);
        }
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

    #region MVVM_HP
    public void RequestChangeHp(int hp)
    {
        PlayerHpIconChangeCallBack?.Invoke(hp);
    }

    public void RegisterChangeHpCallBack(Action<int> hpCallBack)
    {
        PlayerHpIconChangeCallBack += hpCallBack;
    }

    public void UnRegisterChangeHpCallBack(Action<int> hpCallBack)
    {
        PlayerHpIconChangeCallBack -= hpCallBack;
    }
    #endregion

    #region MVVM_Skill
    public void RequestChangeSkill(PlayerSkill changeSkill)
    {
        m_currentSkill = changeSkill;

        SkillChangeCallBack?.Invoke(m_currentSkill);
    }

    public void RequestChangeSkillCount(int skillCount)
    {
        PlayerSkillCountCallBack?.Invoke(skillCount);
    }

    public void RegisterChangeSkillCountCallBack(Action<int> skillCountCallBack)
    {
        PlayerSkillCountCallBack += skillCountCallBack;
    }

    public void UnRegisterChangeSkillCountCallBack(Action<int> skillCountCallBack)
    {
        PlayerSkillCountCallBack -= skillCountCallBack;
    }

    public void RegisterChangeSkillCallBack(Action<PlayerSkill> skillChangeCallBack)
    {
        SkillChangeCallBack += skillChangeCallBack;
    }

    public void UnRegisterChangeSkillCallBack(Action<PlayerSkill> skillChangeCallBack)
    {
        SkillChangeCallBack -= skillChangeCallBack;
    }
    #endregion
}
