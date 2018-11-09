using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinePlayerGrab : MonoBehaviour
{
    public GameObject player;
    public GameObject playerList;
    private GameObject respawnLoc;

    void Start()
    {
        // Loop through to find where the player respawn is.
        var childCount = playerList.transform.childCount;
        for (var i = 0; i < childCount; ++i)
        {
            var child = playerList.transform.GetChild(i);
            var childTag = child.tag;

            if (childTag == "Respawn")
                respawnLoc = child.gameObject;
        }
    }
    void Update ()
    {
        //We call the Cinemachine virtual camera here for later.
        var cam = gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>();

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
        else
        {
            //Here's the later part! Just reset the follow and lookAt functions when player respawns essentially.
            cam.Follow = player.transform;
            cam.LookAt = player.transform;
        }

	}
}
