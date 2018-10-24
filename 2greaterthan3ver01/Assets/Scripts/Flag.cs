using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool status;
    public GameObject objectOne, objectTwo, objectThree;
	
	// This generalized script can accomodate three different objects to be affected. 
    // Because the status is made public, other scripts can reference the status variable
    // for their own personalized scripts as need be. 
	void Update ()
    {
        // If status is true, the status of all affected objects should they be assigned is now true.
        if (status)
        {
            if (objectOne != null)
                objectOne.GetComponent<Flag>().status = true;

            if (objectTwo != null)
                objectOne.GetComponent<Flag>().status = true;

            if (objectOne != null)
                objectOne.GetComponent<Flag>().status = true;
        }

        else // Likewise as long affected objects are assigned, they are set to false.
        {
            if (objectOne != null)
                objectOne.GetComponent<Flag>().status = false;

            if (objectTwo != null)
                objectOne.GetComponent<Flag>().status = false;

            if (objectOne != null)
                objectOne.GetComponent<Flag>().status = false;
        }
	}
}
