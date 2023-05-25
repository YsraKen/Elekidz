using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FixerUpper
{
	public class Hint : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Text _text;
		[SerializeField] private float _textAnimationDuration = 3f;
		
		public static GameObject CurrentHint { get; set; }
		public static bool hasNextHint { get; set; }
		
		private void OnEnable()
		{
			if(!Application.isPlaying) return;
			
			StartCoroutine(r());
			
			IEnumerator r()
			{
				string text = _text.text;
				_text.text = string.Empty;
				
				int length = text.Length;
				float duration = _textAnimationDuration / (float) length;
				var step = new WaitForSeconds(duration);
				
				for(int i = 0; i < length; i++)
				{
					_text.text += text[i];
					yield return step;
				}
			}
			
			GameManager.Instance.HintCloseButton.SetActive(true);
		}
		
		private void LateUpdate()
		{
			if(_target)
				transform.position = _target.position;
		}
		
		public void OnOkButton()
		{
			if(CurrentHint)
			{
				CurrentHint.SetActive(false);
				GameManager.Instance.HintCloseButton.SetActive(false);
			}
			
			if(hasNextHint)
				GameManager.Instance.ShowHint();
		}
	}
}