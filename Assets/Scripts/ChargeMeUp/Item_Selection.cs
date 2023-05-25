using UnityEngine;
using UnityEngine.EventSystems;

namespace ChargeMeUp
{
	public partial class Item
	{
		[SerializeField] private Highlight _highlight;
		
		private bool _isSelected;
		
		public Vector3 position
		{
			get => transform.position;
			set => transform.position = value;
		}
		
		public void OnPointerEnter(PointerEventData data)
		{
			if(!_isSelected)
				_highlight.SetActive(true);
		}
		
		public void OnPointerExit(PointerEventData data)
		{
			if(!_isSelected)
				_highlight.SetActive(false);
		}
		
		public void OnPointerClick(PointerEventData data) => Selection.Instance.Select(this);
		
		public void OnSelectionUpdateChanged(bool isSelected)
		{
			_isSelected = isSelected;
			
			if(_isSelected)
				_highlight.SetColor(Color.green);
			
			else
				_highlight.ResetColor();
			
			_highlight.SetActive(_isSelected);
		}
	}
}