using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CurrentRun {
public class GameManager : MonoBehaviour {
	
	public Transform platformGenerator;
	private Vector3 platformStartPoint;
	
	public PlayerController thePlayer;
	private Vector3 playerStartpoint;
	
	private	PlatformDestroyer [] platformList;
	
	private ScoreManager theScoreManager;
	
	public DeathMenu theDeathScreen;
	public startGameMenu theStartMenu;
	public GameObject TextManager;
	public GameObject counterObj;
	public GameObject pauseButton;
	public GameObject howToPlay; 
	
	public GameObject Elekidz;
	
	public RollCam theRollcam;
	public CameraController camControl;
	private Vector3 CamPosition;
	
	public string playGameLevel;
	public AudioSource bgSound;
	public AudioSource StartSound;
	public AudioSource DeathSound;
	
	
	
	
    // Start is called before the first frame update
    void Start() {
        platformStartPoint = platformGenerator.position;
		playerStartpoint = thePlayer.transform.position;
		theScoreManager = FindObjectOfType <ScoreManager>();
		thePlayer.gameObject.SetActive (false);
		thePlayer.enabled = false;
		Elekidz.gameObject.SetActive (true);
		theStartMenu.gameObject.SetActive (true);
		theDeathScreen.gameObject.SetActive(false);
		theRollcam = FindObjectOfType<RollCam>();
		camControl = FindObjectOfType<CameraController>();
		TextManager.gameObject.SetActive(false);
		counterObj.gameObject.SetActive(false);
		pauseButton.gameObject.SetActive (false);
		howToPlay.gameObject.SetActive (false);
		bgSound.Play();
		StartSound.Stop();
		DeathSound.Stop();
	
    }

    // Update is called once per frame
    void Update() {
        
    }
	public void Reset () //Restart Game Function
	{
		//theStartMenu.gameObject.SetActive (true);
		//theDeathScreen.gameObject.SetActive(false);
		//thePlayer.transform.position = playerStartpoint;
		//platformGenerator.position = platformStartPoint;
		SceneManager.LoadScene(playGameLevel);
		

	}
	
	public void RestartGame()
	{
		theDeathScreen.gameObject.SetActive(true);
		theStartMenu.gameObject.SetActive (false);
		theScoreManager.scoreIncreasing = false;
		thePlayer.gameObject.SetActive (false);
		//StartCoroutine ("RestartGameCo");
		thePlayer.transform.position = playerStartpoint;
		platformGenerator.position = platformStartPoint;
		TextManager.gameObject.SetActive(false);
		counterObj.gameObject.SetActive(false);
		pauseButton.gameObject.SetActive (false);
		bgSound.Stop();
		StartSound.Stop();
		DeathSound.Play();
		
		//theScoreManager.scoreCount = 0;
		thePlayer.moveSpeed = 12; 
		thePlayer.speedCount = 0;
		thePlayer.speedPercent = 100; 
		thePlayer.increaseSpeed = 0;
		thePlayer.decreaseSpeed =0;
		
		
		theRollcam.enabled = true;
		camControl.enabled = false;
	}
	
	public void StartGame () //Start Button function
	{
		theDeathScreen.gameObject.SetActive(false);
		theStartMenu.gameObject.SetActive (false);
		thePlayer.gameObject.SetActive(true);
		thePlayer.enabled = true;
		TextManager.gameObject.SetActive(true);
		counterObj.gameObject.SetActive(true);
		pauseButton.gameObject.SetActive (true);
		bgSound.Stop();
		//StartSound.Play();
		DeathSound.Stop();
		
		platformList = FindObjectsOfType <PlatformDestroyer> ();
		for (int i= 0 ; i < platformList.Length ; i++)
		{
			platformList [i].gameObject.SetActive(false);
		}
		
		thePlayer.transform.position = playerStartpoint;
		platformGenerator.position = platformStartPoint;
		thePlayer.gameObject.SetActive(true);
		
		Elekidz = GameObject.Find("Elekidz");
		Elekidz.gameObject.SetActive(true);
		
		theScoreManager.scoreIncreasing = true;
		theRollcam.enabled = false;
		
	}
	/*public IEnumerator RestartGameCo()
	{
		theScoreManager.scoreIncreasing = false;
		thePlayer.gameObject.SetActive (false);
		yield return new WaitForSeconds (0.5f);
		platformList = FindObjectsOfType <PlatformDestroyer> ();
		for (int i= 0 ; i < platformList.Length ; i++)
		{
			platformList [i].gameObject.SetActive(false);
		}
		
		thePlayer.transform.position = playerStartpoint;
		platformGenerator.position = platformStartPoint;
		thePlayer.gameObject.SetActive(true);
		
		theScoreManager.scoreCount = 0;
		theScoreManager.scoreIncreasing = true;
		thePlayer.moveSpeed = 12; 
		thePlayer.speedCount = 0;
		thePlayer.speedPercent = 100; 
		thePlayer.increaseSpeed = 0;
		thePlayer.decreaseSpeed =0;
		
		 
	} */
	
	
}
}