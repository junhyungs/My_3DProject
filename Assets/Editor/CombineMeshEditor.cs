using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombineMesh))]
public class CombineMeshEditor : Editor
{
    private CombineMesh _combineMesh;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //기본 인스펙터 Draw

        EditorGUILayout.BeginHorizontal(); //수평 레이아웃 시작

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
