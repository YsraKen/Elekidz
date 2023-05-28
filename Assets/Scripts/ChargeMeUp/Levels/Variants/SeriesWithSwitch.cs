using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp
{
	using Experimental.Electronics;
	
	public class SeriesWithSwitch : Level
	{
		[SerializeField] private string _switchId = "defaultSwitch";
		
		[Space]
		[SerializeField] private string _tWireId = "tWire";
		
		[SerializeField] private string _bulbId = "defaultLoad";
		[SerializeField] private int _targetBulbsCount = 2;
		
		private CircuitManager _circuitMgr;
		
		protected override void Start()
		{
			base.Start();
			
			_circuitMgr = CircuitManager.Instance;
			_circuitMgr.onTickFinished += CheckProgress;
		}
		
		private void CheckProgress()
		{
#region Search for T wires
			
			bool hasTWire = default;
			
			foreach(var instance in _circuitMgr.ElectronPathInstances)
			{
				var component = instance as Component;
				if(!component) continue;
				
				hasTWire = component.ComponentId == _tWireId;
				
				if(hasTWire) break;
			}
			
			if(hasTWire)
			{
				_log = "Search for T wires";
				return;
			}
			
#endregion

#region Count the Sources

			if(_circuitMgr.SourceInstances.Count != 1)
			{
				_log = "Count the Sources";
				return;
			}
			
#endregion

#region Check circuit/loop/path count

			var source = _circuitMgr.SourceInstances[0];
			
			if(source.PathInfos.Count != 1)
			{
				_log = "Search for T wires";
				return;
			}

#endregion

#region Search for Bulbs

			var bulbs = new List<IElectronPath>();
			
			foreach(var instance in _circuitMgr.ElectronPathInstances)
			{
				var component = instance as Component;
				if(!component) continue;
				
				if(component.ComponentId == _bulbId)
					bulbs.Add(instance);
			}
			
			if(bulbs.Count != _targetBulbsCount)
			{
				_log = "Search for Bulbs";
				return;
			}
			
#endregion

#region Search for Switch
			
			var path = source.PathInfos[0];
			bool hasSwitch = default;
			
			foreach(var element in path.elements)
			{
				var component = element as Component;
				if(!component) continue;
				
				hasSwitch = component.ComponentId == _switchId;
				
				if(hasSwitch)
					break;
			}
			
			if(!hasSwitch)
			{
				_log = "Search for Switch";
				return;
			}
			
#endregion

#region Identify if the circuit is series

			bool isSeries = default;
			
			foreach(var bulb in bulbs)
			{
				isSeries = path.elements.Contains(bulb);
				
				if(!isSeries) break;
			}
			
			if(!isSeries)
			{
				_log = "Identify if the circuit is series";
				return;
			}

#endregion

			FinishLevel();
		}
	}
}