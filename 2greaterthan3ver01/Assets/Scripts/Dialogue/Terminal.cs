using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Dialogue dialogue;
    public Animator anim;
    public BasicMovement baseMove;
    public Animator playerAnim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (baseMove.paused == true)
        {
            anim.SetBool("Active", true);
        } else
        {
            anim.SetBool("Active", false);
        }
    }
    public void TerminalDialogue()
    {
        FindObjectOfType<DiaManager>().StartDialogue(dialogue);
    }
}
