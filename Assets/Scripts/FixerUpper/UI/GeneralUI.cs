using UnityEngine;
using UnityEngine.UI;

namespace FixerUpper
{
	public class GeneralUI : MonoBehaviour
	{
		[SerializeField] private Image _miniProgress;
		
		public static GeneralUI Instance { get; private set; }
		private void Awake() => Instance = this;
		
		public void ShowMiniProgress(Vector3 position, float value)
		{
			_miniProgress.transform.position = position;
			_miniProgress.fillAmount = value;
			
			if(!_miniProgress.gameObject.activeSelf)
				_miniProgress.gameObject.SetActive(true);
		}
		
		public void HideMiniProgress() => _miniProgress.gameObject.SetActive(false);
	}
}