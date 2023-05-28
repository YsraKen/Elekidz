using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace FixerUpper
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private Appliance _appliance;
		[SerializeField] private GameObject _overlay;
		
		[SerializeField] private bool _showHintOnStart, _autoShowNextStepHint;
		
		[field: SerializeField] public GameObject HintCloseButton { get; private set; }
		
		[field: SerializeField] public Transform DefaultDragParent { get; private set; }
		
		[Space]
		[SerializeField] private GameObject _mainPanel;
		[SerializeField] private GameObject _toolsPanel;
		
		[Space]
		[SerializeField] private GameObject _instructionPanel;
		[SerializeField] private Text _instructionText;

		[Space]
		[SerializeField] private GameObject _timerUI;
		[SerializeField] private Text _timerTxt;
		[SerializeField] private string _timerFormat = @"hh\:mm\:ss";
		
		public float Timer { get; private set; }
		private Coroutine _timerRoutine;
		
		[Space]
		[SerializeField] private ParticleSystem[] _particles;
		
		[Space]
		[SerializeField] private GameObject _stepFinishPrompt;
		[SerializeField] private float _stepFinishPromptDuration = 2f;
		[SerializeField] private Text _stepFinishTxt;
		
		[Space]
		[SerializeField] private GameObject _pausePanel;
		[SerializeField] private GameObject _gameOverUI;
		[SerializeField] private Text _timerFinalTxt;
		
		[SerializeField] private GameObject[] _winObjects, _failObjects;
		
		[Space]
		[SerializeField] private GameObject _highscoreField;
		[SerializeField] private Text _highscoreFieldTxt;
		
		[Space]
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private LevelGameObject[] _levelGameObjects;
		[SerializeField] private string _homeScene = "FixerUpper";
		
		private bool _newHighScore;
		public string highscoreName { get; set; }
		
		private List<HighScoreData> _highscores;
		
		private Coroutine _onStepFinishedRoutine, _gameOverRoutine;
		
		#region Singleton
		public static GameManager Instance { get; private set; }
		private void Awake() => Instance = this;
		#endregion
		
		#region Start
		
		private IEnumerator Start()
		{
			foreach(var objects in _levelGameObjects)
				objects.SetActive(false);
			
			_overlay.SetActive(true);
			
			// Toggle levels
			int levelIndex = _levelManager.PlayerProgress.currentLevelIndex;
				_appliance = _levelGameObjects[levelIndex % _levelGameObjects.Length].appliance;
			
			for(int i = 0; i < _levelGameObjects.Length; i++)
			{
				bool isCurrentLevel = i == levelIndex;
				_levelGameObjects[i].SetActive(isCurrentLevel);
				
				if(!isCurrentLevel)
					_levelGameObjects[i].Destroy();
			}
			
			yield return null;
			
			// Hide all panels
			_mainPanel.SetActive(false);
			_toolsPanel.SetActive(false);
			
			// Play intro
			yield return new WaitForSeconds(1f);
			_mainPanel.SetActive(true);
			
			yield return new WaitForSeconds(1.5f);
			_toolsPanel.SetActive(true);
			
			// Start Button
			yield return WaitForStartButton();
			
			// Actual Gameplay
			yield return null;
			StartTimer();
			_overlay.SetActive(false);
			
			// Startup Hints
			if(_showHintOnStart)
			{
				yield return new WaitForSeconds(2f);
				ShowHint();
			}
		}
		
		private IEnumerator WaitForStartButton()
		{
			_instructionPanel.SetActive(true);
			_instructionText.text = _appliance.Description;
			
			yield return new WaitWhile(() => _instructionPanel.activeSelf);
		}
		
		#endregion
		
		private void Update()
		{
			if(Input.GetButtonDown("Cancel"))
			{
				if(Hint.CurrentHint)
					HideHint();
				
				else
					SetPauseState(!_pausePanel.activeSelf);
			}
		}
		
		#region Timer
		
		private void StartTimer()
		{
			_timerRoutine = StartCoroutine(r());
			
			IEnumerator r()
			{
				_timerUI.SetActive(true);
				
				while(true)
				{
					Timer += Time.deltaTime;
					_timerTxt.text = GetFormattedTimer(Timer);
					
					yield return null;
				}
			}
		}
		
		private void StopTimer()
		{
			StopCoroutine(_timerRoutine);
			
			_timerUI.SetActive(false);
			_timerTxt.text = GetFormattedTimer(Timer);
		}
		
		private string GetFormattedTimer(float value) => TimeSpan.FromSeconds(value).ToString(_timerFormat);
		
		#endregion
		
		#region Hint
		
		public void ShowHint()
		{
			var currentLevelData = _levelManager.GetCurrentLevelData();
			
			if(currentLevelData != null && currentLevelData.completed)
				return;
			
			var steps = _appliance.GetSteps();
			
			var step = Array.Find(steps, step => !step.IsDone);
				step?.ShowHint();
		}
		
		public void HideHint()
		{
			if(Hint.CurrentHint)
				Hint.CurrentHint.SetActive(false);
			
			HintCloseButton.SetActive(false);
		}
		
		#endregion
		
		#region Gameplay Updates
		
		public void PlayParticle(Vector3 position = default, string name = null)
		{
			var particle = default(ParticleSystem);
			
			if(name != null)
				particle = Array.Find(_particles, particle => particle.name == name);
			else
				particle = _particles[Random.Range(0, _particles.Length)];
			
			particle.transform.position = position;
			particle.Play(true);
		}
		
		public void OnStepFinished(Step step)
		{
			string description = step.Description;
			
			if(string.IsNullOrEmpty(description))
				return;
			
			RestartCoroutine(r(), ref _onStepFinishedRoutine);
			
			IEnumerator r()
			{
				string particleName = Random.value < 0.5f? "StarExplosion": "StarPoof";
				PlayParticle(name: particleName);
				yield return null;
				
				_stepFinishPrompt.SetActive(true);
				_stepFinishTxt.text = description;
				
				yield return new WaitForSeconds(_stepFinishPromptDuration);
				_stepFinishPrompt.SetActive(false);
				
				if(_autoShowNextStepHint)
				{
					yield return new WaitForSeconds(1f);
					ShowHint();
				}
			}
		}
		
		public void SetPauseState(bool isPaused)
		{
			_pausePanel.SetActive(isPaused);
			Time.timeScale = isPaused? 0f: 1f;
		}
		
		public void GameOver(bool win = false)
		{
			RestartCoroutine(r(), ref _gameOverRoutine);
			IEnumerator r()
			{
				StopTimer();
				_timerFinalTxt.text = GetFormattedTimer(Timer);
				
				yield return new WaitForSeconds(3f);
				
				_gameOverUI.SetActive(true);
				
				Toggle(_winObjects, win);
				Toggle(_failObjects, !win);
				
				HandleHighscoring();
				SaveProgressData();
			}
		}
		
		private void SaveProgressData()
		{
			var data = _levelManager.GetCurrentLevelData();
			
			data.completed = true;
			// data.time = Timer;
			
			var playerProgress = _levelManager.PlayerProgress;
				playerProgress.currentLevelIndex ++;
				playerProgress.SaveData();
		}
		
		#endregion
		
		#region HighScores
		
		private void HandleHighscoring()
		{
			var data = _levelManager.GetCurrentLevelData();
			
			_highscores = data.highScores;
			
			if(_highscores == null)
			{
				_highscores = new List<HighScoreData>();
				_newHighScore = true;
				
				return;
			}
			
			if(_highscores.Count <= 0)
				_newHighScore = true;
			
			else
			{
				var highestRecord = _highscores[0];
				
				if(Timer < highestRecord.value)
					_newHighScore = true;
			}
		}
		
		private void RecordNewHighScoreData(Action onDone)
		{
			StartCoroutine(r());
			IEnumerator r()
			{
				_highscoreField.SetActive(true);
				_highscoreFieldTxt.text = $"<b>{GetFormattedTimer(Timer)}</b>";
				
				if(_highscores.Count > 0)
				{
					string oldHighscore = $"(old record: '{GetFormattedTimer(_highscores[0].value)}')";
					_highscoreFieldTxt.text += "\n" + oldHighscore;
				}
				
				yield return new WaitWhile(()=> _highscoreField.activeInHierarchy);
				
				onDone();
			}
		}
		
		public void SubmitHighscoreData()
		{
			var newData = new HighScoreData()
			{
				name = highscoreName,
				value = Timer
			};
			
			_highscores.Insert(0, newData);
			_levelManager.PlayerProgress.SaveData();
			
			_highscoreField.SetActive(false);
		}
		
		#endregion
		
		#region Utils
		
		private void RestartCoroutine(IEnumerator iEnumerator, ref Coroutine coroutine)
		{
			if(coroutine != null)
				StopCoroutine(coroutine);
			
			coroutine = StartCoroutine(iEnumerator);
		}
		
		private void Toggle(GameObject[] objects, bool isActive)
		{
			foreach(var obj in objects)
				obj.SetActive(isActive);
		}
		
		#endregion
		
		#region Scene
		
		public void NextLevel()
		{
			var playerProgress = _levelManager.PlayerProgress;
			int levelIndex = playerProgress.currentLevelIndex;
			
			var scene = levelIndex < playerProgress.levels.Length? gameplayScene(): "MainMenu";
			
			if(_newHighScore)
				RecordNewHighScoreData(onDone: ()=> SceneManager.LoadScene(scene));
			
			else
				SceneManager.LoadScene(scene);
			
			string gameplayScene() => SceneManager.GetActiveScene().name;
		}
		
		// public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
		public void Exit(bool checkForHighScore = true)
		{
			SetPauseState(false);
			
			if(!checkForHighScore)
			{
				SceneManager.LoadScene(_homeScene);
				return;
			}
			
			int levelIndex = _levelManager.PlayerProgress.currentLevelIndex;
			
			if(_newHighScore)
				RecordNewHighScoreData(onDone: ()=> SceneManager.LoadScene(_homeScene));
			
			else
				SceneManager.LoadScene(_homeScene);
		}
		
		#endregion
		
		private void OnValidate()
		{
			foreach(var lvlObj in _levelGameObjects)
				lvlObj.OnValidate();
		}
		
		[System.Serializable]
		public class LevelGameObject
		{
			public string name;
			public Appliance appliance;
			public GameObject[] gameObjects;
			
			public void SetActive(bool isActive)
			{
				appliance.gameObject.SetActive(isActive);
				
				foreach(var gameObject in gameObjects)
					gameObject.SetActive(isActive);
			}
			
			public void Destroy()
			{
				Object.Destroy(appliance.gameObject);
				
				for(int i = 0; i < gameObjects.Length; i++)
					Object.Destroy(gameObjects[i]);
			}
			
			public void OnValidate()
			{
				if(!appliance)
					return;
				
				if(string.IsNullOrEmpty(name))
					name = appliance.name;
			}
		}
	}
}