using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    // This needs: respawnLoc, the list of player objs; the transform you'd like the player to appear at; "empty" game objects that contain:
    // A - the area the player is coming FROM; B - the area the player is going TO;
    // And it also needs the cinemachine camera.
    public GameObject worldA;
    public GameObject worldB;
    public Transform teleportHere;
    public GameObject respawnList;
    public GameObject cineCamera;

    // This script grabs the player from the respawn list.
    private GameObject player;

	void Update ()
    {
        if (player == null || player.Equals(null))
        {
            var childCount = respawnList.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = respawnList.transform.GetChild(i);
                var childTag = child.tag;

                if (childTag == "Player")
                    player = child.gameObject;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            worldB.SetActive(true);
            respawnList.transform.position = teleportHere.position;
            cineCamera.SetActive(false);
            cineCamera.transform.position = new Vector3(teleportHere.transform.position.x, teleportHere.transform.position.y, (teleportHere.transform.position.z - 1));
            player.transform.position = teleportHere.position;
            cineCamera.SetActive(true);
            worldA.SetActive(false);
        }
    }
}
