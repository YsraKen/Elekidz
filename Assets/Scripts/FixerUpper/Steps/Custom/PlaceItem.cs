using UnityEngine;

namespace FixerUpper.CustomSteps
{
	public class PlaceItem : Step
	{
		[SerializeField] private DraggableItem _targetItem;
		
		public void OnSlotItemAdded(DraggableItem item)
		{
			if(item == _targetItem)
				Execute();
		}
	}
}