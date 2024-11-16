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

        var material = GetMaterial(meshFilterList);

        if(material == null)
        {
            Debug.Log("다른 머트리얼이 발견되어 중단합니다.");
            return;
        }

        CreateObject(meshFilterList, material);

        if (_isDestroy)
        {
            DestroyObject(_combineObjectArray);
        }
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

    private void CreateObject(List<MeshFilter> meshFilterList, Material material)
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

        var combineObject = new GameObject(_name);

        if (_isStatic)
        {
            combineObject.isStatic = true;  
        }

        var newMesh = new Mesh();
        newMesh.indexFormat = totalVertexCount > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
        newMesh.CombineMeshes(combineInstanceList.ToArray(), true, true);

        var newMeshFilter = combineObject.AddComponent<MeshFilter>();
        newMeshFilter.sharedMesh = newMesh;

        var newMeshRenderer = combineObject.AddComponent<MeshRenderer>();
        newMeshRenderer.sharedMaterial = material;
    }

    private Material GetMaterial(List<MeshFilter> meshFilterList)
    {
        Material material = null;

        foreach(var meshFilter in meshFilterList)
        {
            if(meshFilter.TryGetComponent(out MeshRenderer meshRenderer))
            {
                if(material == null)
                {
                    material = meshRenderer.sharedMaterial;
                }
                else if(material != meshRenderer.sharedMaterial)
                {
                    Debug.Log("다른 머트리얼이 발견되었습니다.");
                    return null;
                }
            }
        }

        return material;    
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
            Undo.DestroyObjectImmediate(combineObjects[i]);
        }
    }
}
