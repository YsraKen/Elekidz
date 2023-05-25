using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ChargeMeUp
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private float _startDelay = 2f;
		[SerializeField] private GameObject _inputsOverlay;
		
		[Space]
		[SerializeField] private GameObject _timerObj;
		[SerializeField] private TMP_Text _currentTimeText;
		
		public bool isTimerActive { get; set; }
		private float _currentTime;
		
		[SerializeField] private ItemInfo[] _itemInfos;
		[SerializeField] private Transform _itemSpawnPoint;
		
		[SerializeField] private Slot[] _slots;
		
		public List<Item> ItemInstances { get; private set; } = new List<Item>();
		
		[field: SerializeField] public Transform DefaultDragParent { get; private set; }
		[SerializeField] private Button _resetButton, _deleteButton, _rotateButton;
		[SerializeField] private TMP_Text _deleteButtonTxt;
		
		[Space]
		[SerializeField] private GameObject _messagePanel;
		[SerializeField] private TMP_Text _messageTxt;
		[SerializeField] private string _homeScene = "MainMenu";
		
		public enum MessageType { Normal, Warning, Error }
		private const int NUM_OF_MSG_TYPE = 3;
		
		[SerializeField] private GameObject[] _messageTypeObjects = new GameObject[NUM_OF_MSG_TYPE];
		
		[Space]
		[SerializeField] private AudioClip _wrongSound;
		private AudioSource _audioSource;
		
		[Space]
		[SerializeField] private GameObject _pauseMenu;
		[SerializeField] private GameObject _gameOver;
		[SerializeField] private TMP_Text _totalTimeTxt;
		[SerializeField] private LevelManager _levelManager;
		
		public static int selectedLevelIndex;
		
		public static GameManager Instance { get; private set; }
		
		private void OnValidate()
		{
			foreach(var info in _itemInfos)
				info.InitializeUI();
		}
		
		private void Awake()
		{
			Instance = this;
			_audioSource = GetComponent<AudioSource>();
			
			_levelManager.LoadData();
		}
		
		private IEnumerator Start()
		{
			_resetButton.interactable = false;
			_deleteButton.interactable = false;
			_rotateButton.interactable = false;
			
			foreach(var info in _itemInfos)
				info.InitializeUI();
			
			// Start Delay
			_inputsOverlay.SetActive(true);
			_timerObj.SetActive(false);
			{			
				yield return new WaitForSeconds(_startDelay);
				isTimerActive = true;			
			}
			_timerObj.SetActive(true);
			_inputsOverlay.SetActive(false);
		}
		
		private void Update()
		{
			if(isTimerActive)
				UpdateTimer();
		}
		
		private void UpdateTimer()
		{
			_currentTime += Time.deltaTime;
			
			var timeSpan = TimeSpan.FromSeconds(_currentTime);
			_currentTimeText.text = timeSpan.ToString(@"mm\:ss\:ff");
		}
		
		public void SpawnItem(int index)
		{
			if(_itemSpawnPoint.childCount > 0)
			{
				_audioSource.PlayOneShot(_wrongSound);
				return;
			}
			
			var item = _itemInfos[index].InstantiatePrefab(_itemSpawnPoint);
			
			if(item)
			{
				ItemInstances.Add(item);
				Selection.Instance.Select(item);
				
				OnItemInstancesModified();
			}
		}
		
		public void RotateSelectedItems()
		{
			var selectedObjects = Selection.Instance.Objects;
			
			foreach(var obj in selectedObjects)
			{
				var item = obj as Item;
					item?.Rotate();
			}
			
			// CircuitManager.Instance.UpdateTicks();
		}
		
		public void OnSelectionsUpdate()
		{
			int numberOfSelections = Selection.Instance.Objects.Count;
			
			bool interactable = numberOfSelections > 0;
				_deleteButton.interactable = interactable;
				_rotateButton.interactable = interactable;
			
			_deleteButtonTxt.text = numberOfSelections > 1? "Delete Items": "Delete Item";
		}
		
		public void DeleteItems()
		{
			var selection = Selection.Instance;
			var selectedObjects = new List<ISelectable>(selection.Objects);
			
			for(int i = 0; i < selectedObjects.Count; i++)
			{
				var item = selectedObjects[i] as Item;
				if(!item) continue;
				
				selection.Deselect(item);
				ItemInstances.Remove(item);
				
				Destroy(item.gameObject);
			}
			
			OnItemInstancesModified();
		}
		
		public void ResetBoard()
		{
			Selection.Instance.DeselectAll();
			
			for(int i = 0; i < ItemInstances.Count; i++)
				Destroy(ItemInstances[i].gameObject);
			
			ItemInstances.Clear();
			OnItemInstancesModified();
		}
		
		private void OnItemInstancesModified()
		{
			StartCoroutine(r());
			IEnumerator r()
			{
				yield return null;
				
				// Check for infos
				foreach(var info in _itemInfos)
					info.RecheckInstances();
				
				_resetButton.interactable = ItemInstances.Count > 0;
				
				// CircuitManager.Instance.UpdateTicks();
			}
		}
		
		public void OnItemMoved(Item item)
		{
			// toggle interactions to "_resetButton"
			_resetButton.interactable = Array.Find(_slots, slot => slot.IsOccupied());
		}
		
		public void ShowMessage(string message, MessageType type = default)
		{
			_messageTxt.text = message;
			
			for(int i = 0; i < NUM_OF_MSG_TYPE; i++)
				_messageTypeObjects[i]?.SetActive(i == (int) type);
			
			_messagePanel.SetActive(true);
		}
		
		public void SetPauseState(bool isPaused)
		{
			_pauseMenu.SetActive(isPaused);
			Time.timeScale = isPaused? 0f: 1f;
		}
		
		[ContextMenu("Game Over")]
		public void GameOver()
		{
			isTimerActive = false;
			_gameOver.SetActive(true);
			
			var timeSpan = TimeSpan.FromSeconds(_currentTime);
			_totalTimeTxt.text = timeSpan.ToString(@"mm\:ss\:ff");
			
			var levelData = _levelManager.playerProgress.levels[selectedLevelIndex];
				levelData.completed = true;
				levelData.time = _currentTime;
			
			// Debug.Break();
			_levelManager.playerProgress?.SaveData();
		}
		
		public void MainMenu() => SceneManager.LoadScene(_homeScene);
		
		public void NextLevel()
		{
			selectedLevelIndex ++;
			
			if(selectedLevelIndex < _levelManager.levelScenes.Length)
				SceneManager.LoadScene(_levelManager.levelScenes[selectedLevelIndex]);
			
			else
				MainMenu();
		}
	}
}