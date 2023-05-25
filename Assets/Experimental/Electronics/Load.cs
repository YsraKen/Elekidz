using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ChargeMeUp.Experimental.Electronics
{
	public class Load : MonoBehaviour, IElectronPath
	{
		public float resistance;
		public float maxAmpereRating = 1;
		
		[Range(0,1)] public float targetAmpereRatingNormalized = 0.5f;
		
		public Node[] nodes;
		
		bool isEnergized;
		
		public float Voltage { get; private set; }
		public float Amperes { get; private set; }
		
		public float AmperesNormalized { get; private set; }
		public bool IsBlown { get; private set; }
		
		#region Visuals
		[Space]
		public GameObject energized;
		public CanvasGroup active, overcurrent;
		public GameObject blown;
		#endregion
		
		#if UNITY_EDITOR
		static readonly Vector3 HANDLES_OFFSET = new Vector3(0, 0, 0.25f);
		#endif
		
		void OnEnable() => CircuitManager.Instance?.AddInstance(this);
		void OnDisable() => CircuitManager.Instance?.RemoveInstance(this);
		
		protected virtual void OnValidate()
		{
			if(nodes != null || nodes.Length > 0)
				for(int i = 0; i < nodes.Length; i++)
					nodes[i].name = $"{name} - terminal ({i})";
		}
		
		void OnDrawGizmos()
		{
			if(!Application.isPlaying)
				return;
			
			if(!isEnergized) return;
			if(Amperes <= 0f) return;
			
			float lerpValue = Mathf.Clamp01(AmperesNormalized);
			var color = CircuitManager.Instance.indicatorGradient.Evaluate(lerpValue);
			
			Gizmos.color = color;
			Gizmos.DrawSphere(transform.position, 0.15f);
			
			#if UNITY_EDITOR
			float amperesPercent = AmperesNormalized * 100;
			
			GUI.color = color;
			Handles.Label(transform.position + HANDLES_OFFSET, amperesPercent.ToString());
			#endif
		}
		
		public void Ping(Node sourceNode, Source source, PathInfo pathInfo)
		{
			if(resistance == Mathf.Infinity)
				return;
			
			bool isMyNode = Array.Exists(nodes, node => node == sourceNode);
			if(!isMyNode) return;
			
			if(pathInfo.Contains(this))
				return;
			
			else pathInfo.Add(this);
			
			isEnergized = true;
			
			foreach(var node in nodes)
			{
				if(node == sourceNode)
					continue;
				
				var infoElementsCopy = new List<IElectronPath>(pathInfo.elements);
					
					// new path elements must not contain any element from the "nodes" array exept for this current 'node' and the 'sourceNode'
					// infoElementsCopy.RemoveAll(element => element == (node as IElectronPath));
					foreach(var myNode in nodes)
						infoElementsCopy.Remove(myNode);
					
					infoElementsCopy.Add(sourceNode);
				
				pathInfo = source.CreateNewPath(infoElementsCopy);
				
				node.Ping(Node.PingType.Outgoing, source, pathInfo);
			}
		}
		
		// called when the circuit is in simpled mode
		public void SetAsActiveCircuit() => Amperes = targetAmpereRatingNormalized * maxAmpereRating;
		
		// called when the circuit is simulated
		public void UpdateValues(float totalResistance, float sourceVoltage, float amperes)
		{
			Voltage = (resistance / totalResistance) * sourceVoltage;
			Amperes = amperes;
		}
		
		public void SetAsBlown()
		{
			resistance = Mathf.Infinity;
			
			if(blown) blown.SetActive(true);
			IsBlown = true;
		}
		
		public void OnTick()
		{
			if(IsBlown) return;
			
			AmperesNormalized = Amperes / maxAmpereRating;
			
			if(AmperesNormalized > 1f)
			{
				SetAsBlown();
				return;
			}
			
			UpdateGpx();
		}
		
		public void OnReset()
		{
			isEnergized = false;
			
			Voltage = 0f;
			Amperes = 0f;
		}
		
		void UpdateGpx()
		{
			#region Toggle Objects
			if(energized) energized.SetActive(isEnergized);
			
			bool isStateActive = AmperesNormalized >= targetAmpereRatingNormalized;

			if(active) active.gameObject.SetActive(isStateActive);
			if(overcurrent) overcurrent.gameObject.SetActive(isStateActive);
			
			// if(blown) blown.SetActive(true);
			#endregion
			
			#region Lerp
			
			if(!active || !overcurrent) return;
			
			float activeLerp = Mathf.InverseLerp(0f, targetAmpereRatingNormalized, AmperesNormalized);
			float overcurrentLerp = Mathf.InverseLerp(targetAmpereRatingNormalized, maxAmpereRating, AmperesNormalized);
			
			active.alpha = Mathf.Clamp01(activeLerp);
			overcurrent.alpha = Mathf.Clamp01(overcurrentLerp);
			
			#endregion
		}
	}
}