using UnityEngine;
using System.Collections;

public class LaserEffect : MonoBehaviour {
	
	LineRenderer lr;
	
	void Start ()
	{
		lr = GetComponent<LineRenderer>();
	}
	
	void Update ()
	{
		RaycastHit hit;
			
		if(Physics.Raycast(transform.position,transform.forward,out hit,Mathf.Infinity))
		{
			lr.SetPosition(1,new Vector3(0,0,hit.distance));
		}
			
		else
		{
			lr.SetPosition(1,new Vector3(0,0,300));
		}
	}
}
