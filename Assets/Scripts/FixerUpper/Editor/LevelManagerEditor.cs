using UnityEngine;
using UnityEditor;
using System;

namespace FixerUpper
{
	[CustomEditor(typeof(LevelManager))]
	public class LevelManagerEditor : Editor
	{
		private LevelManager _script;
		
		private void OnEnable() => _script = (LevelManager) target;
		
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			GUILayout.BeginHorizontal();
			{
				DrawButton("Load Data", _script.LoadData);
				DrawButton("Save Data", _script.SaveData);
			}
			GUILayout.EndHorizontal();
			
			DrawButton("Clear Progress Data", _script.ResetProgressData);
		}
		
		private void DrawButton(string label, Action action)
		{
			if(GUILayout.Button(label))
				action();
		}
	}
}