using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ChargeMeUp
{
	public partial class Item : MonoBehaviour,
		ISelectable,
		IPointerEnterHandler,
		IPointerExitHandler,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler,
		IPointerClickHandler
	{
		public Transform parentAfterDrag { get; set; }
		public Vector3 localPositionAfterDrag { get; set; }
		
		public static Item CurrentlyDragged { get; private set; }
		
		[SerializeField] private CanvasGroup _canvasGroup;
		private void Start() => _canvasGroup = GetComponent<CanvasGroup>();
		
		public void OnBeginDrag(PointerEventData data)
		{
			CurrentlyDragged = this;
			
			parentAfterDrag = transform.parent;
			localPositionAfterDrag = transform.localPosition;
			
			transform.SetParent(GameManager.Instance.DefaultDragParent);
			
			_canvasGroup.blocksRaycasts = false;
			transform.localScale = Vector3.one;
			
			Selection.Instance.Select(this);
			
			// CircuitManager.Instance.UpdateTicks();
		}
		
		public void OnDrag(PointerEventData data)
		{
			var targetPosition = Input.mousePosition;
				position = targetPosition;
		}
		
		public void OnEndDrag(PointerEventData data)
		{
			if(CurrentlyDragged == this)
				CurrentlyDragged = null;
			
			transform.SetParent(parentAfterDrag);
			transform.localPosition = localPositionAfterDrag;
			
			_canvasGroup.blocksRaycasts = true;
			transform.localScale = Vector3.one;
			
			GameManager.Instance?.OnItemMoved(this);
			
			// CircuitManager.Instance.UpdateTicks();
		}
		
		public void Rotate(float angle = 90f) => transform.eulerAngles += Vector3.back * angle;
	}
}