using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CurrentRun {
public class PlayerController : MonoBehaviour {
		
		public float moveSpeed; 
		public float speedPercent;
		public float speedCount; 
		public float increaseSpeed;
		public float decreaseSpeed;
		
		public float speedMultiplier;
		public float speedIncreaseMilestone;
		private float speedMilestoneCount;
		
		public Coins speedEffectCoinVolt;
		public Coins speedEffectCoinOhm;

		public Text speedText;
	
		public float jumpForce; 
		public float jumpTime;
		private float jumpTimeCounter;
		
		private bool stoppedJumping;
		private bool canDoubleJump;
		
		private Rigidbody2D myRigidbody; 
		
		public bool grounded; 
		public LayerMask  whatIsGround;
		public Transform groundCheck;
		public float groundCheckRadius;
		
		
		private Animator myAnimator;
		public GameManager theGameManager;
		public startGameMenu theStartMenu;
		
		public GameObject CameraShake;
		
		public AudioSource coinSound;
		public AudioSource BGSound;

		
		
		
	
		
    // Start is called before the first frame update
    void Start() {
        myRigidbody = GetComponent<Rigidbody2D> ();
		
		myAnimator = GetComponent<Animator> ();
		
		jumpTimeCounter = jumpTime;
		
		stoppedJumping = true;
		
		theStartMenu = FindObjectOfType<startGameMenu>();
		//theStartMenu.gameObject.SetActive (false);
		
		speedMilestoneCount = speedIncreaseMilestone;
		BGSound.Play();
    }

    // Update is called once per frame
    void Update() {
			
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		myRigidbody.velocity = new Vector2(moveSpeed , myRigidbody.velocity.y );
		
		if (transform.position.x > speedMilestoneCount) 
		{
			
			speedMilestoneCount += speedIncreaseMilestone;
			speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;
			moveSpeed = moveSpeed * speedMultiplier;
			
	
		}

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() ) 
		{
		
			if(grounded)
			{
				myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce); 
				stoppedJumping = false;
				
			} 
			
			if(!grounded && canDoubleJump) 
			{
				myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce); 
				jumpTimeCounter = jumpTime;
				stoppedJumping = false;
				canDoubleJump = false; 
			}
			

			 
		}
		
	    if ((Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButton (0))  && !stoppedJumping)
		{
			if (jumpTimeCounter > 0 ) 
			{
				stoppedJumping = false; 
				myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce); 
				jumpTimeCounter -= Time.deltaTime;
			}  
			

		}
		
		if(Input.GetKeyUp (KeyCode.Space) || Input.GetMouseButtonDown (0)) 
		{
			jumpTimeCounter = 0;
			stoppedJumping = true;
			

			
		} 
		
		if(grounded)
		{
			
			jumpTimeCounter = jumpTime;
			canDoubleJump = true;
			
		} 
		
		myAnimator.SetFloat ("Speed", myRigidbody.velocity.x);
		myAnimator.SetBool ("Grounded", grounded);
		
		speedPercent = (moveSpeed/12f)*100f;
		speedText.text = "Charge: " + Mathf.Round (speedPercent) + "%";
		
	}
	
	
	
	void OnCollisionStay2D(Collision2D other) 
	{
		if(other.gameObject.tag == "killbox")
        {
            theGameManager.RestartGame();
			
        }
		
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{

		if(col.gameObject.tag == "Ohm") 
		{
			decreaseSpeed += speedEffectCoinOhm.changeSpeed; 
			moveSpeed = moveSpeed - decreaseSpeed;
			coinSound.Play();
			
		}
		
		if(col.gameObject.tag == "Volt") 
		{
			increaseSpeed  += speedEffectCoinVolt.changeSpeed; 
			moveSpeed = moveSpeed + increaseSpeed;
			CameraShake.gameObject.SetActive(true);
			coinSound.Play();


		}
		
		//speedCount = increaseSpeed - decreaseSpeed;
		
		//moveSpeed = moveSpeed + increaseSpeed;
		//moveSpeed = moveSpeed - decreaseSpeed;
		myRigidbody.velocity = new Vector2(moveSpeed , myRigidbody.velocity.y );
		myAnimator.SetFloat ("Speed", myRigidbody.velocity.x);
		//speedPercent = (moveSpeed/12f)*100f;
		//speedText.text = "Speed: " + Mathf.Round (speedPercent) + "%";
		
	}
	

	

	}	
}