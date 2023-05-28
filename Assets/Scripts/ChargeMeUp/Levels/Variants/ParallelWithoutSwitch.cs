using UnityEngine;

namespace ChargeMeUp
{
	using Experimental.Electronics;
	
	public class ParallelWithoutSwitch : Level
	{
		[SerializeField] private string _tWireId = "tWire";
		[SerializeField] private int _numberOfTWires = 4;
		
		[Space]
		[SerializeField] private int _numberOfLoops = 3;
		[SerializeField] private string _bulbId = "defaultLoad";
		
		private CircuitManager _circuitMgr;
		
		protected override void Start()
		{
			base.Start();
			
			_circuitMgr = CircuitManager.Instance;
			_circuitMgr.onTickFinished += CheckProgress;
		}
		
		private void CheckProgress()
		{
#region Count the Sources

			if(_circuitMgr.SourceInstances.Count != 1)
				return;
			
			var source = _circuitMgr.SourceInstances[0];
		
			// "walang wire na walang connection (open circuit)"
			_circuitMgr.checkForOpenNodes = true;
			
			if(source.HasOpenNode)
				return;
			
#endregion

#region Search for T wires
			
			int tWireCount = default;
			
			foreach(var instance in _circuitMgr.ElectronPathInstances)
			{
				var load = instance as Load;
				if(!load) continue;
				
				bool isTWire = load.ComponentId == _tWireId;
				if(!isTWire) continue;
				
				// "may ginamit na atleast 4 T-wire na nag ON"
				if(load.IsEnergized)
					tWireCount ++;
			}
			
			if(tWireCount < _numberOfTWires)
				return;
			
#endregion

#region Check circuit/loop/path count

		if(source.PathInfos.Count != _numberOfLoops)
			return;

#endregion

#region Search for Bulbs

			int totalBulbCount = 0;
			
			foreach(var instance in _circuitMgr.ElectronPathInstances)
			{
				var component = instance as Component;
				if(!component) continue;
				
				if(component.ComponentId == _bulbId)
					totalBulbCount ++;
			}
			
			if(totalBulbCount != _numberOfLoops)
				return;
			
#endregion

#region Identify if the circuit is parallel

			var loops = source.PathInfos;
			
			bool isParallel = true;
			
			// search if each loops has one bulb in it
			foreach(var loop in loops)
			{
				int bulbCount = default;
				
				foreach(var element in loop.elements)
				{
					var component = element as Component;
					if(!component) continue;
					
					if(component.ComponentId == _bulbId)
						bulbCount ++;
				}
				
				if(bulbCount != 1)
				{
					isParallel = false;
					break;
				}
			}
			
			if(!isParallel) return;

#endregion
			
			FinishLevel();
		}
	}
}