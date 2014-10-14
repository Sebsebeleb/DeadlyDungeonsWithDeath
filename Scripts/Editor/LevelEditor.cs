using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();

        Level levelScript = (Level)target;

		if (GUILayout.Button("Generate Level")){
			levelScript.DeleteLevel();
			levelScript.MakeLevel();
		}

		if (GUILayout.Button("Delete Level")) {
			levelScript.DeleteLevel();
		}
    }
}