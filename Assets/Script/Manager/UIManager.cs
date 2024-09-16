using System;
using System.Collections.Generic;
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

    #region Interaction
    //Interaction UI Dic
    private Dictionary<Transform, GameObject> ActiveUIInstance = new Dictionary<Transform, GameObject>();
    
    public void ItemInteractionUI(Transform itemTransform, InteractionUI_Type interactionType)
    {
        if (!ActiveUIInstance.ContainsKey(itemTransform))
        {
            GameObject ui = PoolManager.Instance.GetInteractionUI(interactionType);

            switch (interactionType)
            {
                case InteractionUI_Type.Use:
                    InteractionUIPosition(ui, itemTransform, new Vector3(1.5f, 0.5f, 0f));
                    break;
                case InteractionUI_Type.Get:
                    InteractionUIPosition(ui, itemTransform, new Vector3(1.7f, 0.5f, 0f));
                    break;
                case InteractionUI_Type.Ladder:
                    InteractionUIPosition(ui, itemTransform, new Vector3(-1f, 0.5f, 0f));
                    break;
            }
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

    private void InteractionUIPosition(GameObject ui, Transform itemTransform, Vector3 position)
    {
        ui.transform.position = itemTransform.position + position;
        ui.transform.rotation = Quaternion.Euler(51f, 0f, 0f);
        ActiveUIInstance.Add(itemTransform, ui);
    }
    #endregion

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
