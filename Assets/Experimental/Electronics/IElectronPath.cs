using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	// conductor
	public interface IElectronPath : ITickUpdate
	{
		public Transform transform { get; }
		
		public void OnReset();
	}
	
	public interface ITickUpdate
	{
		public void OnTick();
	}
}