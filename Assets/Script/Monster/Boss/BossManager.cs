using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    private ForestMotherData _data;
    public ForestMotherData MotherData => _data;

    private void Start()
    {
        StartCoroutine(LoadMotherData("B101"));
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
}
