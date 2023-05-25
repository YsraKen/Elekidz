using UnityEngine;
using System;

namespace FixerUpper
{
	[CreateAssetMenu]
	public class LevelManager : ScriptableObject
	{
		[SerializeField] private PlayerProgress
			defaultPlayerProgressData,
			loadedProgressData;
		
		public PlayerProgress PlayerProgress => loadedProgressData;
		
		// [field: SerializeField] public string[] LevelScenes { get; private set; }
		
		[ContextMenu("Load Saved Data")]
		public void LoadData()
		{
			loadedProgressData = SaveManager.Load<PlayerProgress>(PlayerProgress.SAVE_KEY);
			
			if(loadedProgressData == null)
				loadedProgressData = new PlayerProgress(defaultPlayerProgressData);
		}
		
		[ContextMenu("Clear Progress Data")]
		public void ResetProgressData()
		{
			PlayerPrefs.DeleteAll();
			loadedProgressData = null;
		}
		
		public LevelData GetCurrentLevelData()
		{
			int index = PlayerProgress.currentLevelIndex;
			
			return index < PlayerProgress.levels.Length? PlayerProgress.levels[index]: null;
		}
		
		#if UNITY_EDITOR
		[ContextMenu("Save Data")]
		public void SaveData() => PlayerProgress.SaveData();
		#endif
	}
}