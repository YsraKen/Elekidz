using UnityEngine;
using System.Collections.Generic;

namespace FixerUpper.CustomSteps.FanCapacitor
{
	public class RemoveOldCapacitor : Step
	{
		[SerializeField] private List<Tooled> _cuttableWires = new List<Tooled>();
		
		// to be called from interactable's (wire) "On Interaction" event
		public void OnWireCut(Tooled wire)
		{
			if(_cuttableWires.Contains(wire))
				_cuttableWires.Remove(wire);
		}
		
		// to ba called from interactable's (wire) "Target Step"
		public override void Execute()
		{
			if(_cuttableWires.Count == 0)
				base.Execute();
		}
	}
}