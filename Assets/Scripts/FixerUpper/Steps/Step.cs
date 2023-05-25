using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FixerUpper
{
	public class Step : MonoBehaviour
	{
		[field: SerializeField, TextArea] public string Description { get; private set; }
		[field: SerializeField] public bool IsOptional { get; private set; }
		
		[Space]
		[SerializeField] protected Appliance _appliance;
		[SerializeField, FormerlySerializedAs("_onExecute")] private UnityEvent _onFinish;
		
		[Space]
		[SerializeField] private GameObject[] _hints;
		protected int _hintIndex;
		
		[SerializeField] private bool _showHintOnStart;
		
		public bool IsDone { get; private set; }
		
		protected virtual void Start()
		{
			if(_showHintOnStart)
				ShowHint();
		}
		
		private void OnValidate()
		{
			if(!_appliance)
				_appliance = GetComponentInParent<Appliance>();
		}
		
		public virtual void Execute()
		{
			if(IsDone) return;
			
			IsDone = true;
			
			_onFinish.Invoke();
			_appliance.UpdateProgress();
			
			Debug.Log($"Step completed: '<b>{Description}</b>'", this);
			GameManager.Instance.OnStepFinished(this);
			
			gameObject.SetActive(false);
			
			GameManager.Instance.HideHint();
		}
		
		public virtual void ShowHint()
		{
			var hint = default(GameObject);
			int count = _hints.Length;
			
			bool hasNextHint = false;
			
			if(count > 0)
			{
				hint = _hints[_hintIndex ++ % count];
				hint.SetActive(true);
				
				hasNextHint = _hintIndex < count;
			}
			
			Hint.CurrentHint = hint;
			Hint.hasNextHint = hasNextHint;
		}
	}
}