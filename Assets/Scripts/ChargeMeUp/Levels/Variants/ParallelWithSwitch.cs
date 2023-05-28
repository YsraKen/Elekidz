using UnityEngine;

namespace ChargeMeUp
{
	using Experimental.Electronics;
	
	public class ParallelWithSwitch : Level
	{
		[SerializeField] private string _tWireId = "tWire";
		[SerializeField] private int _numberOfTWires = 2;
		
		[Space]
		[SerializeField] private int _numberOfLoops = 2;
		[SerializeField] private string _bulbId = "defaultLoad";
		
		[Space]
		[SerializeField] private string _switchId = "defaultSwitch";
		
		private CircuitManager _circuitMgr;
		
		protected override void Start()
		{
			base.Start();
			
			_circuitMgr = CircuitManager.Instance;
			_circuitMgr.onTickFinished += CheckProgress;
			
			_circuitMgr.checkForOpenNodes = true;
		}
		
		private void CheckProgress()
		{
#region Check the Sources

			if(_circuitMgr.SourceInstances.Count != 1)
			{
				_log = "Check the Sources";
				return;
			}
			
			var source = _circuitMgr.SourceInstances[0];
			
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
			
			if(totalBulbCount != _numberOfLoops)
			{
				_log = "Search for Bulbs";
				return;
			}
			
#endregion

#region Search for Switch

			var loops = source.PathInfos;
			bool isSwitchSeriesToAll = true;
			
			foreach(var loop in loops)
			{
				bool hasSwitch = default;
				
				foreach(var element in loop.elements)
				{
					var component = element as Component;
					if(!component) continue;
					
					hasSwitch = component.ComponentId == _switchId;
					
					if(hasSwitch)
						break;
				}
				
				if(!hasSwitch)
				{
					isSwitchSeriesToAll = false;
					break;
				}
			}
			
			if(!isSwitchSeriesToAll)
			{
				_log = "Search for Switch";
				return;
			}

#endregion

#region Identify if the circuit is parallel
			
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
			
			if(!isParallel)
			{
				_log = "Identify if the circuit is parallel";
				return;
			}

#endregion

#region Check for open circuits
			
			// "walang wire na walang connection (open circuit)"
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