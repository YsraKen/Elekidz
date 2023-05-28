using UnityEngine;

namespace ChargeMeUp
{
	using Experimental.Electronics;
	
	// ■ may ginamit na atleast 2 T-wire na nag ON
	// ■ 3 bulbs light up
	// ■ walang wire na walang connection (open circuit)

	public class SeriesParallelWithoutSwitch : Level
	{
		[SerializeField] private string _tWireId = "tWire";
		[SerializeField] private int _numberOfTWires = 2;
		
		[Space]
		[SerializeField] private int _numberOfLoops = 2;
		
		[Space]
		[SerializeField] private int _numberOfBulbs = 3;
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
			{
				_log = "Check the Sources";
				return;
			}

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
			{
				_log = "Search for T wires";
				return;
			}
			
#endregion

#region Check circuit/loop/path count

			var source = _circuitMgr.SourceInstances[0];

			if(source.PathInfos.Count != _numberOfLoops)
			{
				_log = "Check circuit/loop/path count";
				return;
			}

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
			
			if(totalBulbCount != _numberOfBulbs)
			{
				_log = "Search for Bulbs";
				return;
			}
			
#endregion

#region Search for Series Loop
			
			var loops = source.PathInfos;
			
			int bulbCounter = default;
			var seriesLoop = default(PathInfo);
			
			foreach(var loop in loops)
			{
				foreach(var element in loop.elements)
				{
					var component = element as Component;
					if(!component) continue;
					
					if(component.ComponentId == _bulbId)
						bulbCounter ++;
					
					if(bulbCounter > 1)
					{
						seriesLoop = loop;
						break;
					}
				}
			}
			
			if(seriesLoop == null)
			{
				_log = "Search for Series Loop";
				return;
			}
			
#endregion

#region Search for Parallel loop

			bulbCounter = 0;
			bool hasParallelLoop = true;
			
			foreach(var loop in loops)
			{
				if(loop == seriesLoop)
					continue;
				
				foreach(var element in loop.elements)
				{
					var component = element as Component;
					if(!component) continue;
					
					if(component.ComponentId == _bulbId)
						bulbCounter ++;
					
					if(bulbCounter > 1)
					{
						hasParallelLoop = false;
						break;
					}
				}
			}
			
			if(!hasParallelLoop)
			{
				_log = "Search for Parallel loop";
				return;
			}

#endregion

#region Check for open circuits
			
			// "walang wire na walang connection (open circuit)"
			_circuitMgr.checkForOpenNodes = true;
			
			if(source.HasOpenNode)
			{
				_log = "Count the Sources";
				return;
			}
			
			bool hasOpenComponents = false;
			
			foreach(var epi in _circuitMgr.ElectronPathInstances)
			{
				foreach(var loop in loops)
				{
					hasOpenComponents = !loop.elements.Exists(element => element == epi);
					
					if(hasOpenComponents)
						break;
				}
			}
			
			if(hasOpenComponents)
				return;
			
#endregion

			FinishLevel();
		}
	}
}