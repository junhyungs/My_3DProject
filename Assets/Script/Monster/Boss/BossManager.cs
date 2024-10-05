using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    private ForestMotherData _data;
    public ForestMotherData MotherData => _data;

    #region interface
    private List<ISendVineEvent> _vineEvent;
    #endregion

    private void Start()
    {
        StartCoroutine(LoadMotherData("B101"));
    }

    private IEnumerator LoadMotherData(string id)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("ForestMother 데이터를 가져오지 못했습니다.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as ForestMotherData;
        _data = data;
    }

    public void RegisetVine(ISendVineEvent vineEvent)
    {
        if(_vineEvent == null)
        {
            _vineEvent  = new List<ISendVineEvent>();
        }

        _vineEvent.Add(vineEvent);
    }

    public void AddVineEvent(Action<Vine, float> callBack, bool isRegistered)
    {
        foreach(var vine in  _vineEvent)
        {
            vine.AddVineEvent(callBack, isRegistered);
        }
    }
}
