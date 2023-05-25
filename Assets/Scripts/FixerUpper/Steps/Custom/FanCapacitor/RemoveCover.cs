using UnityEngine;

namespace FixerUpper.CustomSteps.FanCapacitor
{
	public class RemoveCover : Step
	{
		// Loosen screw with Screwdriver
		// Remove the screw
		// Remove the cover
		
		[SerializeField] private ItemSlot _screwSlot, _capSlot;
		[SerializeField] private DraggableItem _cap;
		
		public override void Execute()
		{
			// Don't proceed if the step is done already
			if(IsDone) return;
			
			// Don't proceed if the screw is still placed in the slot
			if(_screwSlot.IsOccupied())
				return;
			
			// Enable dragging for the cap
			_cap.enabled = true;

			// Don't proceed until the cap is removed from the slot 
			if(_capSlot.IsOccupied())
				return;
			
			// Proceed
			base.Execute();
			
			_capSlot.gameObject.SetActive(false);
		}	
	}
}