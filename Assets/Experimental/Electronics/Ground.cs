using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Ground : Node
	{
		public enum ConnectionType { AsCommonPoint, MustBeLooped }
		[field: SerializeField] public ConnectionType Type { get; private set; }
		
		/* [SerializeField] Node 
		
		public bool CheckConnection()
		{
			
			switch(Type)
			{
				case ConnectionType.AsCommonPoint: isConnected = true; break;
			}
		} */
	}
}