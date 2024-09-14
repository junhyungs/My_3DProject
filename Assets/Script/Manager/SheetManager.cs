using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class SheetManager : MonoBehaviour
{
    public enum JsonName
    {
        Monster,
        Boss,
        PlayerSkill,
        PlayerWeapon
    }

    const string _monsterDataPath = "https://script.google.com/macros/s/AKfycbzGxeBRVwKLdZ3tjSWQIzMWvoSTRlVlC50D6NYysEoE-PK3D4EoMFgY6VFdkFWKfrTavg/exec";

    private void Start()
    {
        StartCoroutine(SaveJsonData(JsonName.Monster, _monsterDataPath));
    }

    public IEnumerator SaveJsonData(JsonName name, string url)
    {
        string fileName = name.ToString();

        var path = Path.Combine(Application.dataPath, $"Resources/Data/{fileName}.json");

        if(File.Exists(path))
        {
            Debug.Log(" �̹� �ش� ������ �����մϴ�.");
            ReadData(path);
        }
        else
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

            yield return unityWebRequest.SendWebRequest();

            if(unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("�����͸� �������� ���߽��ϴ�");
            }
            else
            {
                string data = unityWebRequest.downloadHandler.text;

                File.WriteAllText(path, data);
            }
        }
    }

    private void ReadData(string path)
    {

    }


}
