using UnityEngine;

namespace ChargeMeUp
{
	public class Level : MonoBehaviour
	{
		[field: SerializeField, TextArea(3, 20)] public string Notes { get; private set; }
		
		[Space]
		[SerializeField, TextArea(3, 100)] private string _hint;
		[SerializeField] private Sprite[] _hintAnimation;
		[SerializeField] private AudioClip _hintClip;
		
		[Space]
		[SerializeField] private ItemInfo[] _items;
		
		[Space]
		[SerializeField, TextArea] protected string _log;
		public string Log => _log;
		
		protected virtual void Start()
		{
			var gameMgr = GameManager.Instance;
				gameMgr.SetHint(_hint, _hintAnimation, _hintClip);
				gameMgr.itemInfos = _items;
		}
		
		protected virtual void FinishLevel() => GameManager.Instance.GameOver();
	}
}