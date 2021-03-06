using UnityEngine;
using UnityEditor;
using libmapgen;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        MapGenerator generator = (MapGenerator) target;

        generator.InitRuleset();

        generator.SetSize(
            EditorGUILayout.IntField("Width", generator.GetWidth()),
            EditorGUILayout.IntField("Height", generator.GetHeight())
        );

        if (GUILayout.Button("Reset ruleset")) {
            generator.ResetRuleset();
        }

        EditorGUILayout.Space();

        MapContext context = generator.GetMapContext();
        int len = generator.GetPassCount();
        for (int i = 0; i < len; i++) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(generator.GetPassName(i));
            generator.GetPass(i).Draw(context);
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();

        int? pass = PassesManager.SelectPass();
        if (pass.HasValue) {
            generator.AddPass(PassesManager.CreatePass(pass.Value), PassesManager.GetPassName(pass.Value));
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate statically")) {
            generator.RegenerateMap();
        }
        if (GUILayout.Button("Remove static map")) {
            Mesh mesh = generator.DeleteMap();
            if (mesh != null) {
                DestroyImmediate(mesh);
            }
        }
    }

}
