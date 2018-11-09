using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrManager : MonoBehaviour
{

    // corrBase represents UI for being alive and corrupted. corrEnd is what is seen on game over.
    public Sprite corrBase, corrEnd;
    public Animator corrAnim;

    public Image corrUI;

    // Call the player from the player list so that if the player respawns, this doesn't break.
    public GameObject playerList;
    [SerializeField] private GameObject player;
    private GameObject respawnLoc;

    //In fact, let's also call the prefab for the player so we CAN respawn them.
    public GameObject plPrefab;

    [SerializeField] private float corruption;
    private Color tempColor;

    private bool done;

    //The below are used in respawning the player.
    // We're going to track death counts, because why not? Also makes it amusing for the AI to confront the player about it later on.
    [HideInInspector] public int deathCount = 0;

    //Timers and vars used specifically for respawning.
    public float respTime = 3.0f;
    private bool respawning;
    private IEnumerator respawn;

    void Start ()
    {
        //corrUI.sprite = corrBase;
        tempColor = corrUI.color;
        respawn = RespPlayer();

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

        // If the player is not currently set, find and set it.
        if (player == null || player.Equals(null))
        {
            var childCount = respawnLoc.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = respawnLoc.transform.GetChild(i);
                var childTag = child.tag;
                Debug.Log("Looping through " + childCount + " times");

                if (childTag == "Player")
                    player = child.gameObject;                
            }
        }

        // Used for determining opacity of our corruption UI.
        if (player != null)
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
            //corrUI.sprite = corrEnd;
            corrAnim.SetBool("CorrFull", true);
            done = true;
            Destroy(player);
            if (!respawning)
            {
                StartCoroutine(respawn);
                respawning = true;
            }
        }
	}


    private IEnumerator RespPlayer()
    {
        yield return new WaitForSeconds(respTime);
        Instantiate(plPrefab, respawnLoc.transform);
        corruption = 0;
        corrAnim.SetBool("CorrFull", false);
        done = false;
        respawning = false;
    }

    /*void RespawnPl()
    {
        player.GetComponentInChildren<Corruption>().corrSlow = gameObject.GetComponent<Respawn>().Resp(plPrefab);
    }*/
}
