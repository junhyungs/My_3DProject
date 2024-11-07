using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombineMesh))]
public class CombineMeshEditor : Editor
{
    private CombineMesh _combineMesh;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _combineMesh = (CombineMesh)target;

        if (GUILayout.Button("Button"))
        {
            Debug.Log("버튼 눌림");
        }
    }
}
