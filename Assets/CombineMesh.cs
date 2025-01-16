#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class CombineMesh : MonoBehaviour
{
    [Header("CombineObject")] //�ڽ� ������Ʈ���� MeshFilter�� �߰��ϱ� ���� GameObject�� ����.
    [SerializeField] private GameObject[] _combineObjectArray;

    [Header("CombineObjectName")]
    [SerializeField] private string _name;

    [Header("SavePath")]
    [SerializeField] private string _savePath;

    [Header("MakeStatic")]
    [SerializeField] private bool _isStatic;
    [Header("DestroyObject")]
    [SerializeField] private bool _isDestroy;

    public void MeshCombine()
    {
        if(_combineObjectArray == null)
        {
            Debug.Log("������ �޽��� �����ϴ�.");
            return;
        }

        var meshFilterList = GetMeshFilterList();

        CreateMeshObject(meshFilterList);

        if (_isDestroy)
        {
            DestroyObject(_combineObjectArray);
        }

        ResetCombine();
    }

    private List<MeshFilter> GetMeshFilterList()
    {
        var meshFilterList = new List<MeshFilter>();    

        foreach(var gameObject in _combineObjectArray)
        {
            meshFilterList.AddRange(gameObject.GetComponentsInChildren<MeshFilter>());
        }

        return meshFilterList;
    }

    private void CreateMeshObject(List<MeshFilter> meshFilters)
    {
        var meshDictionary = new Dictionary<Material, List<MeshFilter>>();

        foreach(var meshFilter in meshFilters)
        {
            if (meshFilter.TryGetComponent(out MeshRenderer meshRenderer))
            {
                var material = meshRenderer.sharedMaterial;

                if (!meshDictionary.ContainsKey(material))
                {
                    meshDictionary[material] = new List<MeshFilter>();
                }

                meshDictionary[material].Add(meshFilter);
            }
            else
                continue;
        }

        var combineParentObject = new GameObject(_name);

        foreach(var keyValuePair in meshDictionary)
        {
            var material = keyValuePair.Key;

            var meshFilterList = keyValuePair.Value;

            CreateObject(meshFilterList, material, combineParentObject);
        }

    }

    private void CreateObject(List<MeshFilter> meshFilterList, Material material, GameObject parentObject)
    {
        List<CombineInstance> combineInstanceList = new List<CombineInstance>();

        int totalVertexCount = 0;

        foreach(var meshFilter in meshFilterList)
        {

            if(meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.Log("meshFilter�� Null �Ǵ� sharedMesh�� Null�Դϴ�.");
                continue;
            }

            combineInstanceList.Add(new CombineInstance
            {
                mesh = meshFilter.sharedMesh,
                transform = meshFilter.transform.localToWorldMatrix
            });

            totalVertexCount += meshFilter.sharedMesh.vertexCount;
        }

        var childObject = new GameObject(material.name);

        childObject.transform.SetParent(parentObject.transform, true);

        if (_isStatic)
        {
            childObject.isStatic = true;
        }

        var newMesh = new Mesh();
        newMesh.indexFormat = totalVertexCount > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;

        try
        {
            newMesh.CombineMeshes(combineInstanceList.ToArray(), true, true);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        SaveMesh(newMesh, $"{_name}_CombineMesh");
        
        var newMeshFilter = childObject.AddComponent<MeshFilter>();
        newMeshFilter.sharedMesh = newMesh;

        var newMeshRenderer = childObject.AddComponent<MeshRenderer>();
        newMeshRenderer.sharedMaterial = material;
    }

    private void SaveMesh(Mesh mesh, string meshName)
    {
#if UNITY_EDITOR //�����Ϳ����� ó���� �� �ֵ���.
        string path = $"{_savePath}/{meshName}.asset";

        path = AssetDatabase.GenerateUniqueAssetPath(path);

        AssetDatabase.CreateAsset(mesh, path);

        AssetDatabase.SaveAssets();

        Debug.Log("������ ����Ǿ����ϴ�.");

    #endif
    }

    public void ResetCombine()
    {
        _combineObjectArray = null;
        _name = null;   
    }

    private void DestroyObject(GameObject[] combineObjects)
    {//������ ��忡���� Destroy�� �ƴ϶� DestroyImmediate�� �����ؾ���. �� ��쿡 Undo ����� ��� �ǵ����⸦ ����(control + Z) �׷��� Undo.DestroyObjectImmediate�� ���.
        for (int i = 0; i < combineObjects.Length; i++)
        {
            if (combineObjects[i] != null)
            {
                Undo.DestroyObjectImmediate(combineObjects[i]);
            }
        }
    }
}
#endif
