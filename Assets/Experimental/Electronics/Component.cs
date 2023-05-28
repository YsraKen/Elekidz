using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Component : MonoBehaviour
	{
		[field: SerializeField] public string ComponentId { get; private set; }
	}
}