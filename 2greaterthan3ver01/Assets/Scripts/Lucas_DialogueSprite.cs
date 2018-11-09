using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucas_DialogueSprite : MonoBehaviour {

    public Animator anim;
    public Animator diagAnim;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (diagAnim.GetBool("IsOpen") == true)
        {
            anim.SetBool("Active", true);
        } else
        {
            anim.SetBool("Active", false);
        }
	}
}
