using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

namespace FixerUpper
{
	public class Tooled : Interactable, IPointerEnterHandler, IPointerExitHandler, IDropHandler
	{
		[field: SerializeField] public Tool targetTool { get; set; }
		
		[SerializeField] private float _interactionDuration;
		[SerializeField] private Highlight _highlight;
		
		[SerializeField] private bool _autoCancelProgress;
		private float _progress;
		
		#region EventSystems
		
		public void OnPointerEnter(PointerEventData data)
		{
			if(!hasInteractionDuration) return;
			
			if(IsProperToolUsed())
				StartCoroutine(StartInteracting());
		}
		
		public void OnPointerExit(PointerEventData data) => StopInteracting();
		
		public void OnDrop(PointerEventData data)
		{
			if(hasInteractionDuration)
			{
				StopInteracting();
				return;
			}
			
			if(IsProperToolUsed())
			{
				Interact();
				targetTool.Use();
			}
		}
		
		#endregion
		
		private IEnumerator StartInteracting()
		{
			targetTool.Use(autoStop: false);
			
			float timer = _interactionDuration * _progress;
			var generalUI = GeneralUI.Instance;
			
			while(timer < _interactionDuration)
			{
				yield return null;
				
				timer += Time.deltaTime;
				_progress = Mathf.Clamp01(timer / _interactionDuration);
				
				generalUI.ShowMiniProgress(_transform.position, _progress);
			}
			
			targetTool.StopUsing();
			yield return null;
			
			Interact();
			generalUI.HideMiniProgress();
		}
		
		public override void Interact()
		{
			_progress = 0f;
			base.Interact();
		}
		
		private void StopInteracting()
		{
			if(_progress <= 0f) return;
			
			StopAllCoroutines();
			
			if(_autoCancelProgress)
				_progress = 0f;
			
			// targetTool.StopUsing();
			targetTool.StopAllCoroutines();
			GeneralUI.Instance.HideMiniProgress();
		}
		
		private bool hasInteractionDuration => _interactionDuration > 0f;
		private bool IsProperToolUsed() => DraggableItem.CurrentlyDragged == targetTool;
		
		// Highlight
		public void Highlight(bool isShown)
		{
			if(!isShown) return;
			
			var currentTool = DraggableItem.CurrentlyDragged;
			var highlightColor = Color.yellow;
			
			if(currentTool)
			{
				bool isCorrectTool = currentTool == targetTool;
				highlightColor = isCorrectTool? Color.green: Color.red;
			}
			
			_highlight.SetColor(highlightColor);
		}
	}
}