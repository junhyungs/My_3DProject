using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


public class SheetManager : MonoBehaviour
{
    #region Unit
    const string _monsterDataPath = "https://script.google.com/macros/s/AKfycbwQFqkt8VSFxyAAm-WVklWQJeC7Qe83SDwx-eVLxMRov16002ukbPQRpOPWiM1otWmOtQ/exec";
    const string _bossDataPath = "https://script.google.com/macros/s/AKfycbxW6S5vqRPWTEODXXg5wH8YMeCL8bcQtxoZYEIgMDhFCZy1oEtTSdpdxR5ZSeTpJskx/exec";
    const string _bossProjectilePath = "https://script.google.com/macros/s/AKfycbwbMtYcLvktX_C7LzfUixOe1t7WnImnCFjXUF38B727H2cogk8F8NmBEWAd6o-NzJWw1A/exec";
    const string _playerDataPath = "https://script.google.com/macros/s/AKfycbzMh0vG80vBfRFabsD6m2ZylqQ9KtwfKcpugyYHxWUNIxXRu7hwLHdMDT1jK0qDzugU/exec";
    const string _playerSkillPath = "https://script.google.com/macros/s/AKfycbxhWqsuLuLUq3v2fbhn5nsmBmw-6rlojQhwPqcVI7eyMnker25SHIBw_1OGixucLMd7/exec";
    const string _playerWeaponPath = "https://script.google.com/macros/s/AKfycbxbq7qDRyj1pAtfOzrkKGTi7erKqjpu7sIAXN1WeDu_ETys0dXQofrNXEGtpRte3AqA/exec";
    const string _PrefabPath = "https://script.google.com/macros/s/AKfycbxN2axOwanrYCeupzvIkWa-QHgbkBfYh8BjL2V8uNcQFCaq6er3uhauN3JhzsF-XiDl/exec";
    #endregion

    #region Dialogue
    const string _dialoguePath = "https://script.google.com/macros/s/AKfycbw75qKMaJlorm7F7D1MGmliZCFX_lPYAOuk-k9QISSpejEfYgaQ0SiZN0QdD_qJ7HOD/exec";
    #endregion

    #region Item
    const string _itemDataPath = "https://script.google.com/macros/s/AKfycbxvjrAtBLmy0WiiQIMxDADV2APHNpKifaW3F_idjogUQkHlbzmuAJQjFHunRYJAXf7_7Q/exec";
    #endregion

    #region Ability
    const string _abilityPath = "https://script.google.com/macros/s/AKfycbwGjSAV99tGtWeAXyRzonORHNMtmDuzwP28AM7wFtiXquHaL-S6ErKpNV3HyQ2vFuCj/exec";
    #endregion

    #region Map
    const string _mapDataPath = "https://script.google.com/macros/s/AKfycbzCRou8ldvpGxo2SxWF03p_2onC-QSrOeHptV983lEdz_74R85IUmDAr5q-23YJcgft/exec";
    #endregion

    const string _testPost = "https://script.google.com/macros/s/AKfycbwY8fVsfmZXyi3dZqyuyQT8TtNj30hk2xGh7HZTcGgDtyhHGwchcGHpTF6lszbXDg7W/exec";

    [Header("SaveJson")]
    [SerializeField] private bool _saveJson;

    private void Awake()
    {
        StartSheetManager();
    }

    private void StartSheetManager()
    {
        var jsonList = new List<(JsonName, string)>
        {
            {(JsonName.Monster, _monsterDataPath)},
            {(JsonName.Player, _playerDataPath)},
            {(JsonName.Boss, _bossDataPath)},
            {(JsonName.PlayerWeapon, _playerWeaponPath)},
            {(JsonName.PlayerSkill, _playerSkillPath)},
            {(JsonName.PrefabPath, _PrefabPath)},
            {(JsonName.BossProjectile, _bossProjectilePath)},
            {(JsonName.Dialogue, _dialoguePath)},
            {(JsonName.Item, _itemDataPath)},
            {(JsonName.Ability, _abilityPath)},
            {(JsonName.Map, _mapDataPath)}
        };

        if (_saveJson)
        {
            SaveJson(jsonList);
        }
        else
        {
            LoadJson(jsonList);
        }
    }

    private void SaveJson(List<(JsonName, string)> pathList)
    {
        foreach(var(jsonName, path) in pathList)
        {
            StartCoroutine(SaveJsonData(jsonName, path));
        }
    }

    private void LoadJson(List<(JsonName, string)> pathList)
    {
        foreach (var (jsonName, path) in pathList)
        {
            StartCoroutine(LoadJsonData(jsonName, path));
        }
    }

    public IEnumerator LoadJsonData(JsonName name, string url)
    {
        string fileName = name.ToString();

        //var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");
        var path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        if (File.Exists(path))
        {
            ReadData(fileName);
        }
        else
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("�����͸� �������� ���߽��ϴ�");
            }
            else
            {
                string data = unityWebRequest.downloadHandler.text;

                File.WriteAllText(path, data);

                ReadData(fileName);
            }
        }
    }

    public IEnumerator SaveJsonData(JsonName name, string url)
    {
        string fileName = name.ToString();

        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

        yield return unityWebRequest.SendWebRequest();

        if(unityWebRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("�����͸� �������� ���߽��ϴ�");
        }
        else
        {
            string data = unityWebRequest.downloadHandler.text;

            try
            {
                //�������� ���� string���̽� �����͸� ��ü�� ��ȯ
                var jsonObject = JsonConvert.DeserializeObject(data);
                //������ : ��ü�� �鿩���⸦ �����ؼ� �ٽ� ���̽� ���ڿ��� ��ȯ
                string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                Debug.Log(formattedJson);

                var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

                File.WriteAllText(path, data);

                ReadData(fileName);
            }
            catch(Exception ex)
            {
                Debug.LogError("���̽� ��ȯ �� ������ �߻��߽��ϴ�." +  ex.Message);
            }
        }
    }

    private void ReadData(string fileName)
    {
        var jsonData = Resources.Load<TextAsset>($"Data/{fileName}");

        DataManager.Instance.SetData(fileName, jsonData.text);
    }

    private void Start()
    {
        var playerData = DataManager.Instance.GetData("P101") as PlayerData;

        if(playerData != null)
        {
            Debug.Log(playerData.SpeedOffSet);
            StartCoroutine(TestPost(playerData));
            Debug.Log("��û����");
        }
    }

    public IEnumerator TestPost(PlayerData data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
        Debug.Log(jsonData);
        WWWForm form = new WWWForm();

        form.AddField("json", jsonData);

        UnityWebRequest request = UnityWebRequest.Post(_testPost, form);

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("����!");
        }
        else
        {
            Debug.Log("����!");
        }
    }

    //function doGet(e)
    //{
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("��Ʈ1"); //���� Ȱ��ȭ �Ǿ��ִ� ��Ʈ�� ������.

    //    //���� ��Ʈ�� �ִ� ��� �����͸� 2���� �迭�� ������.
    //    var data = sheet.getDataRange().getValues();
    //    //���� ��Ʈ�� ù��° ���� ����� ����ϰ���.
    //    var headers = data[0];
    //    //���̽� ��ü�� ������ �� �迭 ����
    //    var jsonData = [];


    //    for (var i = 1; i < data.length; i++)
    //    {//0��°�� ���, �����ٺ��� ����. data �迭�� ������.
    //        var rowData = { };

    //        for (var j = 0; j < headers.length; j++)
    //        { //�� ���� �ݺ��ϸ� 2���� ���� ����� Ű�� ����Ͽ� �����͸� �߰���.
    //          //if(j!==1) <- � ����� �����ϰ� �ʹٸ�
    //          //{

    //            //}
    //            rowData[headers[j]] = data[i][j];
    //        }
    //        jsonData.push(rowData); //�����͸� �����ϴ� �迭�� �߰�
    //    }
    //    var jsonOut = JSON.stringify(jsonData); //���̽����� ��ȯ

    //    return ContentService.createTextOutput(jsonOut).setMimeType(ContentService.MimeType.JSON);
    //}

    //�鿩����
    //function doGet(e)
    //{
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("��Ʈ1");

    //    var data = sheet.getDataRange().getValues();

    //    var headers = data[0];

    //    var jsonData = [];

    //    for (var i = 1; i < data.length; i++)
    //    {
    //        var rowData = { };

    //        for (var j = 0; j < headers.length; j++)
    //        {
    //            rowData[headers[j]] = data[i][j];
    //        }
    //        jsonData.push(rowData);
    //    }
    //    var jsonOut = JSON.stringify(jsonData, null, 2);

    //    return ContentService.createTextOutput(jsonOut).setMimeType(ContentService.MimeType.JSON);
    //}


  //  function doPost(e)
  //  {
  //      var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("��Ʈ1"); //���� Ȱ��ȭ �� ��Ʈ

  //      var jsonString = e.parameter.json; //form�� ���� �� ������ �ʵ� �̸�

  //      var data = JSON.parse(jsonString); //���̽����� ��ȯ

  //      var headers = Object.keys(data); //�������� �ʵ� �̸��� ������. ex) data.ID �̸� ID�� ������.

  //      // ù ��° �࿡ ����� ���ٸ� ��� �߰�
  //      if (sheet.getLastRow() == 0)
  //      {
  //          sheet.appendRow(headers);
  //      }

  //      var rowData = []; //�����͸� ������ �迭 ����

  //      //��� �迭�� ��ȸ�ϸ� �� ��� ���� Ű������ data �迭���� �ش��ϴ� �����͸� rowData �迭�� �ִ´�.
  //      for (var i = 0; i < headers.length; i++)
  //      {
  //          var header = headers[i];

  //          rowData.push(data[header]);
  //      }

  //      var lastRow = sheet.getLastRow(); //���� ��Ʈ�� ������ ���� ������

  //      if (lastRow > 1)
  //      { //���� �ִٸ�

  //          var currentData = sheet.getDataRange().getValues(); //��Ʈ�� ��� �����͸� ������.
  //          var currentHeaders = currentData[0]; //����� �׻� ù ��° ��

  //          for (var i = 1; i < currentData.length; i++)
  //          { //ù ��° ���� �����ϰ� ��ȸ�ϸ� �����͸� �����

  //              for (var j = 0; j < currentHeaders.length; j++)
  //              {
  //                  currentData[i][j] = rowData[j];
  //              }

  //          }
  //          //����� �����͸� ��Ʈ�� �ݿ�.
  //          sheet.getRange(2, 1, currentData.length - 1, currentData[0].length).setValues(currentData.slice(1));//getRange(���� ��, ���� ��, �����͸� �ݿ��� ���� ����, �������� �� ��).setValues(currentData.slice(1)) -> currentData �迭�� ù ��° ���(���)�� ������ ������ �����͸� �߶󳽴�. (�� ����� �߶󳻰� �����Ͱ� ��ϵ� �ุ ó���ϰڴ�.)
  //      }
  //      else
  //      {
  //          // ������ ���� �߰�
  //          sheet.appendRow(rowData); //appendRow �Ź� ���ο� ���� �����Ѵ�.
  //      }

  //      return ContentService.createTextOutput(
  //        JSON.stringify({ success: true})
  //).setMimeType(ContentService.MimeType.JSON);

  //  }

}
