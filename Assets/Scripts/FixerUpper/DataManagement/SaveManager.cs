using UnityEngine;

public static class SaveManager
{
	public static void Save<T>(T data, string key)
	{
		string json = JsonUtility.ToJson(data, true);
		PlayerPrefs.SetString(key, json);
		
		Debug.Log($"Data '<b><color=green>{key}</color></b>' succesfully <b>saved</b>!\n{json}");
	}
	
	public static T Load<T>(string key)
	{
		var data = default(T);
		
		if(PlayerPrefs.HasKey(key))
		{
			string json = PlayerPrefs.GetString(key);
			data = JsonUtility.FromJson<T>(json);
			
			Debug.Log($"Data '<b><color=green>{key}</color></b>' succesfully <b>loaded</b>!\n{json}");
		}
		
		return data;
	}
}
