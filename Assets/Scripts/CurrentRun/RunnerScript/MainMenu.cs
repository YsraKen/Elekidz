using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	
	public string CurrentRun;

	
	public void Runner () => LoadScene(CurrentRun);
	
	public void LoadScene (string sceneName) => SceneManager.LoadScene(sceneName);
	
	
	public void QuitGame () 
	{
		Application.Quit();
		
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
