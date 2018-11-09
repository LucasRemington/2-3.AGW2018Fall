using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Dialogue dialogue;
    public Transform respawnLoc;
    private GameObject player;

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
    }
    public void TerminalDialogue()
    {
        // We locally create a position where the player is now. This allows us to move the respawn location, the player's parent object,
        // without moving the player's position as well. Then, start dialogue.
        var tempPosition = player.transform.position;
        respawnLoc.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, respawnLoc.position.z);
        player.transform.position = tempPosition;
        FindObjectOfType<DiaManager>().StartDialogue(dialogue);
    }
}
