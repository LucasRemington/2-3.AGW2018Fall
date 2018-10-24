using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnTriggerStay (Collider other) {

        if (other.gameObject.CompareTag("Button"))
        {
                other.gameObject.GetComponent<Flag>().status = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Button"))
        {
                other.gameObject.GetComponent<Flag>().status = false;
        }
    }
}
