using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace FixerUpper
{
	public class ItemSlot : MonoBehaviour, IDropHandler
	{
		[SerializeField] private string[] _acceptedIds;
		[SerializeField] private int _space = 1;
		
		[SerializeField] private Transform _itemParent;
		public Transform ItemParent => _itemParent? _itemParent: transform;
		
		[SerializeField] private UnityEvent<DraggableItem> _onItemAdded;
		
		public void OnDrop(PointerEventData data)
		{
			if(IsFull()) return;
			
			var item = DraggableItem.CurrentlyDragged; 
			if(!item) return;
			
			bool isIdAccepted = ProcessID(item.ID);
			
			if(isIdAccepted)
			{
				item.targetSlotAfterDrag = this;
				_onItemAdded.Invoke(item);
			}
		}
		
		// Only allow certain item to be placed in 'this' slot using 'id'
		public bool ProcessID(string id)
		{
			bool isAccepted = _acceptedIds == null || _acceptedIds.Length == 0;
			
			if(!isAccepted)
				isAccepted = Array.Exists(_acceptedIds, acceptedId => acceptedId == id);
			
			return isAccepted;
		}
		
		public bool IsOccupied() => ItemParent.childCount > 0;
		public bool IsFull() => ItemParent.childCount >= _space;
		
		public DraggableItem GetItem(int index = 0) => ItemParent.GetChild(index).GetComponent<DraggableItem>();
	}
}