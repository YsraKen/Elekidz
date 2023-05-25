using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{
	public TextMeshProUGUI textComponent;
	public Image dialogueImage;
	public DialogueLine[] lines;
	public float textSpeed;
	private int index;
	public GameObject howToPlay;
	
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty ; 
		dialogueImage.gameObject.SetActive(false);
		StartDialogue(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown (0)) 
		{
			if (textComponent.text == lines [index].text)
			{
				NextLine();
			}
			else 
			{
				StopAllCoroutines();
				textComponent.text = lines [index].text;
				
			}
			
			/*Sprite image = lines[index].image;

			dialogueImage.gameObject.SetActive(image);
			dialogueImage.sprite = image; */
		}
    }
	
	void StartDialogue () 
	{
		index = 0;
		StartCoroutine (TypeLine());
		
		Sprite image = lines[index].image;

		dialogueImage.gameObject.SetActive(image);
		dialogueImage.sprite = image; 
	}
	
	IEnumerator TypeLine ()
	{
		foreach (char c in lines [index].text.ToCharArray ()) 
		{
			textComponent.text += c;
			yield return new WaitForSeconds (textSpeed);
			
		}
	}
	void NextLine() 
	{
		if (index < lines.Length -1) 
		{
			index++;
			textComponent.text = string.Empty;
			StartCoroutine(TypeLine());
		}
		else 
		{
			gameObject.SetActive (false);
		}
		
		Sprite image = lines[index].image;

		dialogueImage.gameObject.SetActive(image);
		dialogueImage.sprite = image;
	}
	
	public void Skip () 
	{
		StopAllCoroutines();
		textComponent.text = lines [index].text;
		howToPlay.gameObject.SetActive (false);
	}
	
	public void Button () 
	{
		howToPlay.gameObject.SetActive (true);
	}
	

	[System.Serializable]
	public class DialogueLine
	{
		[TextArea] public string text;
		public Sprite image;
	}
}

