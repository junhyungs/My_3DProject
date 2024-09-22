using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossManager : Singleton<BossManager>
{
    private BT_BossData _data;

    public BT_BossData Data => _data;

    private void Start()
    {
        StartCoroutine(LoadBossData());
    }

    private IEnumerator LoadBossData()
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("���� �����͸� �������� ���߽��ϴ�.");
            return DataManager.Instance.GetData("B101") == null;
        });

        var data = DataManager.Instance.GetData("B101") as BT_BossData;

        _data = data;
    }
}
