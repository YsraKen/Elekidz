using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CurrentRun {
public class ScoreManager : MonoBehaviour {
	
	public Text scoreText;
	public Text scoreTextDisplay;
	public Text scoreTextDisplay2;
	public Text highScoreText;
	
	public float scoreCount;
	public float highScoreCount;
	
	public float pointsPerSecond;
	
	public bool scoreIncreasing;
	public PlayerController thePlayer;
	
	public GameObject StartPoint; 
	public GameObject Elekidz; 
	
    // Start is called before the first frame update
    void Start()
    {
		StartPoint = GameObject.Find("StartPoint");
		//Elekidz = GameObject.Find("Elekidz");
        if (PlayerPrefs.HasKey("HighScore"))
		{
			highScoreCount = PlayerPrefs.GetFloat ("HighScore");
 
		}
    }

    // Update is called once per frame
    void Update()
    {
		/*thePlayer = FindObjectOfType <PlayerController> ();
		StartPoint = GameObject.Find("StartPoint");
		Elekidz = GameObject.Find("Elekidz"); */
		if (scoreIncreasing == true) 
		{
			scoreCount =  Elekidz.transform.position.x - StartPoint.transform.position.x;
			Debug.Log(scoreCount);
		}
			
		if( scoreCount > highScoreCount ) 
		{
			highScoreCount = scoreCount; 
			PlayerPrefs.SetFloat ("HighScore" , highScoreCount);
		}

        scoreText.text = "Score: " + Mathf.Round (scoreCount);
		scoreTextDisplay.text = "Score: " + Mathf.Round (scoreCount);
		highScoreText.text = "High score: " + Mathf.Round(highScoreCount);
		
    }
	
	public void AddScore (int pointsToAdd) 
	{
		scoreCount += pointsToAdd; 
		
	}
}
}