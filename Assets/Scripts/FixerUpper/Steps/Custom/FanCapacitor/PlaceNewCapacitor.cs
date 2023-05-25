using UnityEngine;
using System.Collections.Generic;

namespace FixerUpper.CustomSteps.FanCapacitor
{
	public class PlaceNewCapacitor : Step
	{
		[SerializeField] private DraggableItem _newCapacitor;
		[SerializeField] private ItemSlot _capacitorSlot;
		
		[SerializeField] private List<Tooled> _solderableWires = new List<Tooled>();
		[SerializeField] private List<Tooled> _tapableWires = new List<Tooled>();
		
		[SerializeField] private GameObject _solderableWiresParent, _tapableWiresParent;
		
		public override void Execute()
		{
			// Check for capacitor
			var currentlyPlacedCapacitor = _capacitorSlot.GetItem();
			
			if(currentlyPlacedCapacitor != _newCapacitor)
				return;
			
			// Check for solderable wires
			if(_solderableWires.Count > 0)
				return;
			
			_solderableWiresParent.SetActive(false);
			_tapableWiresParent.SetActive(true);
			
			// Check for wires that can be applied with electrical taped
			if(_tapableWires.Count > 0)
				return;
			
			// Proceed
			base.Execute();
		}
		
		public void OnWireSoldered(Tooled wire)
		{
			if(!_solderableWires.Contains(wire))
				return;
			
			_solderableWires.Remove(wire);
			_newCapacitor.enabled = false;
		}
		
		public void OnWireTaped(Tooled wire)
		{
			if(_tapableWires.Contains(wire))
				_tapableWires.Remove(wire);
		}
	}
}