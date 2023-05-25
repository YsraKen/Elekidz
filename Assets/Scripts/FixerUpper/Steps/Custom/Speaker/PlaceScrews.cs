using UnityEngine;
using System;
using System.Collections;

namespace FixerUpper.CustomSteps.Speaker
{
	public class PlaceScrews : Step
	{
		[SerializeField] private Step[] _otherSteps;
		[SerializeField] private Screw[] _screws;
		[SerializeField] private DraggableItem _cover;
		
		public override void Execute()
		{
			// Check for unfinished steps
			bool hasUnfinishedSteps = Array.Exists(_otherSteps, step => !step.IsDone);
			if(hasUnfinishedSteps) return;
			
			// Check for fixed/unfixed screws
			bool allScrewFixed = true;
			
			foreach(var screw in _screws)
			{
				// OTHER: disable dragging for the cover when there's a fixed screw
				if(screw.IsFixed)
					_cover.enabled = false;
				
				// Check for fixed/unfixed screws
				else
					allScrewFixed = false;
			}
			
			// Finish
			if(allScrewFixed)
				base.Execute();
			
			// Coroutine is needed for delaying a single frame
			// StopAllCoroutines();
			// StartCoroutine(ExecuteRoutine());
		}
		
		/* private IEnumerator ExecuteRoutine()
		{
			// Apply 'this' to the srews target step
			foreach(var screw in _screws)
				if(screw.targetStep != this)
					screw.targetStep = this;
			
			// 1 frame delay
			yield return null;
			
			// Check for fixed/unfixed screws
			bool allScrewFixed = true;
			
			foreach(var screw in _screws)
			{
				// OTHER: disable dragging for the cover when there's a fixed screw
				if(screw.IsFixed)
					_cover.enabled = false;
				
				// Check for fixed/unfixed screws
				else
					allScrewFixed = false;
			}
			
			// Finish
			if(allScrewFixed)
				base.Execute();
		} */
	}
}