using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    private ForestMotherData _data;
    private ForestMotherProjectileData _projectileData;
    public ForestMotherData MotherData => _data;
    public ForestMotherProjectileData ProjectileData => _projectileData;

    #region interface
    private List<ISendVineEvent> _vineEvent;
    #endregion

    private void Start()
    {
        StartCoroutine(LoadMotherData("B101"));
        StartCoroutine(LoadMotherProjectileData("BP101"));
    }

    private IEnumerator LoadMotherData(string id)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("ForestMother �����͸� �������� ���߽��ϴ�.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as ForestMotherData;
        _data = data;
    }

    private IEnumerator LoadMotherProjectileData(string id)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("ForestMotherProjectile �����͸� �������� ���߽��ϴ�.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as ForestMotherProjectileData;

        _projectileData = data;

        ObjectPool.Instance.CreatePool(ObjectName.MotherProjectile, 10);
    }

    #region SendVine
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
    #endregion
}
