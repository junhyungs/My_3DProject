using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class SheetManager : MonoBehaviour
{
    const string _monsterDataPath = "https://script.google.com/macros/s/AKfycbwQFqkt8VSFxyAAm-WVklWQJeC7Qe83SDwx-eVLxMRov16002ukbPQRpOPWiM1otWmOtQ/exec";
    const string _bossDataPath = "https://script.google.com/macros/s/AKfycbxW6S5vqRPWTEODXXg5wH8YMeCL8bcQtxoZYEIgMDhFCZy1oEtTSdpdxR5ZSeTpJskx/exec";
    const string _playerDataPath = "https://script.google.com/macros/s/AKfycbzMh0vG80vBfRFabsD6m2ZylqQ9KtwfKcpugyYHxWUNIxXRu7hwLHdMDT1jK0qDzugU/exec";
    const string _playerSkillPath = "https://script.google.com/macros/s/AKfycbxhWqsuLuLUq3v2fbhn5nsmBmw-6rlojQhwPqcVI7eyMnker25SHIBw_1OGixucLMd7/exec";
    const string _playerWeaponPath = "https://script.google.com/macros/s/AKfycbxbq7qDRyj1pAtfOzrkKGTi7erKqjpu7sIAXN1WeDu_ETys0dXQofrNXEGtpRte3AqA/exec";
    const string _PrefabPath = "https://script.google.com/macros/s/AKfycbxN2axOwanrYCeupzvIkWa-QHgbkBfYh8BjL2V8uNcQFCaq6er3uhauN3JhzsF-XiDl/exec";

    [Header("SaveJson")]
    [SerializeField] private bool _saveJson;

    private void Awake()
    {
        if (_saveJson)
        {
            StartCoroutine(SaveJsonData(JsonName.Monster, _monsterDataPath));
            StartCoroutine(SaveJsonData(JsonName.Player, _playerDataPath));
            StartCoroutine(SaveJsonData(JsonName.Boss, _bossDataPath));
            StartCoroutine(SaveJsonData(JsonName.PlayerWeapon, _playerWeaponPath));
            StartCoroutine(SaveJsonData(JsonName.PlayerSkill, _playerSkillPath));
            StartCoroutine(SaveJsonData(JsonName.PrefabPath, _PrefabPath));
        }

        StartCoroutine(LoadJsonData(JsonName.Monster, _monsterDataPath));
        StartCoroutine(LoadJsonData(JsonName.Player, _playerDataPath));
        StartCoroutine(LoadJsonData(JsonName.Boss, _bossDataPath));
        StartCoroutine(LoadJsonData(JsonName.PlayerWeapon, _playerWeaponPath));
        StartCoroutine(LoadJsonData(JsonName.PlayerSkill, _playerSkillPath));
        StartCoroutine(LoadJsonData(JsonName.PrefabPath, _PrefabPath));
    }

    public IEnumerator LoadJsonData(JsonName name, string url)
    {
        string fileName = name.ToString();

        var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

        if(File.Exists(path))
        {
            Debug.Log(" �̹� �ش� ������ �����մϴ�.");
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

            var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

            File.WriteAllText(path, data);
        }
    }

    private void ReadData(string fileName)
    {
        var jsonData = Resources.Load<TextAsset>($"Data/{fileName}");

        DataManager.Instance.SetData(fileName, jsonData.text);
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

}
