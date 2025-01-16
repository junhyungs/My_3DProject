using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityUI : MonoBehaviour
{
    private enum AbilityValue
    {
        Ability_1,
        Ability_2,
        Ability_3,
        Ability_4
    }

    private GameObject _abilityUI;
    private AbilityPanel _panel;

    private void Awake()
    {
        _abilityUI = transform.GetChild(0).gameObject;

        _panel = _abilityUI.GetComponent<AbilityPanel>();
    }

    private void OnEnable()
    {
        UIManager._onAbilityUI += OnAbility;
    }

    private void OnDisable()
    {
        UIManager._onAbilityUI -= OnAbility;
    }

    private void Start()
    {
        StartCoroutine(LoadAbilityData());
    }

    private IEnumerator LoadAbilityData()
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData("Ability_1") == null;
        });

        Array enumArray = Enum.GetValues(typeof(AbilityValue));

        List<AbilityData> dataList = new List<AbilityData>();

        for(int i = 0; i < enumArray.Length; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as AbilityData;

            dataList.Add(data);
        }
        _panel.DataList = dataList;
    }

    private void OnAbility()
    {
        _abilityUI.SetActive(true);
    }
}
