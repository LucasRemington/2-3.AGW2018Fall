using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrow : MonoBehaviour
{

    public float throwStrength = 8.0f;
    public Transform holdPoint;

    //Bool for if we're holding something or not, we are able to interact, and a reference to what we're holding.
    private bool occupied;
    private bool canUse;
    public GameObject held;

    //Rigidbody of the parent object as well as the trigger itself, to save typing.
    private Rigidbody rb;
    public CapsuleCollider check;

    //Facing is used for throw direction.
    private int facing;

    void Start ()
    {
        //To save typing, we grab the rigidbody component and interaction trigger once here.
        rb = GetComponentInParent<Rigidbody>();
        check = GetComponent<CapsuleCollider>();

        //Set occupied to false, we can use something to start, and the object we're holding to Null on start.
        occupied = false;
        canUse = true;
        held = null;
    }
	
	
	void Update ()
    {
        //This little chunk here is literally just so that we don't release something as soon as we pick it up.
        if (Input.GetButtonUp("XboxB") == true)
            canUse = true;

        if (Input.GetAxisRaw("Horizontal") > 0)
            facing = 1;
        if (Input.GetAxisRaw("Horizontal") < 0)
            facing = -1;
        if (occupied && held != null)
        {
            //Keep the held object in front of us.
            held.transform.position = Vector3.Lerp(held.transform.position, holdPoint.position, 1);

            if (Input.GetButtonDown("XboxB") == true && canUse)
            {
                //Throw our object. First we make it unkinematic so physics apply, then add the impulse force. 
                //Finally, remove our occupied and held flags as well as restore its collider.
                held.GetComponent<Rigidbody>().isKinematic = false;
                held.GetComponent<Rigidbody>().AddForce(new Vector3((1.0f * facing), 0.5f) * throwStrength, ForceMode.Impulse);
                held.GetComponent<Collider>().enabled = true;
                occupied = false;
                held = null;
            }
        }
        


        
    }

    //This is our actual interaction command.
    private void OnTriggerStay(Collider other)
    {
        //Because the trigger always activates, we need to check button inputs specifically.
        //Pushable objects need the button held down. Interactables otherwise just need a single tap.
        if (Input.GetButton("XboxB") == true && !occupied && other.tag == "Pushable")
        {
            //Check for pushable objects.
            if (other.tag == "Pushable")
            {
                //Setting kinematic to false allows it to move via physics. Once we release the interact button,
                //turning kinematic back on means only scripts can move it.
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        if (Input.GetButtonDown("XboxB") == true && !occupied)
        {
            //Check for objects that can be picked up.
            if (other.tag == "Pickup")
            {
                // Once we pick up an object, do the following: Disable collider, disable physics, move it to a specified point in the editor.
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //ToDo for polish: Interpolate distance more smoothly over a short period of time so it doesn't just teleport.
                //other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, holdPoint.position, 1);
                held = other.gameObject;
                occupied = true;
                canUse = false;
            }

            //Check for objects that can otherwise be used.
            if (other.tag == "Checkable" && !occupied)
            {
                // Stick an interaction animation here. For now, it's instant, but later we'll program to wait for the animation to end.

                // Set flag of the object to "true." We make sure the proper script is present first.
                if (other.gameObject.GetComponent<Flag>())
                {
                    if (other.gameObject.GetComponent<Flag>().status == false)
                    other.gameObject.GetComponent<Flag>().status = true;

                    else if (other.gameObject.GetComponent<Flag>().status == true)
                        other.gameObject.GetComponent<Flag>().status = false;
                }
            }
        }

        else if (Input.GetButton("XboxB") == false)
        {
            //Release pushable objects.
            if (other.tag == "Pushable")
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
