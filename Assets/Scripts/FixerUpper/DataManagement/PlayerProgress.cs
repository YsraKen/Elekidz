using UnityEngine;
using System;
using System.Collections.Generic;

namespace FixerUpper
{
	[Serializable]
	public class PlayerProgress
	{
		public LevelData[] levels;
		public int currentLevelIndex;
		
		public const string SAVE_KEY = "PlayerProgress";
		
		public void SaveData() => SaveManager.Save(this, SAVE_KEY);
		
		public PlayerProgress(PlayerProgress copy)
		{
			levels = new LevelData[copy.levels.Length];
			
			for(int i = 0; i < copy.levels.Length; i++)
				levels[i] = new LevelData();
			
			currentLevelIndex = copy.currentLevelIndex;
		}
	}

	[Serializable]
	public class LevelData
	{
		public bool completed;
		// public float time = float.MaxValue;
		
		public List<HighScoreData> highScores = new List<HighScoreData>();
	}

	[System.Serializable]
	public class HighScoreData
	{
		public string name;
		public float value = float.MaxValue;
	}
}