using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class CircuitManager : MonoBehaviour
	{
		[field: SerializeField] public bool SimpleMode { get; private set; }
		
		public Gradient indicatorGradient;
		
		List<IElectronPath> electronPathInstances = new List<IElectronPath>();
		List<Source> sourceInstances = new List<Source>();
		
		static CircuitManager instance;
		public static CircuitManager Instance
		{
			get
			{
				if(!instance)
					instance = FindObjectOfType<CircuitManager>();
				
				return instance;
			}
		}
		
		IEnumerator Start()
		{
			var step = new WaitForSeconds(1f / 5f);
			
			while(true)
			{
				yield return step;
				
				yield return ResetElectronPaths();
				yield return TickSources();
				yield return TickLoads();		
			}
		}
		
		IEnumerator ResetElectronPaths()
		{
			foreach(var ep in electronPathInstances)
				ep?.OnReset();
			
			yield return null;
		}
		
		IEnumerator TickSources()
		{
			foreach(var s in sourceInstances)
				s?.OnTick();
			
			yield return null;
		}
		
		IEnumerator TickLoads()
		{
			foreach(var ep in electronPathInstances)
			{
				if(ep == null) continue;
				
				var component = ep as Load;
					component?.OnTick();
			}
			
			yield return null;
		}
		
		public void AddInstance(IElectronPath instance)
		{
			if(!electronPathInstances.Contains(instance))
				electronPathInstances.Add(instance);
		}
		
		public void RemoveInstance(IElectronPath instance)
		{
			if(electronPathInstances.Contains(instance))
				electronPathInstances.Remove(instance);
		}
		
		public void AddInstance(Source instance)
		{
			if(!sourceInstances.Contains(instance))
				sourceInstances.Add(instance);
		}
		
		public void RemoveInstance(Source instance)
		{
			if(sourceInstances.Contains(instance))
				sourceInstances.Remove(instance);
		}
	}
}