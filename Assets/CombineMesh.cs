using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class CombineMesh : MonoBehaviour
{
    [Header("CombineObject")] //자식 오브젝트까지 MeshFilter를 추가하기 위해 GameObject로 받음.
    [SerializeField] private GameObject[] _combineObjectArray;

    [Header("CombineObjectName")]
    [SerializeField] private string _name;

    [Header("MakeStatic")]
    [SerializeField] private bool _isStatic;
    [Header("DestroyObject")]
    [SerializeField] private bool _isDestroy;

    public void MeshCombine()
    {
        if(_combineObjectArray == null)
        {
            Debug.Log("병합할 메쉬가 없습니다.");
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

        if (_isStatic)
        {
            combineParentObject.isStatic = true;
        }

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
            combineInstanceList.Add(new CombineInstance
            {
                mesh = meshFilter.sharedMesh,
                transform = meshFilter.transform.localToWorldMatrix
            });

            totalVertexCount += meshFilter.sharedMesh.vertexCount;
        }

        var childObject = new GameObject(material.name);

        childObject.transform.SetParent(parentObject.transform, false);

        if (_isStatic)
        {
            childObject.isStatic = true;
        }

        var newMesh = new Mesh();
        newMesh.indexFormat = totalVertexCount > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
        newMesh.CombineMeshes(combineInstanceList.ToArray(), true, true);

        var newMeshFilter = childObject.AddComponent<MeshFilter>();
        newMeshFilter.sharedMesh = newMesh;

        var newMeshRenderer = childObject.AddComponent<MeshRenderer>();
        newMeshRenderer.sharedMaterial = material;
    }

    public void ResetCombine()
    {
        _combineObjectArray = null;
        _name = null;   
    }

    private void DestroyObject(GameObject[] combineObjects)
    {//에디터 모드에서는 Destroy가 아니라 DestroyImmediate로 삭제해야함. 이 경우에 Undo 기능이 없어서 되돌리기를 못함(control + Z) 그래서 Undo.DestroyObjectImmediate를 사용.
        for (int i = 0; i < combineObjects.Length; i++)
        {
            if (combineObjects[i] != null)
            {
                Undo.DestroyObjectImmediate(combineObjects[i]);
            }
        }
    }
}
