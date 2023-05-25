using UnityEngine;
using UnityEngine.Events;

namespace FixerUpper
{
	public class Interactable : MonoBehaviour
	{
		[field: SerializeField] public Step targetStep { get; set; }
		[SerializeField] private UnityEvent _onInteraction;
		
		public virtual void Interact()
		{
			_onInteraction.Invoke();
			targetStep?.Execute();
		}
	}
}