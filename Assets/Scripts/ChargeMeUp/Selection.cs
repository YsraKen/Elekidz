using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ChargeMeUp
{
	public interface ISelectable
	{
		public Vector3 position { get; set; }
		public void OnSelectionUpdateChanged(bool isSelected);
	}

	public class Selection : MonoBehaviour
	{
		[SerializeField] private RectTransform _highlightRect;
		[SerializeField] private UnityEvent _onSelectionUpdateChanged;
		
		public List<ISelectable> Objects { get; private set; }
		
		public static Selection Instance { get; private set; }
		
		private Camera _cam;
		
		private void Awake()
		{
			Instance = this;
			Objects = new List<ISelectable>();
			
			_cam = Camera.main;
		}
		
		public void StartMultiSelectionMode()
		{
			Debug.Log("Multi-selection mode started", this);
			
			StartCoroutine(r());
			IEnumerator r()
			{
				// initialize lists
				DeselectAll();
				
				var allSelectables = FindObjectsOfType<MonoBehaviour>().OfType<ISelectable>().ToArray();
				yield return null;
				
				// initialize bounds/rects
				_highlightRect.gameObject.SetActive(true);
				
				var initialPosition = Input.mousePosition;
				var bounds = new Bounds(initialPosition, Vector3.zero);
				
				while(Input.GetMouseButton(0))
				{
					// update bounds size and position
					var displacement = Input.mousePosition - initialPosition;
					
					bounds.size = new Vector2
					(
						Mathf.Abs(displacement.x),
						Mathf.Abs(displacement.y)
					);
					
					var position = initialPosition + (displacement / 2f);
						position.z = 0f;
					
					bounds.center = position;
					
					// update selections and ui
					GetSelectionFromBounds(allSelectables, bounds);
					UpdateHighlightRect(bounds);
					
					yield return null;
				}
				
				_highlightRect.gameObject.SetActive(false);
			}
		}
		
		private void GetSelectionFromBounds(ISelectable[] allSelectables, Bounds bounds)
		{
			foreach(var selectable in allSelectables)
			{
				if(Objects.Contains(selectable))
					continue;
				
				var position = _cam.WorldToScreenPoint(selectable.position);
					position.z = 0f;
				
				if(bounds.Contains(position))
					Select(selectable, isMultiSelection: true, invokeCallbackEvents: false);
			}
			
			// Callback Events
			foreach(var obj in Objects)
				obj.OnSelectionUpdateChanged(true);
			
			_onSelectionUpdateChanged.Invoke();
		}
		
		private void UpdateHighlightRect(Bounds bounds)
		{
			_highlightRect.sizeDelta = (Vector2) bounds.size;
			_highlightRect.anchoredPosition = bounds.center;
		}
		
		public void Select(ISelectable newObject, bool isMultiSelection = false, bool invokeCallbackEvents = true)
		{
			// Handle deselections first
			if(!isMultiSelection)
				DeselectAll();
			
			// Hendle selections
			if(!Objects.Contains(newObject))
				Objects.Add(newObject);
			
			// Callback Events
			if(invokeCallbackEvents)
			{
				foreach(var obj in Objects)
				obj.OnSelectionUpdateChanged(true);
			
				_onSelectionUpdateChanged.Invoke();
			}
		}
		
		public void Deselect(ISelectable current)
		{
			if(!Objects.Contains(current))
				return;
			
			Objects.Remove(current);
			
			// Callback Events
			current.OnSelectionUpdateChanged(false);
			_onSelectionUpdateChanged.Invoke();
		}
		
		public void DeselectAll()
		{
			foreach(var obj in Objects)
					obj.OnSelectionUpdateChanged(false);
			
			Objects.Clear();
			_onSelectionUpdateChanged.Invoke();
		}
	}
}