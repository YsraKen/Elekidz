using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChargeMeUp
{
	[System.Serializable]
	public class ItemInfo // : ScritpableObject
	{
		public string name;
		public Sprite icon;
		public Item prefab;
		public int maxCount = 1;
		
		public List<Item> Instances { get; private set; } = new List<Item>();
		
		private ItemContainer _itemContainerUI;
		
		public void InitializeUI(ItemContainer container)
		{
			_itemContainerUI ??= container;
			
			_itemContainerUI.SetupUI(icon, name);
			UpdateRemainingCount();
		}
		
		public Item InstantiatePrefab(Transform parent)
		{
			Item instance = null;
			
			if(Instances.Count < maxCount)
			{
				instance = Object.Instantiate(prefab, parent);
				Instances.Add(instance);
			}
			
			else
			{
				// string warningMessage = $"<color=yellow>Can't spawn more <b>'{name} ({prefab.name})'</b>. Maximum count of instances has been reached</color>";
				string warningMessage = $"<color=yellow>Can't spawn more <b>'{name}'</b>. Maximum count of instances has been reached</color>";
				
				GameManager.Instance.ShowMessage(warningMessage, GameManager.MessageType.Warning);
				Debug.Log(warningMessage);
			}
			
			UpdateRemainingCount();
			
			return instance;
		}
		
		public void RecheckInstances()
		{
			for(int i = Instances.Count - 1; i > -1; i--)
			{
				if(!Instances[i])
					Instances.RemoveAt(i);
			}
			
			UpdateRemainingCount();
		}
		
		public void UpdateRemainingCount()
		{
			int remaining = maxCount - Instances.Count;
			_itemContainerUI.SetCount(remaining);
		}
	}
}