using UnityEngine;
using System;

namespace ChargeMeUp
{
	[CreateAssetMenu]
	public class LevelManager : ScriptableObject
	{
		public PlayerProgress playerProgress;
		
		public string[] levelScenes;
		
		public void LoadData()
		{
			var loadedData = PlayerProgress.LoadSavedData();
			
			if(loadedData == null)
				loadedData = playerProgress;
			
			else
				playerProgress = loadedData;
		}
		
		public bool HasCompletedLevel() => Array.Exists(playerProgress.levels, level => level.completed);
	}
}