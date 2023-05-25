using UnityEngine;

public class MonoBehaviour : UnityEngine.MonoBehaviour
{
	private Transform _t;
	
	public Transform _transform => _t ??= transform;
}