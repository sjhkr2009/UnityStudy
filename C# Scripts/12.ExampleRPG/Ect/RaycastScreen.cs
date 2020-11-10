using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScreen : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, ~(1 << 8)))
			{
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name} (Tag: {hit.collider.gameObject.tag})");
			}
		}
    }
}
