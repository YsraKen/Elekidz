using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	[System.Serializable]
	public class PathInfo
	{
		public List<IElectronPath> elements = new List<IElectronPath>();
		public List<Transform> elementsT = new List<Transform>();
		
		public bool isClosedCircuit;
		
		// Constructor
		public PathInfo(List<IElectronPath> elements = null)
		{
			if(elements != null)
				foreach(var element in elements)
					Add(element);
		}
		
		public bool Contains(IElectronPath element) => elements.Contains(element);
		
		public void Add(IElectronPath element)
		{
			elements.Add(element);
			
			if(element is Load)
				elementsT.Add(element.transform);
		}
		
		public void DrawGizmos(Color color)
		{
			if(elementsT == null || elementsT.Count == 0)
				return;
			
			var previous = elementsT[0].position;
			Gizmos.color = color;
			
			for(int i = 1; i < elementsT.Count; i++)
			{
				var current = elementsT[i].position;
				
				Gizmos.DrawLine(previous, current);
				
				previous = current;
			}
		}
		
		#region Info
		
		public float GetTotalResistance()
		{
			float total = 0f;
			
			elements.ForEach(element =>
			{
				if(element is Load)
					total += (element as Load).resistance;
			});
			
			return total;
		}
		
		public Load GetWeakestLoad()
		{
			float lowestAmpereRating = float.MaxValue;
			var weakest = default(Load);
			
			foreach(var element in elements)
			{
				var load = element as Load;
				if(!load) continue;
				
				float value = load.MaxAmpereRating;
				
				if(value < lowestAmpereRating)
				{
					lowestAmpereRating = value;
					weakest = load;
				}
			}
			
			return weakest;
		}
		
		#endregion
		
		#region Shortcuts
		
		public bool HasOpenNode()
		{
			bool hasOpenNode = default;
			
			foreach(var element in elements)
			{
				var node = element as Node;
				
				if(node)
					hasOpenNode = node.IsOpenEnded;
			}
			
			return hasOpenNode;
		}
		
		public void SetAsActiveCircuit()
		{
			foreach(var element in elements)
				(element as Load)?.SetAsActiveCircuit();
		}
		
		public void UpdateElectronicValues(float totalResistance, float voltage, float amperes)
		{
			foreach(var element in elements)
				(element as Load)?.UpdateValues(totalResistance, voltage, amperes);
		}
		
		public void DamageComponents()
		{
			foreach(var element in elements)
				(element as Load)?.Damage();
		}
		
		#endregion
	}
}