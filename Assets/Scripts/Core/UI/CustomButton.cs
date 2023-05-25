using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CustomButton : MonoBehaviour
{
	public OnClickEvent[] onClickEvents;
	public bool realtime;
	
	[Space]
	[SerializeField] private Animation _animation;
	[SerializeField] private AudioSource _audioSource;
	
	[System.Serializable]
	public class OnClickEvent
	{
		public string name;
		public float delay;
		public UnityEvent action;
		
		public IEnumerator Invoke(bool realtime)
		{
			if(realtime)
				yield return new WaitForSecondsRealtime(delay);
			
			else
				yield return new WaitForSeconds(delay);
			
			action.Invoke();
		}
	}
	
	public void OnClick()
	{
		StopAllCoroutines();
		StartCoroutine(r());
		
		IEnumerator r()
		{
			_animation.Play();
			_audioSource.Play();
			
			foreach(var onClickEvent in onClickEvents)
				yield return onClickEvent.Invoke(realtime);
		}
	}
	
	private void OnValidate()
	{
		if(!_animation)
			_animation = GetComponent<Animation>();
	}
}