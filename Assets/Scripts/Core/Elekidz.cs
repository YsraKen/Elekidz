using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class Elekidz : ScriptableObject
{
	#if UNITY_EDITOR
	
	#region Singleton
	private static Elekidz _instance;
	
	public static Elekidz Instance
	{
		get
		{
			if(!_instance)
			{
				var instances = Resources.LoadAll<Elekidz>("");
				
				if(instances != null || instances.Length < 0)
					_instance = instances[0];
			}
		
			return _instance;
		}
	}
	#endregion
	
	#region References
	
	[Header(SCENE)]
	public Object mainMenuScene;
	public Object runnerMenuScene, fixerMenuScene, chargeMenuScene;
	public Object runnerGameScene, fixerGameScene, chargeGameScene;
	
	private const string
		BASE = "ELEKIDZ",
		SCENE = "Scenes",
		MENU = "Homescreens",
		GAME = "Gameplays";
	
	#endregion
	
	#region Scenes
	
	[MenuItem(BASE + "/" + SCENE + "/Main Menu")]
	public static void scene1() => Ping(Instance.mainMenuScene);
	
		#region Menu
		
		[MenuItem(BASE + "/" + SCENE + "/" + MENU + "/Current Run")]
		public static void sceneMenu1() => Ping(Instance.runnerMenuScene);
		
		[MenuItem(BASE + "/" + SCENE + "/" + MENU + "/Fixer Upper")]
		public static void sceneMenu2() => Ping(Instance.fixerMenuScene);
		
		[MenuItem(BASE + "/" + SCENE + "/" + MENU + "/Charge Me Up")]
		public static void sceneMenu3() => Ping(Instance.chargeMenuScene);
		
		#endregion
	
		#region Games
		
		[MenuItem(BASE + "/" + SCENE + "/" + GAME + "/Current Run")]
		public static void sceneGame1() => Ping(Instance.runnerGameScene);
		
		[MenuItem(BASE + "/" + SCENE + "/" + GAME + "/Fixer Upper")]
		public static void sceneGame2() => Ping(Instance.fixerGameScene);
		
		[MenuItem(BASE + "/" + SCENE + "/" + GAME + "/Charge Me Up")]
		public static void sceneGame3() => Ping(Instance.chargeGameScene);
		
		#endregion
		
	#endregion
	
	public static void Ping(Object obj)
	{
		if(!obj)
		{
			EditorUtility.DisplayDialog(BASE, "Object not found", "OK");
			
			if(EditorUtility.DisplayDialog(BASE, "Do you want to assign a reference for this object?", "Assign Reference", "Cancel"))
			{
				var instance = Instance;
				
				if(instance)
					PingDirect(instance);
				
				else
					EditorUtility.DisplayDialog(BASE, $"There is no {BASE} scriptable object in the Resources folder", "OK");
			}
			
			return;
		}
	
		string file = Path.GetFileName(AssetDatabase.GetAssetPath(obj));
		
		int dialogue = EditorUtility.DisplayDialogComplex
		(
			BASE,
			"You're currently accessing '" + file +"'. \n\nWhat do you want to do?",
			"Open",
			"Cancel",
			"Locate"
		);
		
		switch(dialogue)
		{
			case 0: AssetDatabase.OpenAsset(obj); break;
			case 1: break;
			
			case 2:
				PingDirect(obj);
				break;
			
			default: Debug.LogError("Unknown error."); break;
		}
	}
	
	private static void PingDirect(Object obj)
	{
		Selection.objects = new Object[] { obj };
		EditorGUIUtility.PingObject(obj);
	}
	#endif
}