using UnityEngine;

namespace ChargeMeUp.Experimental.Electronics
{
	public class Switch : Load 
	{
		[field: SerializeField, Space] public bool IsClosed { get; private set; }
		
		[SerializeField] private GameObject _closedState, _openState;
		
		void Start() => UpdateState();
		
		[ContextMenu("Toggle")]
		public void Toggle()
		{
			if(IsBlown) return;
			
			IsClosed = !IsClosed;
			
			UpdateState();
		}
		
		protected override void OnValidate() => UpdateState();
		
		private void UpdateState()
		{
			if(IsClosed)
				resistance = 0f;
			
			else
				resistance = Mathf.Infinity;
			
			if(_closedState) _closedState.SetActive(IsClosed);
			if(_openState) _openState.SetActive(!IsClosed);
		}
	}
}