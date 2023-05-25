using UnityEngine;
using System;

namespace FixerUpper.CustomSteps.Radio
{
	public class FixScrews : Step
	{
		[SerializeField] private Screw[] _screws;
		[SerializeField] private DraggableItem _cover;
		[SerializeField] private Step[] _otherSteps;
		
		public override void Execute()
		{
			// Check for unfinished steps
			bool hasUnfinishedSteps = Array.Exists(_otherSteps, step => !step.IsDone);
			
			if(hasUnfinishedSteps)
				return;
			
			// Check for cover being fixed by screws
			bool hasFixedScrews = Array.Find(_screws, screw => screw.IsFixed);
			// _cover.enabled = !hasFixedScrews;
			
			if(hasFixedScrews)
				_cover.enabled = false;
			else
				_cover.enabled = true;
			
			// check for progress by checking if all screws are fixed
			bool hasUnfixedScrews = Array.Find(_screws, screw => !screw.IsFixed);
			
			if(hasUnfixedScrews)
				return;
			
			// finish
			base.Execute();
		}
	}
}