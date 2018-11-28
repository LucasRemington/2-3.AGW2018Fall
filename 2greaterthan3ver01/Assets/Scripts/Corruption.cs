using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corruption : MonoBehaviour
{
    // Every time corrFast charges to full, corrSlow increments. If corrSlow hits 100, player dies.
    // corrCap determines what corrFast needs to be full.
    public int corrCap;
    public int corrSlow, corrFast;
    private bool corrupting;
	
	void Start ()
    {
        corrSlow = 0;
        corrFast = 0;
	}


    void Update()
    {
        // This de-increments our fast corruption when the player isn't currently being corrupted.
        if (!corrupting && corrFast > 0)
            corrFast--;

	}

    private void OnTriggerStay(Collider other)
    {
        // This chunk here should check to see if a child object has the tag "corrupt."
        // Code courtesy of brianturner on the Unity Answers forums, adjusted as needed.

        // Essentially, we take the hit object and loop through all of its child objects, looking for a Corrupt tag.
        // If we find it, we break the loop for efficiency and then begin incrementing our corrupt counters.
        var childCount = other.gameObject.transform.childCount;
        var childTag = transform.tag;
        for (var i = 0; i < childCount; ++i)
        {
            var child = other.gameObject.transform.GetChild(i);
            childTag = child.tag;

            if (childTag == "Corrupt")
                i = childCount + 1;
        }

        if (childTag == "Corrupt")
        {
            corrupting = true;

            // Our fast corruption increments between 0 and its specified cap. After each loop, fast corruption is incremented.
            if (corrFast < corrCap)
                corrFast++;
            else if (corrFast >= corrCap)
            {
                corrSlow++;
                corrFast = 0;
            }

        }
    }


    // Once an object leaves the trigger, if we -are- corrupting, and that object was corrupted, we turn it off.
    private void OnTriggerExit(Collider other)
    {
        if (corrupting)
        {
            var childCount = other.gameObject.transform.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = other.gameObject.transform.GetChild(i);
                var childTag = child.tag;

                if (childTag == "Corrupt")
                    corrupting = false;
            }
        }
    }
}
