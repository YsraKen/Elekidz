using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// namespace StudyTime.MSC
// {
public class AnimateText : MonoBehaviour
{
	[SerializeField] bool _asToggleOnly = true;
	
	[SerializeField] Text _txt;
	[SerializeField] TextMeshProUGUI _tmp;
	[SerializeField] float _duration = 1.5f;
	[SerializeField] bool _waitForFrameOnStart = true;
	
	void OnEnable()
	{
		StartCoroutine(r());
		IEnumerator r()
		{
			if(_waitForFrameOnStart)
				yield return null;
			
			string value = _txt? _txt.text: _tmp? _tmp.text: default;
			SetValue("");
			
			if(_asToggleOnly)
				SetValue(value);
				
			else
				// yield return Tools.AnimateText(value, SetValue, _duration);
				yield return AnimateTextRoutine(value, SetValue, _duration);
		}
		
		void SetValue(string value)
		{
			if(_txt) _txt.text = value;
			if(_tmp) _tmp.text = value;
		}
	}
	
	// public static IEnumerator AnimateText(string text, Action<string> action, float duration = 1.5f)
	IEnumerator AnimateTextRoutine(string text, Action<string> action, float duration = 1.5f)
	{
		if(text == null) yield break;
	
		string current = "";
		int count = text.Length;
		
		bool skipped = false;
		
		float seconds = Mathf.Clamp(duration / count, 0, float.MaxValue);
		
		// var step = WaitForSecSkippable(seconds, onSkip: () => skipped = true);
		var step = new WaitForSeconds(seconds);
		
		for(int i = 0; i < count; i++)
		{
			if(skipped)
			{
				int remaining = count - i + 1;
				
				for(int j = i; j < remaining; j++)
					current += text[j];
				
				action(current);
				break;
			}
			else current += text[i];
			
			yield return step;
			action(current);
		}
	}
	
	#if UNITY_EDITOR
	// public InspectorButtons btt;
	
	// [ContextMenu("Get Components"), DrawButton]
	[ContextMenu("Get Components")]
	public void GetReferences()
	{
		_txt ??= GetComponent<Text>();
		_tmp ??= GetComponent<TextMeshProUGUI>();
		
		UnityEditor.EditorUtility.SetDirty(this);
	}
	
	void OnValidate() => GetReferences();
	#endif
	}
// }