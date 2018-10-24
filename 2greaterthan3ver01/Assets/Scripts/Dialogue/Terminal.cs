using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Dialogue dialogue;

    void Update()
    {
        /*if (gameObject.GetComponent<Flag>().status)
        {
            TerminalDialogue();
            gameObject.GetComponent<Flag>().status = false;
        }*/
    }
    public void TerminalDialogue()
    {
        FindObjectOfType<DiaManager>().StartDialogue(dialogue);
    }
}
