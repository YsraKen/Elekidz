using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
	public Dialogue dialogue;
	
	public void SkipButton () 
	{
		dialogue.Skip ();
	}
}
