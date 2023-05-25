using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
	public float range = 5f;
	public Transform target;
	Vector3 targetPosition;
	public float speed = 10f;
	public float positionChangeDuration = 1.5f;
	
	IEnumerator Start () 
	{
		while (true) 
		{
			
			targetPosition = transform.position + (Random.insideUnitSphere * range);
			yield return new WaitForSeconds(positionChangeDuration * Random.value);
			
		}
	}
	
	void Update() => target.position = Vector3.Lerp(target.position, targetPosition, speed * Time.deltaTime);
	void OnDrawGizmos() =>  Gizmos.DrawWireSphere(transform.position,range);
}
