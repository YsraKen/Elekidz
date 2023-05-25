using UnityEngine;
using UnityEngine.UI;

namespace ChargeMeUp
{
	public class Highlight : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private Outline _outline;
		
		public void SetActive(bool isActive) => gameObject.SetActive(isActive);
		
		public void SetColor(Color color)
		{
			_image.color = color;
			_outline.effectColor = color;
		}
		
		public void ResetColor() => SetColor(Color.white);
	}
}