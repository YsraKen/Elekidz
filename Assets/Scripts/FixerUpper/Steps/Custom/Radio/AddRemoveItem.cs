using UnityEngine;
using System;

namespace FixerUpper.CustomSteps.Radio
{
	public class AddRemoveItem : Step
	{
		[SerializeField] private DraggableItem _item;
		[SerializeField] private Step[] _otherSteps;
		
		public void OnItemAdded(DraggableItem item)
		{
			if(item == _item)
				Execute();
		}
		
		public override void Execute()
		{
			bool hasUnfinishedSteps = Array.Exists(_otherSteps, step => !step.IsDone);
			
			if(!hasUnfinishedSteps)
				base.Execute();
		}
	}
}