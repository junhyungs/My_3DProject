using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum InteractionUI_Type
{
    Use,
    Get,
    Ladder
}

public class UIManager : Singleton<UIManager>
{
    [Header("InteractionCanvas")]
    [SerializeField] private Canvas m_InteractionCanvas;

    private PlayerSkill m_currentSkill;
    private PlayerWeapon m_currentWeapon;    

    private Action<PlayerSkill> SkillChangeCallBack;
    private Action<int> PlayerSkillCountCallBack;
    private Action<int> PlayerHpIconChangeCallBack;

    //Interaction UI Dic
    private Dictionary<Transform, GameObject> ActiveUIInstance = new Dictionary<Transform, GameObject>();

    void Start()
    {
        m_currentSkill = SkillManager.Instance.GetCurrentSkill();
    }
    
    public void ItemInteractionUI(Transform itemTransform, InteractionUI_Type interactionType)
    {
        if (!ActiveUIInstance.ContainsKey(itemTransform))
        {
            GameObject ui = PoolManager.Instance.GetInteractionUI(interactionType);

            ui.transform.position = itemTransform.position + new Vector3(1.5f, 0.5f, 0);

            ui.transform.rotation = Quaternion.Euler(51f, 0, 0f);

            ActiveUIInstance.Add(itemTransform, ui);
        }
    }

    public void HideItemInteractionUI(Transform itemTransform ,InteractionUI_Type interactionType)
    {
        if(ActiveUIInstance.ContainsKey(itemTransform))
        {
            PoolManager.Instance.ReturnInteractionUI(ActiveUIInstance[itemTransform], interactionType);
            ActiveUIInstance.Remove(itemTransform);
        }
    }

    //Event
    //HP
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

    //Skill
    public void RequestChangeSkill(PlayerSkill changeSkill)
    {
        m_currentSkill = changeSkill;

        SkillChangeCallBack?.Invoke(m_currentSkill);
    }

    public void RequestChangeSkillCount(int skillCount)
    {
        PlayerSkillCountCallBack?.Invoke(skillCount);
    }

    public void RefreshSkillInfo(Action<PlayerSkill> callBack)
    {
        callBack?.Invoke(m_currentSkill);
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
   
}
