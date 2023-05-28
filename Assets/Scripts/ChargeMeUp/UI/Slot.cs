using UnityEngine;

namespace ChargeMeUp
{
	public class Slot : MonoBehaviour
	{
		[SerializeField] private Transform _itemParent;
		public Transform ItemParent => _itemParent? _itemParent: transform;
		
		public void OnBeginDrag()
		{
			if(!IsOccupied())
				Selection.Instance?.StartMultiSelectionMode();
		}
		
		public void OnDrop()
		{
			if(IsOccupied()) return;
				
			var component = Item.CurrentlyDragged;
			
			if(!component)
				return;
			
			component.parentAfterDrag = ItemParent;
			component.localPositionAfterDrag = default;
		}
		
		public void OnPointerClick() => Selection.Instance.DeselectAll();
		
		public bool IsOccupied() => ItemParent.childCount > 0;
	}
}