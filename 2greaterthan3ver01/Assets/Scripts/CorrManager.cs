using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrManager : MonoBehaviour
{

    // corrBase represents UI for being alive and corrupted. corrEnd is what is seen on game over.
    public Sprite corrBase, corrEnd;
    public Animator corrAnim;

    //Used in the UI animations.
    public Animator fastCorrAnim;
    public Animator slowCorrAnim;
    public bool corrupting;
    private int fastCorr;

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

        // Loop through to find where the player respawn is.
        var childCount = playerList.transform.childCount;
        for (var i = 0; i < childCount; ++i)
        {
            var child = playerList.transform.GetChild(i);
            var childTag = child.tag;

            if (childTag == "Respawn")
                respawnLoc = child.gameObject;
        }

        //fastCorrAnim.Play("uicorr1");
        slowCorrAnim.Play("slowCorr0");
    }
	

	void Update ()
    {
        // Hitting escape quits the game. Don't ask why it's here, it's pretty much just last minute.
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

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

        // Used for animations.
        if (player != null)
        { 
            corruption = (float)player.GetComponentInChildren<Corruption>().corrSlow; // Actually just used for the slow corruption.
            fastCorr = player.GetComponentInChildren<Corruption>().corrFast;
            corrupting = player.GetComponentInChildren<Corruption>().corrupting; // Sets a bool to allow the fast anim to loop
        }
        else
        {
            fastCorr = 0;
            corrupting = false;
        }

        slowCorrAnim.SetFloat("corrSlow", corruption);

        if (corrupting)
        {
            fastCorrAnim.SetBool("canLoop", true);
            fastCorrAnim.SetFloat("animDirection", 1.0f);
        }
        else
        {
            fastCorrAnim.SetBool("canLoop", false);
            fastCorrAnim.SetFloat("animDirection", -1.0f);
        }

        // As alpha can not be directly set, we instead set a tempColor, change its alpha, and set our UI color to that.
        // It's really dumb and it sounds redundant, I know.
        if (!done)
        {
            tempColor.a = (corruption / 100);
            corrUI.color = tempColor;
        }

        if (corruption >= 100 || Input.GetKey("r"))
        {
            //corrUI.sprite = corrEnd;
            corrAnim.SetBool("CorrFull", true);
            done = true;
            Destroy(player);
            if (!respawning)
            {
                StopCoroutine(RespPlayer());
                StartCoroutine(RespPlayer());
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
