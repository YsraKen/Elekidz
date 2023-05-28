using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

namespace ChargeMeUp
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private GameObject _levelSelections;
		[SerializeField] private LevelButton[] _levelButtons;
		
		[SerializeField] private string _mainMenuScene = "MainMenu";
		
		private void Start()
		{
			// deactivate level selections
			_levelSelections.SetActive(false);
			Array.ForEach(_levelButtons, button => button.gameObject.SetActive(false));
			
			// load progress
			_levelManager.LoadData();
			var levels = _levelManager.playerProgress.levels;
			
			// activate completed levels
			for(int i = 0; i < levels.Length; i++)
			{
				_levelButtons[i].gameObject.SetActive(true);
				
				if(levels[i].completed)
				{
					var timeSpan = TimeSpan.FromSeconds(levels[i].time);
					_levelButtons[i].timeTxt.text = $"({timeSpan.ToString(@"mm\:ss\:ff")})";
				}
				
				else break;
			}
		}
		
		public void OnStartButton()
		{	
			if(_levelManager.HasCompletedLevel())
				_levelSelections.SetActive(true);
			
			else
				LoadLevel(0);
		}
		
		public void LoadLevel(int index)
		{
			GameManager.selectedLevelIndex = index;
			
			string scene = _levelManager.levelScenes[index];
			SceneManager.LoadScene(scene);
		}
		
		public void LoadScene(string name) => SceneManager.LoadScene(name);
		
		public void OnExitButton() => SceneManager.LoadScene(_mainMenuScene);
	}
}