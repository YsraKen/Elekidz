using UnityEngine;
using System;

namespace ChargeMeUp
{
	[Serializable]
	public class PlayerProgress
	{
		public LevelData[] levels;
		
		#region SAVE/LOAD
		private const string SAVE_KEY = "PlayerProgress";
		
		public void SaveData()
		{
			string json = JsonUtility.ToJson(this, true);
			PlayerPrefs.SetString(SAVE_KEY, json);
			
			Debug.Log($"Data '<b><color=green>{SAVE_KEY}</color></b>' succesfully <b>saved</b>!\n{json}");
		}
		
		public static PlayerProgress LoadSavedData()
		{
			var data = default(PlayerProgress);
			
			if(PlayerPrefs.HasKey(SAVE_KEY))
			{
				string json = PlayerPrefs.GetString(SAVE_KEY);
				data = JsonUtility.FromJson<PlayerProgress>(json);
				
				Debug.Log($"Data '<b><color=green>{SAVE_KEY}</color></b>' succesfully <b>loaded</b>!\n{json}");
			}
			
			return data;
		}
		#endregion
	}

	[Serializable]
	public class LevelData
	{
		public bool completed;
		public float time;
	}
}