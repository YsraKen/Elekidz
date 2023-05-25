using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
	
	public ObjectPooler coinPool;
	
	public ObjectPooler[] theCoinPools;
	
	public float distanceBetweencoins;
	
	private int coinSelector;
	
   public void SpawnCoins (Vector3 startPosition ) 
   {
	   
	   coinSelector = Random.Range (0, theCoinPools.Length);
	   
	   GameObject coin1 = theCoinPools[coinSelector].GetPooledObject();
	   coin1.transform.position = startPosition;
	   coin1.SetActive(true);   //for every coin
	   
	   
	  /* GameObject coin2 = theCoinPools[coinSelector].GetPooledObject();
	   coin2.transform.position = new Vector3 (startPosition.x - distanceBetweencoins, startPosition.y, startPosition.z ); 
	   coin2.SetActive(true);  */
   }
} 
