using UnityEngine;
using System;

namespace FixerUpper.CustomSteps.FanCapacitor
{
	public class PlaceCover : Step
	{
		[SerializeField] private ItemSlot _capSlot;
		[SerializeField] private Screw _screw;
		
		[Space]
		[SerializeField] private Step[] _otherSteps;
		
		public override void Execute()
		{
			bool hasUnfinishedSteps = Array.Exists(_otherSteps, step => !step.IsDone);
			
			if(hasUnfinishedSteps)
				return;
			
			if(_capSlot.IsOccupied() && _screw.IsFixed)
				base.Execute();
		}
	}
}