using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombineMesh))]
public class CombineMeshEditor : Editor
{
    private CombineMesh _combineMesh;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //�⺻ �ν����� Draw

        EditorGUILayout.BeginHorizontal(); //���� ���̾ƿ� ����

        _combineMesh = (CombineMesh)target;

        if (GUILayout.Button("ComBine"))
        {
            _combineMesh.MeshCombine();
        }
        else if (GUILayout.Button("Clear"))
        {
            _combineMesh.ResetCombine();
        }

        EditorGUILayout.EndHorizontal();
    }
}
