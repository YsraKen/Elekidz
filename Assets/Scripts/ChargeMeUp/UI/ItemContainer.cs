using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace ChargeMeUp
{
	public class ItemContainer : MonoBehaviour,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler
	{
		public int itemIndex;
		
		[Space]
		[SerializeField] private Image _iconImg;
		[SerializeField] private TMP_Text _labelTxt, _countTxt;
		
		private Item _draggedItem;
		private GameManager _gameMgr;
		
		private void Start() => _gameMgr = GameManager.Instance;
		
		public void SetupUI(Sprite icon, string label)
		{
			_iconImg.sprite = icon;
			_labelTxt.text = label;
		}
		
		public void SetCount(int count) => _countTxt.text = count.ToString();
		
		public void OnBeginDrag(PointerEventData data)
		{
			_gameMgr.SpawnItem(itemIndex);
			_draggedItem = _gameMgr.ItemInstances[_gameMgr.ItemInstances.Count - 1];
			
			_draggedItem.OnBeginDrag(data);
		}
		
		public void OnDrag(PointerEventData data) => _draggedItem?.OnDrag(data);
		
		public void OnEndDrag(PointerEventData data)
		{
			if(!_draggedItem)
				return;
			
			_draggedItem.OnEndDrag(data);
			_draggedItem = null;
		}
		
		public void OnClick() => _gameMgr.SpawnItem(itemIndex);
	}
}