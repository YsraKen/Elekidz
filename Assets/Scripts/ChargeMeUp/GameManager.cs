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
		// [SerializeField] private Level[] _levels;
		[SerializeField] private Level level;
		
		[SerializeField] private float _startDelay = 2f;
		[SerializeField] private GameObject _inputsOverlay;
		
		[Space]
		[SerializeField] private GameObject _timerObj;
		[SerializeField] private TMP_Text _currentTimeText;
		
		public bool isTimerActive { get; set; }
		private float _currentTime;
		
		[Space]
		public ItemInfo[] itemInfos;
		[SerializeField] private ItemContainer[] _itemContainers;
		[SerializeField] private Transform _itemSpawnPoint;
		
		[SerializeField] private Slot[] _slots;
		
		public List<Item> ItemInstances { get; private set; } = new List<Item>();
		
		[field: SerializeField, Space] public Transform DefaultDragParent { get; private set; }
		
		[SerializeField] private Button _resetButton, _deleteButton, _rotateButton;
		[SerializeField] private TMP_Text _deleteButtonTxt;
		
		[Space]
		[SerializeField] private GameObject _messagePanel;
		[SerializeField] private TMP_Text _messageTxt;
		
		public enum MessageType { Normal, Warning, Error }
		private const int NUM_OF_MSG_TYPE = 3;
		
		[SerializeField] private GameObject[] _messageTypeObjects = new GameObject[NUM_OF_MSG_TYPE];
		
		[Space]
		[SerializeField] private TMP_Text _hintTxt;
		[SerializeField] private SpriteSheetAnimationUI _hintAnimation;
		
		private AudioClip _hintClip;
		
		[Space]
		[SerializeField] private AudioClip _wrongSound;
		private AudioSource _audioSource;
		
		[Space]
		[SerializeField] private GameObject _pauseMenu;
		[SerializeField] private GameObject _gameOver;
		[SerializeField] private TMP_Text _totalTimeTxt;
		[SerializeField] private LevelManager _levelManager;
		
		[SerializeField] private GameObject m_musicPlayer;
		private static GameObject _musicPlayer;
		
		public static int selectedLevelIndex;
		
		public static GameManager Instance { get; private set; }
		
		private void OnValidate()
		{
			for(int i = 0; i < itemInfos.Length; i++)
				itemInfos[i].InitializeUI(_itemContainers[i]);
		}
		
		private void Awake()
		{
			Instance = this;
			_audioSource = GetComponent<AudioSource>();
			
			_levelManager.LoadData();
			
			// for(int i = 0; i < _levels.Length; i++)
				// _levels[i].gameObject.SetActive(i == selectedLevelIndex);
			
			if(level)
				level.gameObject.SetActive(true);
			
			if(_musicPlayer)
				Destroy(m_musicPlayer);
			
			else
			{
				_musicPlayer = m_musicPlayer;
				DontDestroyOnLoad(_musicPlayer);
			}
		}
		
		private IEnumerator Start()
		{
			_resetButton.interactable = false;
			_deleteButton.interactable = false;
			_rotateButton.interactable = false;
			
			yield return null;
			
			for(int i = 0; i < itemInfos.Length; i++)
				itemInfos[i].InitializeUI(_itemContainers[i]);
			
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
			
			var item = itemInfos[index].InstantiatePrefab(_itemSpawnPoint);
			
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
				if(!item.enabled) continue;
				
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
			{
				var item = ItemInstances[i];
				
				if(item.enabled)
					Destroy(item.gameObject);
			}
			
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
				foreach(var info in itemInfos)
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
			if(!Application.isPlaying) return;
			
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
		
		public void SetHint(string text, Sprite[] frames, AudioClip clip)
		{
			_hintTxt.text = text;
			_hintAnimation.sprites = frames;
			_hintClip = clip;
		}
		
		public void ReadHint()
		{
			_audioSource.clip = _hintClip;
			
			_audioSource.Stop();
			_audioSource.Play();
		}
		
		public void NextLevel()
		{
			selectedLevelIndex ++;
			
			if(selectedLevelIndex < _levelManager.levelScenes.Length)
				SceneManager.LoadScene(_levelManager.levelScenes[selectedLevelIndex]);
			
			else
				MainMenu();
		}
		
		public void MainMenu()
		{
			if(_musicPlayer)
				Destroy(_musicPlayer);
			
			string sceneName = _levelManager.menuScene;
			SceneManager.LoadScene(sceneName);
		}
	}
}