using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MainStage : MonoBehaviour
{
    [Header("TrinketTransform")]
    [SerializeField] private Transform[] _trinketTransforms;

    private List<GameObject> _itemList;
    private MapData _mainStageData;

    private void Start()
    {
        var startTimeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.Intro);

        if(startTimeLine != null)
        {
            startTimeLine.Play();
        }
        
        StartCoroutine(LoadData("MainStage"));
    }

    private IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as MapData;

        _mainStageData = data;

        CreateItem(_mainStageData);
    }

    private void CreateItem(MapData data)
    {
        _itemList = new List<GameObject>();

        for (int i = 0; i < data.ItemPath.Count; i++)
        {
            GameObject prefab = Resources.Load<GameObject>(data.ItemPath[i]);

            GameObject item = Instantiate(prefab);

            if (_trinketTransforms[i] != null)
            {
                item.transform.position = _trinketTransforms[i].position;
            }

            _itemList.Add(item);
        }
    }
}
