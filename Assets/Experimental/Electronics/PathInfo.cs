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
			
			LoopCertainElementsOfType<Load>(element =>
			{
				float value = element.maxAmpereRating;
				
				if(value < lowestAmpereRating)
				{
					lowestAmpereRating = value;
					weakest = element;
				}
			});
			
			return weakest;
		}
		
		#endregion
		
		#region Shortcuts
		
		public void SetAsActiveCircuit()
			=> LoopCertainElementsOfType<Load>(e => e.SetAsActiveCircuit());
		
		public void UpdateElectronicValues(float totalResistance, float voltage, float amperes)
			=> LoopCertainElementsOfType<Load>(e => e.UpdateValues(totalResistance, voltage, amperes));
		
		private void LoopCertainElementsOfType<T>(System.Action<T> onIterate) where T : Object, IElectronPath
		{
			foreach(var element in elements)
			{
				T t = element as T;
				
				if(t) onIterate(t);
			}
		}
		
		#endregion
	}
}