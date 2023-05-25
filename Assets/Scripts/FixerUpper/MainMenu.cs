using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace FixerUpper
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private Button[] _levelButtons;
		[SerializeField] private Text _highschoreTxt;
		
		[Space]
		[SerializeField] private string _gameplayScene = "SampleScene";
		[SerializeField] private string _mainMenuScene = "Main Menu";
		
		private void Start()
		{
			Array.ForEach(_levelButtons, button => button.interactable = false);
			
			// load progress
			_levelManager.LoadData();
			
			LoadLevelSelections();
			LoadHighScores();
		}
		
		private void LoadLevelSelections()
		{
			var levels = _levelManager.PlayerProgress.levels;
			
			// activate completed levels
			for(int i = 0; i < levels.Length; i++)
			{
				_levelButtons[i].interactable = true;
				
				if(!levels[i].completed)
					break;
			}
		}
		
		private void LoadHighScores()
		{
			_highschoreTxt.text = string.Empty;
			
			for(int i = 0; i < _levelManager.PlayerProgress.levels.Length; i++)
			{
				_highschoreTxt.text += $"\n<b>------------- LEVEL {i + 1} -------------</b>";
				
				var level = _levelManager.PlayerProgress.levels[i];
				
				foreach(var data in level.highScores)
				{
					string value = TimeSpan.FromSeconds(data.value).ToString(@"mm\:ss\:ff");
					string name = string.IsNullOrEmpty(data.name)? "(no name)": data.name;
					
					_highschoreTxt.text += $"\n<b>{name}:</b> {value}\n";
				}
				
				_highschoreTxt.text += "\n";
			}
		}
		
		public void OnStartButton()
		{
			var playerProgress = _levelManager.PlayerProgress;
			
			int index = playerProgress.currentLevelIndex;
				index = index % playerProgress.levels.Length;
			
			LoadLevel(index);
		}
		
		public void LoadLevel(int index)
		{
			_levelManager.PlayerProgress.currentLevelIndex = index;
			SceneManager.LoadScene(_gameplayScene);
		}
		
		public void Exit() => SceneManager.LoadScene(_mainMenuScene);
	}
}