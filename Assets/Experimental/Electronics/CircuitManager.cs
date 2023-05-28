using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class CircuitManager : MonoBehaviour
	{
		[field: SerializeField] public bool SimpleMode { get; private set; }
		
		public Gradient indicatorGradient;
		
		public List<IElectronPath> ElectronPathInstances { get; private set; } = new List<IElectronPath>();
		public List<Source> SourceInstances { get; private set; } = new List<Source>();
		
		public Action onTickFinished;
		
		public bool checkForOpenNodes { get; set; }
		
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
				
				onTickFinished?.Invoke();
			}
		}
		
		IEnumerator ResetElectronPaths()
		{
			foreach(var ep in ElectronPathInstances)
				ep?.OnReset();
			
			yield return null;
		}
		
		IEnumerator TickSources()
		{
			foreach(var s in SourceInstances)
			{
				if(!s) continue;
				
				s.checkForOpenNodes = this.checkForOpenNodes;
				s.OnTick();
			}
			
			yield return null;
		}
		
		IEnumerator TickLoads()
		{
			foreach(var ep in ElectronPathInstances)
			{
				if(ep == null) continue;
				
				var component = ep as Load;
					component?.OnTick();
			}
			
			yield return null;
		}
		
		public void AddInstance(IElectronPath instance)
		{
			if(!ElectronPathInstances.Contains(instance))
				ElectronPathInstances.Add(instance);
		}
		
		public void RemoveInstance(IElectronPath instance)
		{
			if(ElectronPathInstances.Contains(instance))
				ElectronPathInstances.Remove(instance);
		}
		
		public void AddInstance(Source instance)
		{
			if(!SourceInstances.Contains(instance))
				SourceInstances.Add(instance);
		}
		
		public void RemoveInstance(Source instance)
		{
			if(SourceInstances.Contains(instance))
				SourceInstances.Remove(instance);
		}
		
		#region ------------------TEST------------------
		
		// Temporary
		
		[Space]
		[SerializeField] GameObject explosionAnimation;
		[SerializeField] float explosionAnimationDuration = 1f;
		[SerializeField] GameObject explodeEffectTestA, explodeEffectTestB;
		[SerializeField] int explodeEffectTestFrameCount = 2;
		
		public void PlayExplosionAnimationAtPosition(Vector3 position)
		{
			StartCoroutine(r());
			IEnumerator r()
			{
				var screenPoint = Camera.main.WorldToScreenPoint(position);
					explosionAnimation.transform.position = screenPoint;
				
				explodeEffectTestA.SetActive(true);
				yield return null;
				
				explosionAnimation.SetActive(true);
				
				for(int i = 1; i < explodeEffectTestFrameCount; i++)
					yield return null;
				
				explodeEffectTestA.SetActive(false);
				explodeEffectTestB.SetActive(true);
				
				for(int i = 1; i < explodeEffectTestFrameCount; i++)
					yield return null;
				
				explodeEffectTestB.SetActive(false);
				
				yield return new WaitForSeconds(explosionAnimationDuration);
				explosionAnimation.SetActive(false);
			}
		}
		
		#endregion
	}
}