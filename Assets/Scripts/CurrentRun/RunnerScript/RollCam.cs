using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCam : MonoBehaviour
{
    public float rollSpeed;
	//public GameObject PlatFormDestructionPoint;
	public GameObject PlatFormGenerationPoint;

	
    // Start is called before the first frame update
    void Start()
    {
		//PlatFormDestructionPoint.gameObject.SetActive(false);
		PlatFormGenerationPoint.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update() 
	{

		transform.position = new Vector3(transform.position.x + rollSpeed, transform.position.y , transform.position.z);
    }
}
