using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    // Manually link the object you want to spawn in here in the inspector.
    public GameObject prefab;

    private GameObject currentObj;

    // This simple bool makes sure we don't constantly spawn in an object. 
    private bool justSpawned;
	
    // Make sure that there is an attached flag script as well, but not linked to anything.
	

	void Update ()
    {
        // As long as the prefab is initially attached, we find its tag, and use our standard loop
        // to find the prefab in the children once it's destroyed.
        var prefabTag = prefab.tag;
        if (currentObj == null || currentObj.Equals(null))
        {
            var childCount = gameObject.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = gameObject.transform.GetChild(i);
                var childTag = child.tag;

                if (childTag == prefabTag)
                    currentObj = child.gameObject;
            }
        }


        if (gameObject.GetComponent<Flag>().status && !justSpawned)
        {
            Destroy(currentObj);
            Instantiate(prefab, gameObject.transform);
            justSpawned = true;
        }

        if (gameObject.GetComponent<Flag>().status == false)
            justSpawned = false;
	}
}
