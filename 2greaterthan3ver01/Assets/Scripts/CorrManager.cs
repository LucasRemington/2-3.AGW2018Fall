using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrManager : MonoBehaviour
{

    // corrBase represents UI for being alive and corrupted. corrEnd is what is seen on game over.
    public Sprite corrBase, corrEnd;

    public Image corrUI;

    // Call the player from the player list so that if the player respawns, this doesn't break.
    public GameObject playerList;
    private GameObject player;

    private float corruption;
    private Color tempColor;

    private bool done;

    void Start ()
    {
        corrUI.sprite = corrBase;
        tempColor = corrUI.color;
	}
	

	void Update ()
    {

        // If the player is not currently set, find and set it.
        if (player == null)
        {
            var childCount = playerList.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = playerList.transform.GetChild(i);
                var childTag = child.tag;

                if (childTag == "Player")
                    player = child.gameObject;
            }
        }

        // Used for determining opacity of our corruption UI.
        corruption = (float)player.GetComponentInChildren<Corruption>().corrSlow;

        // As alpha can not be directly set, we instead set a tempColor, change its alpha, and set our UI color to that.
        // It's really dumb and it sounds redundant, I know.
        if (!done)
        {
            tempColor.a = (corruption / 100);
            corrUI.color = tempColor;
        }

        if (corruption >= 100)
        {
            corrUI.sprite = corrEnd;
            done = true;
            //temporary, just restart the scene upon death.
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
