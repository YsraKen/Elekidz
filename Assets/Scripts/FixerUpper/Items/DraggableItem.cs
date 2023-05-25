using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FixerUpper
{
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(Animation))]
	public class DraggableItem : MonoBehaviour,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler
	{
		#region Properties
		
		// Inspector
		[field: SerializeField] public string ID { get; private set; }
		[SerializeField] private UnityEvent _onBeginDrag, _onDrag, _onEndDrag;
		
		// Drag & Drop References
		public static DraggableItem CurrentlyDragged { get; private set; }
		
		private ItemSlot _currentSlot;
		private Vector3 _currentLocalPositionOnSlot;
		
		[HideInInspector] public ItemSlot targetSlotAfterDrag;
		
		// Handling GPX	
		protected Animation _animation;
		private CanvasGroup _canvasGroup;
		private Camera _cam;
		
		#endregion
		
		private void Start()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_cam = Camera.main;
			
			_animation = GetComponent<Animation>();
		}
		
		#region Dragging
		
		public void OnBeginDrag(PointerEventData data)
		{
			// References
			CurrentlyDragged = this;
			
			if(!_currentSlot)
				_currentSlot = GetComponentInParent<ItemSlot>();
			
			// Parenting
			_currentLocalPositionOnSlot = _transform.localPosition;
			targetSlotAfterDrag = null;
			
			_transform.SetParent(GameManager.Instance.DefaultDragParent);
			
			// Events
			_canvasGroup.blocksRaycasts = false;
			_onBeginDrag.Invoke();
			
			_animation.Play("pop");
		}
		
		public void OnDrag(PointerEventData data)
		{
			var targetPosition = _cam.ScreenToWorldPoint(data.position);
				targetPosition.z = 0f;
				
				_transform.position = targetPosition;
			
			_onDrag.Invoke();
		}
		
		public void OnEndDrag(PointerEventData data)
		{
			if(CurrentlyDragged != this)
				return;
			
			CurrentlyDragged = null;
			
			if(targetSlotAfterDrag)
			{
				_transform.SetParent(targetSlotAfterDrag.ItemParent);
				_currentSlot = targetSlotAfterDrag;
			}
			
			else
			{
				_transform.SetParent(_currentSlot? _currentSlot.ItemParent: _transform.parent);
				_transform.localPosition = _currentLocalPositionOnSlot;
			}
			
			targetSlotAfterDrag = null;
			
			_canvasGroup.blocksRaycasts = true;
			_onEndDrag.Invoke();
			
			_animation.Play("pop");
		}
		
		#endregion
	}
}