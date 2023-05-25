using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class GameManager : MonoBehaviour
	{
		public int targetFrameRate = 5;
		
		void OnValidate()
		{
			if(Application.isPlaying)
				Application.targetFrameRate = targetFrameRate;
		}
	}
}