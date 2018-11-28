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

    // Make sure we have the player, because if they're holding the box, we don't wanna respawn it.
    public GameObject respawnLoc;
    private GameObject player;

    // Make sure that there is an attached flag script as well, but not linked to anything.


    void Update()
    {
        // If the player is not currently set, find and set it.
        if (player == null || player.Equals(null))
        {
            var childCount = respawnLoc.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = respawnLoc.transform.GetChild(i);
                var childTag = child.tag;

                if (childTag == "Player")
                    player = child.gameObject;
            }
        }

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


            if (gameObject.GetComponent<Flag>().status && !justSpawned && player.GetComponentInChildren<GrabThrow>().occupied == false)
            {
                Destroy(currentObj);
                Instantiate(prefab, gameObject.transform);
                justSpawned = true;
            }

            if (gameObject.GetComponent<Flag>().status == false)
                justSpawned = false;
        }
    }
