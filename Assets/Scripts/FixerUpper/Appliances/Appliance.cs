using UnityEngine;

namespace FixerUpper
{
	public class Appliance : MonoBehaviour
	{
		[field: SerializeField, TextArea] public string Description { get; private set; }
		
		[SerializeField] private Step[] _steps;
		[field: SerializeField, Range(0, 1)] public float Progress { get; private set; }
		
		public void UpdateProgress()
		{
			bool allStepsCompleted = true;
			int completionCount = 0;
			
			foreach(var step in _steps)
			{
				if(step.IsDone)
					completionCount ++;
				
				else
					allStepsCompleted = false;
			}
			
			Progress = (float) completionCount / (float) (_steps.Length - 1);
			
			if(allStepsCompleted)
				GameManager.Instance.GameOver(true);
		}
		
		public Step[] GetSteps()
		{
			int count = _steps.Length;
			var steps = new Step[count];
			
			for(int i = 0; i < count; i++)
				steps[i] = _steps[i];
			
			return steps;
		}
	}
}