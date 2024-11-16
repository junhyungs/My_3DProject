using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshCombiner : MonoBehaviour //병합할 메쉬에 대한 정보를 복사하고 이 정보를 바탕으로 새로운 메쉬를 생성함. 
{                                         //병합할 메쉬(objectsToMerge List)로 추가했던 메쉬들은 새로운 메쉬 생성후에 비활성화하거나 삭제하는 코드.
    public List<GameObject> objectsToMerge = new List<GameObject>(); // 병합할 오브젝트 리스트
    public bool addMeshCollider = false; // 병합된 메쉬에 메쉬 콜라이더를 추가할지 결정하는 변수
    public bool deactivateObjectsAfterMerge = true; // 병합 후 병합된 오브젝트들을 비활성화할지 결정하는 변수
    public bool destroyObjectsAfterMerge = false; // 병합 후 병합된 오브젝트들을 삭제할지 결정하는 변수
    public bool makeStatic = true; // 병합된 오브젝트를 정적 오브젝트로 만들지 결정하는 변수

    public bool isCombined = false; // 현재 병합 상태를 나타내는 변수

    // 병합 또는 클리어 동작을 수행하는 메서드
    public void MergeOrClearMeshes()
    {
        InitializeObjects(); // 초기화 메서드 호출

        // 병합할 오브젝트가 1개 이하인 경우 경고 메시지 출력 및 상태 업데이트
        if (objectsToMerge.Count <= 1)
        {
            Debug.LogWarning(objectsToMerge.Count == 0 ? "병합할 오브젝트가 없습니다." : "1개의 오브젝트는 병합할 수 없습니다.");
            UpdateStateAndButtonLabel(false);
            return;
        }

        MergeMeshes();
        ClearObjects(); // 병합 후 오브젝트 리스트를 클리어
        UpdateStateAndButtonLabel(true);
        Debug.Log("오브젝트가 성공적으로 병합되었습니다.");
    }

    // 오브젝트 리스트를 초기화하는 메서드
    public void InitializeObjects()
    {
        isCombined = false;
    }

    // 병합할 오브젝트 리스트를 비우는 메서드
    public void ClearObjects()
    {
        objectsToMerge.Clear();
        isCombined = false;
        UpdateStateAndButtonLabel(false);
    }

    // 병합 상태와 버튼 레이블을 업데이트하는 메서드
    private void UpdateStateAndButtonLabel(bool combinedState)
    {
        isCombined = combinedState;
        UpdateButtonLabel?.Invoke(isCombined);
        isCombined = false;
    }

    // 버튼 레이블 업데이트를 위한 델리게이트 및 이벤트 정의
    public delegate void ButtonLabelUpdate(bool isCombined);
    public static event ButtonLabelUpdate UpdateButtonLabel;

    // 메쉬를 병합하는 메서드
    private void MergeMeshes()
    {
        var meshFilters = new List<MeshFilter>();

        // 각 오브젝트에서 MeshFilter를 가져옴
        foreach (var obj in objectsToMerge)
        {
            meshFilters.AddRange(obj.GetComponentsInChildren<MeshFilter>());
        }

        // MeshFilter가 없으면 경고 메시지 출력 및 종료
        if (meshFilters.Count == 0)
        {
            Debug.LogWarning("병합할 오브젝트가 없습니다.");
            return;
        }

        // Material을 기준으로 메쉬를 그룹화
        var materialMeshMap = CreateMaterialMeshMap(meshFilters);

        // 병합된 오브젝트 생성 및 설정
        var combinedObject = CreateCombinedObject(materialMeshMap);

        // 필요한 경우 메쉬 콜라이더 추가
        if (addMeshCollider)
        {
            AddMeshCollider(combinedObject);
        }

        // 필요한 경우 병합된 오브젝트 비활성화
        if (deactivateObjectsAfterMerge)
        {
            DeactivateObjects(objectsToMerge);
        }

        // 필요한 경우 병합된 오브젝트 삭제
        if (destroyObjectsAfterMerge)
        {
            DestroyObjects(objectsToMerge);
        }
    }

    // MeshFilter 리스트에서 Material을 기준으로 메쉬를 그룹화하는 메서드/ 새로운 메쉬생성X 공통된 정보를 묶음.
    private Dictionary<Material, List<CombineInstance>> CreateMaterialMeshMap(List<MeshFilter> meshFilters)
    {
        var materialMeshMap = new Dictionary<Material, List<CombineInstance>>();

        foreach (var meshFilter in meshFilters)
        {
            var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer == null || meshFilter.sharedMesh == null)
                continue;

            var sharedMaterials = meshRenderer.sharedMaterials; //현재 이 메쉬에 적용되어 있는 모든 머트리얼을 가져옴
            for (int subMeshIndex = 0; subMeshIndex < meshFilter.sharedMesh.subMeshCount; subMeshIndex++) //머트리얼의 개수 만큼 반복
            {
                if (subMeshIndex >= sharedMaterials.Length) //머트리얼의 정보가 없다면 건너뜀
                    continue;

                var material = sharedMaterials[subMeshIndex]; //현재 머트리얼을 가져옴(인덱스가 0이라면 사용중인 모든 머트리얼 중 첫 번째 머트리얼)
                if (material == null)
                    continue;

                if (!materialMeshMap.ContainsKey(material)) //현재 딕셔너리에 이 머트리얼에 대한 key가 없다면 List 생성
                {
                    materialMeshMap[material] = new List<CombineInstance>();
                }

                materialMeshMap[material].Add(new CombineInstance //리스트안에 병합할 메쉬의 정보를 담아줌
                {
                    mesh = GetSubMesh(meshFilter.sharedMesh, subMeshIndex), //sharedMesh는 원본데이터임. 원본데이터는 모든 오브젝트가 공유함으로 수정하지 않는것이 좋음. 따라서 지금 이 코드는 원본 데이터를 수정하지 않고 현재 작업해야하는 메쉬의 정보를 복사하는거임.
                    transform = meshFilter.transform.localToWorldMatrix //현재 메쉬의 위치, 회전, 크기 정보
                });
            }
        }

        return materialMeshMap;
    }

    // 서브 메쉬를 추출하는 메서드
    private Mesh GetSubMesh(Mesh mesh, int index)
    {
        var subMesh = new Mesh //새로운 메쉬를 생성하여 현재 메쉬에 대한 정보를 복사한다.
        {
            vertices = mesh.vertices, 
            normals = mesh.normals,
            tangents = mesh.tangents,
            uv = mesh.uv,
            uv2 = mesh.uv2,
            uv3 = mesh.uv3,
            uv4 = mesh.uv4,
            colors = mesh.colors
        };
        subMesh.SetTriangles(mesh.GetTriangles(index), 0); //새로 생성한 메쉬에 현재 메쉬의 삼각형 정보를 복사하여 넣는다. 
        return subMesh; //mesh.GetTriangles(index)는 현재 메쉬의 삼각형 정보. 따라서 현재 메쉬의 삼각형(폴리곤)정보를 복사하여 새로운 메쉬에게 할당해주는 부분.
    }

    // 병합된 오브젝트를 생성하고 설정하는 메서드
    private GameObject CreateCombinedObject(Dictionary<Material, List<CombineInstance>> materialMeshMap)
    {
        var combinedObject = new GameObject("Combined Mesh");

        // 사용자가 정적 오브젝트로 만들지 선택할 수 있게 설정
        if (makeStatic) combinedObject.isStatic = true;

        foreach (var kvp in materialMeshMap)
        {
            var material = kvp.Key;
            var combineInstances = kvp.Value;

            var combinedMesh = new Mesh();

            // 버텍스 개수에 따라 인덱스 포맷 설정
            int totalVertexCount = combineInstances.Sum(c => c.mesh.vertexCount); //모든 버텍스 개수를 합산한 값을 반환.
            combinedMesh.indexFormat = totalVertexCount > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16; //1개의 서브 메쉬는 최대 65535개의 정점을 지원한다. 이 정점 수를 초과했다면 포멧을 UInt32로 변경하여 초과되더라도 처리할 수 있게함.

            combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true); //병합하는 부분. 아까 복사할 정보를 담았던 리스트를 배열화, 2번째 매개변수는 모든 서브메쉬를 병합할지 여부. 3번째 매개변수는 변환행렬 정보를 포함하여 병합할건지, 아니면 변환 행렬을 무시하고 로컬 좌표로 병합할지. 

            var child = new GameObject(material.name);
            child.transform.SetParent(combinedObject.transform, false);
            if(makeStatic) child.isStatic = true;

            var meshFilter = child.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = combinedMesh;

            var meshRenderer = child.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
        }

        return combinedObject;
    }

    // 병합된 오브젝트에 메쉬 콜라이더를 추가하는 메서드
    private void AddMeshCollider(GameObject combinedObject)
    {
        var meshColliders = combinedObject.GetComponentsInChildren<MeshFilter>()
            .Select(mf => mf.gameObject.AddComponent<MeshCollider>());

        foreach (var meshCollider in meshColliders)
        {
            meshCollider.sharedMesh = meshCollider.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    // 오브젝트 리스트의 오브젝트들을 비활성화하는 메서드
    private void DeactivateObjects(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    // 오브젝트 리스트의 오브젝트들을 삭제하는 메서드
    private void DestroyObjects(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
}
