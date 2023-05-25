using UnityEngine;
using System;

namespace FixerUpper.CustomSteps.Speaker
{
	public class RemoveScrews : Step
	{
		[Space]
		[SerializeField] private Screw[] _screws;
		[SerializeField] private GameObject _screwPlaceholder;
		
		protected override void Start()
		{
			base.Start();
			_screwPlaceholder.SetActive(false);
		}
		
		[ContextMenu("Execute")]
		public override void Execute()
		{
			if(IsDone) return;
			
			if(!_screwPlaceholder.activeSelf)
				_screwPlaceholder.SetActive(true);
			
			bool hasFixedScrews = Array.Exists(_screws, screw => screw.IsFixed);
			
			if(!hasFixedScrews)
				base.Execute();
		}
	}
}