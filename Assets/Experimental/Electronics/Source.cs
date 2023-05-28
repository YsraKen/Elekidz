using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Source : Component, ITickUpdate
	{
		public float voltage = 5;
		
		public Node node;
		
		public bool checkForOpenNodes { get; set; }
		public bool HasOpenNode { get; private set; }
		
		public List<PathInfo> PathInfos { get; private set; } = new List<PathInfo>();
		
		public Gradient gradient;
		
		CircuitManager _circuitMgr;
		
		void OnEnable() => CircuitManager.Instance?.AddInstance(this);
		void OnDisable() => CircuitManager.Instance?.RemoveInstance(this);
		
		void Start() => _circuitMgr = CircuitManager.Instance;
		
		void OnDrawGizmosSelected()
		{
			int count = PathInfos.Count;
			float maxIndexF = (float) count - 1f;
			
			for(int i = 0; i < count; i++)
			{
				var color = gradient.Evaluate((float) i / maxIndexF);
				PathInfos[i].DrawGizmos(color);
			}
		}
		
		public void OnTick()
		{
			HandleCicuitLoop();
			HandleElectronicValues();
		}
		
		public PathInfo CreateNewPath(List<IElectronPath> elements = null)
		{
			var newInfo = new PathInfo(elements);
				PathInfos.Add(newInfo);
			
			return newInfo;
		}
		
		void HandleCicuitLoop()
		{
			PathInfos.Clear();
			
			var newPathInfo = CreateNewPath();
			
			node.Ping(Node.PingType.Outgoing, this, newPathInfo);
			
			PathInfos.RemoveAll(info => !info.isClosedCircuit);
			HasOpenNode = PathInfos.Exists(info => info.HasOpenNode());
		}
		
		void HandleElectronicValues()
		{
			foreach(var info in PathInfos)
			{
				float totalResistance = info.GetTotalResistance();
				
				bool isShorted = totalResistance <= 0f;
				
				if(isShorted)
				{
					var weakestLoad = info.GetWeakestLoad();
						weakestLoad?.SetAsBlown();
					
					info.DamageComponents();
				}
				
				if(_circuitMgr.SimpleMode)
					info.SetAsActiveCircuit();
				
				else
				{
					float amperes = totalResistance > 0f?
						Mathf.Abs(voltage / totalResistance):
						Mathf.Infinity;
					
					info.UpdateElectronicValues(totalResistance, voltage, amperes);
				}
			}
		}
	}
}