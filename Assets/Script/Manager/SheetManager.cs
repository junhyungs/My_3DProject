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
                Debug.Log("데이터를 가져오지 못했습니다");
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
            Debug.Log("데이터를 가져오지 못했습니다");
        }
        else
        {
            string data = unityWebRequest.downloadHandler.text;

            try
            {
                //포맷팅을 위해 string제이슨 데이터를 객체로 변환
                var jsonObject = JsonConvert.DeserializeObject(data);
                //포맷팅 : 객체를 들여쓰기를 적용해서 다시 제이슨 문자열로 변환
                string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                Debug.Log(formattedJson);

                var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

                File.WriteAllText(path, data);

                ReadData(fileName);
            }
            catch(Exception ex)
            {
                Debug.LogError("제이슨 변환 중 문제가 발생했습니다." +  ex.Message);
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
            Debug.Log("요청시작");
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
            Debug.Log("성공!");
        }
        else
        {
            Debug.Log("실패!");
        }
    }

    //function doGet(e)
    //{
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("시트1"); //현재 활성화 되어있는 시트를 가져옴.

    //    //현재 시트에 있는 모든 데이터를 2차원 배열로 가져옴.
    //    var data = sheet.getDataRange().getValues();
    //    //현재 시트의 첫번째 줄을 헤더로 사용하겠음.
    //    var headers = data[0];
    //    //제이슨 개체를 보관할 빈 배열 선언
    //    var jsonData = [];


    //    for (var i = 1; i < data.length; i++)
    //    {//0번째는 헤더, 다음줄부터 실행. data 배열의 끝까지.
    //        var rowData = { };

    //        for (var j = 0; j < headers.length; j++)
    //        { //각 열을 반복하며 2열을 제외 헤더를 키로 사용하여 데이터를 추가함.
    //          //if(j!==1) <- 어떤 헤더를 제외하고 싶다면
    //          //{

    //            //}
    //            rowData[headers[j]] = data[i][j];
    //        }
    //        jsonData.push(rowData); //데이터를 저장하는 배열에 추가
    //    }
    //    var jsonOut = JSON.stringify(jsonData); //제이슨으로 변환

    //    return ContentService.createTextOutput(jsonOut).setMimeType(ContentService.MimeType.JSON);
    //}

    //들여쓰기
    //function doGet(e)
    //{
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("시트1");

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
  //      var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("시트1"); //현재 활성화 된 시트

  //      var jsonString = e.parameter.json; //form을 만들 때 지정한 필드 이름

  //      var data = JSON.parse(jsonString); //제이슨으로 변환

  //      var headers = Object.keys(data); //데이터의 필드 이름을 가져옴. ex) data.ID 이면 ID를 가져옴.

  //      // 첫 번째 행에 헤더가 없다면 헤더 추가
  //      if (sheet.getLastRow() == 0)
  //      {
  //          sheet.appendRow(headers);
  //      }

  //      var rowData = []; //데이터를 보관할 배열 선언

  //      //헤더 배열을 순회하며 각 헤더 값을 키값으로 data 배열에서 해당하는 데이터를 rowData 배열에 넣는다.
  //      for (var i = 0; i < headers.length; i++)
  //      {
  //          var header = headers[i];

  //          rowData.push(data[header]);
  //      }

  //      var lastRow = sheet.getLastRow(); //현재 시트의 마지막 행을 가져옴

  //      if (lastRow > 1)
  //      { //행이 있다면

  //          var currentData = sheet.getDataRange().getValues(); //시트의 모든 데이터를 가져옴.
  //          var currentHeaders = currentData[0]; //헤더는 항상 첫 번째 행

  //          for (var i = 1; i < currentData.length; i++)
  //          { //첫 번째 행을 제외하고 순회하며 데이터를 덮어씌움

  //              for (var j = 0; j < currentHeaders.length; j++)
  //              {
  //                  currentData[i][j] = rowData[j];
  //              }

  //          }
  //          //덮어씌운 데이터를 시트에 반영.
  //          sheet.getRange(2, 1, currentData.length - 1, currentData[0].length).setValues(currentData.slice(1));//getRange(시작 행, 시작 열, 데이터를 반영할 행의 개수, 데이터의 열 수).setValues(currentData.slice(1)) -> currentData 배열의 첫 번째 요소(헤더)를 제외한 나머지 데이터를 잘라낸다. (즉 헤더만 잘라내고 데이터가 기록된 행만 처리하겠다.)
  //      }
  //      else
  //      {
  //          // 데이터 행을 추가
  //          sheet.appendRow(rowData); //appendRow 매번 새로운 행을 생성한다.
  //      }

  //      return ContentService.createTextOutput(
  //        JSON.stringify({ success: true})
  //).setMimeType(ContentService.MimeType.JSON);

  //  }

}
