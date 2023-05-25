using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace FixerUpper
{
	public class Highlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private Image _highlight;
		[SerializeField] private Outline _outline;
		
		[SerializeField] private UnityEvent<bool> _onHighlight;
		
		public void OnPointerEnter(PointerEventData data) => SetShown(true);
		public void OnPointerExit(PointerEventData data) => SetShown(false);
		
		private void SetShown(bool isShown)
		{
			if(!enabled) return;
			if(!_highlight) return;
			
			_onHighlight.Invoke(isShown);
			_highlight.gameObject.SetActive(isShown);
			
			// Debug.Log(isShown ? "Enter": "Exit");
		}
		
		public void SetColor(Color color)
		{
			_highlight.color = color;
			_outline.effectColor = color;
		}
		
		private void Start(){}
	}
}