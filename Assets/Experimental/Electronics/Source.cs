using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Source : MonoBehaviour, ITickUpdate
	{
		public float voltage = 5;
		
		public Node node;
		
		public List<PathInfo> pathInfos = new List<PathInfo>();
		public bool hasOpenCircuit { get; private set; }
		
		public Gradient gradient;
		
		CircuitManager _circuitMgr;
		
		void OnEnable() => CircuitManager.Instance?.AddInstance(this);
		void OnDisable() => CircuitManager.Instance?.RemoveInstance(this);
		
		void Start() => _circuitMgr = CircuitManager.Instance;
		
		void OnDrawGizmosSelected()
		{
			int count = pathInfos.Count;
			float maxIndexF = (float) count - 1f;
			
			for(int i = 0; i < count; i++)
			{
				var color = gradient.Evaluate((float) i / maxIndexF);
				pathInfos[i].DrawGizmos(color);
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
				pathInfos.Add(newInfo);
			
			return newInfo;
		}
		
		void HandleCicuitLoop()
		{
			pathInfos.Clear();
			
			var newPathInfo = CreateNewPath();
			
			node.Ping(Node.PingType.Outgoing, this, newPathInfo);
			
			hasOpenCircuit = pathInfos.Exists(info => !info.isClosedCircuit);
			pathInfos.RemoveAll(info => !info.isClosedCircuit);
		}
		
		void HandleElectronicValues()
		{
			foreach(var info in pathInfos)
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