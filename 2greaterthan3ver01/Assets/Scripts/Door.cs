using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // This script works with either doors that slide open, or simply disappear to open. 
    // Both can be vertical doors or horizontal, and can toggle between states.
    // This is designed to work primarily with vertical doors; if a door is horizontal,
    // please rotate it within the editor. A model based to be horizontal will not work.

    // To set up, within the editor, please toggle either Vanish or Slide. If Vanish is 
    // set to True, then also toggle one of Horizontal or Vertical. Toggle reverse if the door needs
    // to slide left or down. Make sure the Door object also has the Flag script attached!
    // Within the Flag script, a "false" status toggles the door closed, and "true" toggles it to open.


    // Set up Vector3s for open and closed states rather than calling for Transform.positions. Length, or height, is also called.
    private Vector3 open, closed;
    private float length;

    // Vanish and Slide are a set, and Horizontal and Vertical are a set of toggleables within the editor. Reverse is its own.
    public bool vanish, slide, horizontal, vertical, reverse;

    // slideSpeed just determines how quickly the door slides. A HIGHER NUMBER IS SLOWER.
    public int slideSpeed = 1;
	
	void Start ()
    {
        // We set closed and open, our V3's, to our transform position. 
        // This allows us to edit the position without our variables overwriting each other.
        closed = gameObject.transform.position;
        open = closed;

        // Length finds the length of the model altogether within the script itself. This is multiplied by however
        // much it's been stretched in the local transform, and because models are actually a bit bigger than
        // their actual dimensions, multiplied by 2 again. If that's purely the case for primitives, we can get rid of that 2.
        length = GetComponent<MeshFilter>().mesh.bounds.extents.y * gameObject.transform.localScale.y * 2;

        // This simply sets the closed position based off of what bools are toggled. Really only
        // relevant for a sliding door.
        if (vertical && !reverse)
            open = closed + new Vector3(0, length, 0);

        else if (horizontal && !reverse)
            open = closed + new Vector3(length, 0, 0);

        else if (vertical && reverse)
            open = closed - new Vector3(0, length, 0);

        else if (horizontal && reverse)
            open = closed - new Vector3(length, 0, 0);

    }
	
	
	void Update ()
    {
    
        // Call the attached Flag script's status bool to determine if we need to close or open.
        if (gameObject.GetComponent<Flag>().status == true)
        {
            if (slide)
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, open, 1.0f / slideSpeed);
            if (vanish)
            {
                // If the object vanishes, we only disable the renderer and collider so its scripts still work.
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        else if (gameObject.GetComponent<Flag>().status == false)
        {
            if (slide)
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, closed, 1.0f / slideSpeed);
            if (vanish)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
            }  
        }
	}
}
