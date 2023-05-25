using UnityEngine;
using System.Collections;

namespace FixerUpper
{
	public class Tool : DraggableItem
	{
		public virtual void Use(bool autoStop = true)
		{
			Debug.Log($"Using '<b>{name}</b>'", this);
			
			if(_animation)
				_animation.Play("usingTool");
			
			if(autoStop)
				Invoke(nameof(StopUsing), 1.5f);
		}
		
		public virtual void StopUsing(bool isCanceled = false)
		{
			if(_animation)
				_animation.Play("pop");
			
			if(!isCanceled)
				GameManager.Instance.PlayParticle(transform.position);
		}
	}
}