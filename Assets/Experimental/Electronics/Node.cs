using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Node : MonoBehaviour, IElectronPath
	{
		public bool IsEnergized { get; private set; }
		public bool IsOpenEnded { get; private set; }
		
		[SerializeField] GameObject _energizedGpx;
		
		[field: SerializeField] public Load myComponent { get; private set; }
		
		static List<Node> instances = new List<Node>();
		static float detectRadius = 0.15f;
		
		void OnEnable() => instances.Add(this);
		void OnDisable() => instances.Remove(this);
		
		void OnValidate() => myComponent ??= GetComponentInParent<Load>();
		void OnDrawGizmos() => Gizmos.DrawSphere(transform.position, detectRadius);
		
		public enum PingType { Ingoing, Outgoing }
		
		public void Ping(PingType type, Source source, PathInfo pathInfo)
		{
			pathInfo.Add(this);
			
			// Termination (failed)	
			/* if(pathInfo.Contains(this))
				return;
			
			else pathInfo.Add(this); */
			
			// Termination (success)
			IsEnergized = true;
			
			if(this is Ground)
			{
				pathInfo.isClosedCircuit = true;
				return;
			}
			
			// Continue iteration (terminate if no next available target)
			switch(type)
			{
				case PingType.Ingoing:
					myComponent.Ping(this, source, pathInfo);
					break;
				
				case PingType.Outgoing:
					
					var connected = GetConnectedNode();
						connected?.Ping(PingType.Ingoing, source, pathInfo);
					
					IsOpenEnded = !connected;
					
					break;
			}
		}
		
		Node GetConnectedNode(bool isTwoD = true)
		{
			Node connected = null;
			
			float detectRadiusSqr = Mathf.Pow(detectRadius, 2);
			var myPosition = GetModifiedPosition(transform, isTwoD);
			
			foreach(var other in instances)
			{
				if(other == this) continue;
				if(IsSibling(other)) continue;
				
				var otherPosition = GetModifiedPosition(other.transform, isTwoD);
				var dstSqr = (otherPosition - myPosition).sqrMagnitude;
				
				if(dstSqr <= detectRadiusSqr)
				{
					connected = other;
					break;
				}
			}
			
			return connected;
		}
		
		Vector3 GetModifiedPosition(Transform transform, bool isTwoD)
		{
			var position = transform.position;
			
			if(isTwoD)
				position.z = 0f;
			
			return position;
		}
		
		bool IsSibling(Node other) => Array.Exists(myComponent.nodes, node => node == other);
		
		public void Activate(Source source){}
		
		public void OnTick()
		{
			if(_energizedGpx)
				_energizedGpx.SetActive(IsEnergized);
		}
		
		public void OnReset()
		{
			IsEnergized = false;
			IsOpenEnded = false;
		}
	}
}