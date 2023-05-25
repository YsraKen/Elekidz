using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurrentRun {
public class Coins : MonoBehaviour
{
	
	public int scoreToGive; 
	public float changeSpeed; 

	
	private PlayerController thePlayerController;
	private ScoreManager theScoreManager;
	
	
	
    // Start is called before the first frame update
    void Start()
    {
        theScoreManager = FindObjectOfType<ScoreManager> ();
		thePlayerController = FindObjectOfType<PlayerController> ();
    }

    // Update is called once per frame
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.name == "Elekidz") 
		{
			theScoreManager.AddScore(scoreToGive); 
			gameObject.SetActive (false);
		}
	}
}
}