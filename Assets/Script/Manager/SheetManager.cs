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

        var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

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

    //function doPost(e)
    //{
    //    // 요청 본문에서 폼 데이터 가져오기
    //    var idsToUpdate = e.parameters.ID; // 배열로 가져오기
    //    var newDescriptionNames = e.parameters.DescriptionName; // 배열로 가져오기

    //    // 시트1을 가져옴
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("시트1");

    //    // 시트의 데이터 가져오기
    //    var data = sheet.getDataRange().getValues();

    //    // 데이터에서 헤더를 찾기
    //    var headers = data[0];

    //    // ID와 DescriptionName의 인덱스를 찾기
    //    var idIndex = headers.indexOf("ID");
    //    var descriptionNameIndex = headers.indexOf("DescriptionName");

    //    // ID가 맞는 항목을 찾아서 DescriptionName 수정
    //    for (var i = 0; i < idsToUpdate.length; i++)
    //    {
    //        var idToUpdate = idsToUpdate[i];
    //        var newDescriptionName = newDescriptionNames[i];

    //        for (var j = 1; j < data.length; j++)
    //        { // 데이터는 1번째 인덱스부터 시작
    //            if (data[j][idIndex] === idToUpdate)
    //            {
    //                data[j][descriptionNameIndex] = newDescriptionName; // DescriptionName 수정
    //                break; // 수정 후 반복문 종료
    //            }
    //        }
    //    }

    //    // 수정된 데이터를 다시 시트에 기록
    //    sheet.getRange(1, 1, data.length, headers.length).setValues(data);

    //    // 응답을 생성 (상태 메시지 반환)
    //    return ContentService.createTextOutput(JSON.stringify({ status: "success", message: "Data updated successfully."}))
    //                     .setMimeType(ContentService.MimeType.JSON);
    //}

}
