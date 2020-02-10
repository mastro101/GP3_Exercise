using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonGenerator))]
public class MeshGeneratorEditor : Editor
{
    PolygonGenerator script;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        script = target as PolygonGenerator;

        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }
    }
}
