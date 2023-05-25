using UnityEngine;

namespace FixerUpper
{
	public class Screw : Tooled
	{
		[field: SerializeField] public bool IsFixed { get; private set; }
		[SerializeField] private DraggableItem _draggableItem;
		
		// GPX
		[SerializeField] private GameObject _fixed, _loose, _unattached;
		
		private void Start() => _draggableItem.enabled = !IsFixed;
		
		public override void Interact()
		{
			// Don't fix unless the screw is attached to a slot
			if(!IsFixed && !IsAttached())
				return;
			
			// toggle states
			IsFixed = !IsFixed;
			_draggableItem.enabled = !IsFixed;
			
			// GPX
			SetActive(_fixed, IsFixed);
			SetActive(_loose, !IsFixed);
			SetActive(_unattached, false);
			
			base.Interact();
		}
		
		#region Events
		
		public void OnDragged()
		{
			SetActive(_fixed, false);
			SetActive(_loose, false);
			SetActive(_unattached, true);
		}
		
		public void OnDropped()
		{
			if(IsAttached())
			{
				SetActive(_unattached, false);
				SetActive(_fixed, IsFixed);
				SetActive(_loose, !IsFixed);
			}
			else
			{
				SetActive(_unattached, true);
				SetActive(_fixed, false);
				SetActive(_loose, false);
			}
		}
		
		#endregion
		
		#region Utils
		
		// Checks if the screw is attached to a slot
		private bool IsAttached()
		{
			bool isAttached = default;
			var slot = GetComponentInParent<ItemSlot>();
			
			if(slot)
			{
				string id = _draggableItem.ID;
				isAttached = slot.ProcessID(id);
			}
			
			return isAttached;
		}
		
		// Set active and avoid null error message. "?" operator is not working with GameObject
		private void SetActive(GameObject obj, bool isActive)
		{
			if(obj)
				obj.SetActive(isActive);
			
			// obj?.SetActive(isActive)
		}
		
		#endregion
	}
}