using UnityEngine;
using System;

namespace FixerUpper.CustomSteps.Speaker
{
	public class PlaceCover : Step
	{
		[SerializeField] private Step[] _otherSteps;
		
		public override void Execute()
		{
			bool hasUnfinishedSteps = Array.Exists(_otherSteps, step => !step.IsDone);
			
			if(!hasUnfinishedSteps)
				base.Execute();
		}
	}
}