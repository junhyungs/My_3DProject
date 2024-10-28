using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using System;
using static System.Net.WebRequestMethods;


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
    const string _abilityPath = "https://script.google.com/macros/s/AKfycbyZZJj41jfvj4ppsnwzTau66q2HS6rqTUYHbby65_IUdWa7S0MJcFmQJX3Igp_sxxj5/exec";
    #endregion

    #region Map
    const string _mapDataPath = "https://script.google.com/macros/s/AKfycbzCRou8ldvpGxo2SxWF03p_2onC-QSrOeHptV983lEdz_74R85IUmDAr5q-23YJcgft/exec";
    #endregion


    [Header("SaveJson")]
    [SerializeField] private bool _saveJson;

    [Header("EditJson")]
    [SerializeField] private bool _editJson;

    private void Awake()
    {
        if (_editJson)
        {
            Debug.Log("�����͸� �������Դϴ�. ���� �����͸� �о���� �ʽ��ϴ�.");
            StartCoroutine(EditJson(new string[] { "a", "bc", "def" }, new string[] { "he", "ad", "Er" }, new string[] { }, _abilityPath));
            return;
        }

        if (_saveJson)
        {
            SaveJson();

            return;
        }

        //LoadJson();
    }

    private void SaveJson()
    {
        StartCoroutine(SaveJsonData(JsonName.Monster, _monsterDataPath));
        StartCoroutine(SaveJsonData(JsonName.Player, _playerDataPath));
        StartCoroutine(SaveJsonData(JsonName.Boss, _bossDataPath));
        StartCoroutine(SaveJsonData(JsonName.PlayerWeapon, _playerWeaponPath));
        StartCoroutine(SaveJsonData(JsonName.PlayerSkill, _playerSkillPath));
        StartCoroutine(SaveJsonData(JsonName.PrefabPath, _PrefabPath));
        StartCoroutine(SaveJsonData(JsonName.BossProjectile, _bossProjectilePath));
        StartCoroutine(SaveJsonData(JsonName.Dialogue, _dialoguePath));
        StartCoroutine(SaveJsonData(JsonName.Item, _itemDataPath));
        StartCoroutine(SaveJsonData(JsonName.Ability, _abilityPath));
        StartCoroutine(SaveJsonData(JsonName.Map, _mapDataPath));
    }

    //private void LoadJson()
    //{
    //    StartCoroutine(LoadJsonData(JsonName.Monster, _monsterDataPath));
    //    StartCoroutine(LoadJsonData(JsonName.Player, _playerDataPath));
    //    StartCoroutine(LoadJsonData(JsonName.Boss, _bossDataPath));
    //    StartCoroutine(LoadJsonData(JsonName.PlayerWeapon, _playerWeaponPath));
    //    StartCoroutine(LoadJsonData(JsonName.PlayerSkill, _playerSkillPath));
    //    StartCoroutine(LoadJsonData(JsonName.PrefabPath, _PrefabPath));
    //    StartCoroutine(LoadJsonData(JsonName.BossProjectile, _bossProjectilePath));
    //    StartCoroutine(LoadJsonData(JsonName.Dialogue, _dialoguePath));
    //    StartCoroutine(LoadJsonData(JsonName.Item, _itemDataPath));
    //    StartCoroutine(LoadJsonData(JsonName.Ability, _abilityPath));
    //    StartCoroutine(LoadJsonData(JsonName.Map, _mapDataPath));
    //}

    //public IEnumerator LoadJsonData(JsonName name, string url)
    //{
    //    string fileName = name.ToString();

    //    var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

    //    if(File.Exists(path))
    //    {
    //        ReadData(fileName);
    //    }
    //    else
    //    {
    //        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

    //        yield return unityWebRequest.SendWebRequest();

    //        if (unityWebRequest.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("�����͸� �������� ���߽��ϴ�");
    //        }
    //        else
    //        {
    //            string data = unityWebRequest.downloadHandler.text;

    //            File.WriteAllText(path, data);

    //            ReadData(fileName);
    //        }
    //    }
    //}
    
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

                //File.WriteAllText(path, data);

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

    private IEnumerator EditJson(string[] id, string[] headerName, string[] value, string url)
    {
        WWWForm form = new WWWForm();

        for(int i = 0; i < id.Length; i++)
        {
            form.AddField("ID[]", id[i]);

            form.AddField("HeaderName[]", headerName[i]);

            form.AddField("value[]", value[i]);
        }

        using(UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
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

    //function doPost(e)
    //{
    //    // ��û �������� �� ������ ��������
    //    var idsToUpdate = e.parameters.ID; // �迭�� ��������
    //    var newDescriptionNames = e.parameters.DescriptionName; // �迭�� ��������

    //    // ��Ʈ1�� ������
    //    var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("��Ʈ1");

    //    // ��Ʈ�� ������ ��������
    //    var data = sheet.getDataRange().getValues();

    //    // �����Ϳ��� ����� ã��
    //    var headers = data[0];

    //    // ID�� DescriptionName�� �ε����� ã��
    //    var idIndex = headers.indexOf("ID");
    //    var descriptionNameIndex = headers.indexOf("DescriptionName");

    //    // ID�� �´� �׸��� ã�Ƽ� DescriptionName ����
    //    for (var i = 0; i < idsToUpdate.length; i++)
    //    {
    //        var idToUpdate = idsToUpdate[i];
    //        var newDescriptionName = newDescriptionNames[i];

    //        for (var j = 1; j < data.length; j++)
    //        { // �����ʹ� 1��° �ε������� ����
    //            if (data[j][idIndex] === idToUpdate)
    //            {
    //                data[j][descriptionNameIndex] = newDescriptionName; // DescriptionName ����
    //                break; // ���� �� �ݺ��� ����
    //            }
    //        }
    //    }

    //    // ������ �����͸� �ٽ� ��Ʈ�� ���
    //    sheet.getRange(1, 1, data.length, headers.length).setValues(data);

    //    // ������ ���� (���� �޽��� ��ȯ)
    //    return ContentService.createTextOutput(JSON.stringify({ status: "success", message: "Data updated successfully."}))
    //                     .setMimeType(ContentService.MimeType.JSON);
    //}

}
