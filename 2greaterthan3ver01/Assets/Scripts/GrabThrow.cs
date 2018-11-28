using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrow : MonoBehaviour
{

    public float throwStrength = 8.0f;
    public Transform holdPoint;

    //Bool for if we're holding something or not, we are able to interact, and a reference to what we're holding.
    [HideInInspector] public bool occupied;
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

        //Facing is used to determine what direction we throw stuff in. Note, you can throw stuff at yourself as this differs
        //from normal facing in BasicMovement.cs
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

                if (Input.GetAxis("Vertical") != 0)
                    held.GetComponent<Rigidbody>().AddForce(new Vector3(0f, (Input.GetAxisRaw("Vertical")) * 2) * throwStrength, ForceMode.Impulse);

                if (Input.GetAxis("Vertical") == 0)
                    held.GetComponent<Rigidbody>().AddForce(new Vector3((1.0f * facing), 0.5f) * throwStrength, ForceMode.Impulse);

                held.GetComponent<Collider>().isTrigger = false;
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
        if (Input.GetButton("XboxB") == true && !occupied && other.tag == "Pushable" && rb.GetComponent<BasicMovement>().grounded)
        {

            // We already know this object is pushable, so we just double check that we're grounded so we can't push something mid-air.
            if (rb.gameObject.GetComponent<BasicMovement>().grounded == true)
            {
                //Setting kinematic to false allows it to move via physics. Once we release the interact button,
                //turning kinematic back on means only scripts can move it.
                //We also call the movement script's pushing bool.
                rb.gameObject.GetComponent<BasicMovement>().pushing = true;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                occupied = true;

                // We create a fixed joint while the button is being held down. This allows us to pull it.
                if (!rb.gameObject.GetComponent<FixedJoint>())
                {
                    var attach = rb.gameObject.AddComponent<FixedJoint>();
                    attach.connectedBody = other.GetComponent<Rigidbody>();
                    attach.enableCollision = true;
                }
            }
        }



        if (Input.GetButtonDown("XboxB") == true && !occupied)
        {
            //Check for objects that can be picked up.
            if (other.tag == "Pickup")
            {
                // Once we pick up an object, do the following: Disable collider, disable physics, move it to a specified point in the editor.
                other.gameObject.GetComponent<Collider>().isTrigger = true;
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

                // Like our corruption script, we loop through child objects to check if we're interacting with a terminal.
                // Do NOT tag the terminal itself as a terminal, only a child object!!
                var childCount = other.gameObject.transform.childCount;
                var childTag = transform.tag;
                for (var i = 0; i < childCount; ++i)
                {
                    var child = other.gameObject.transform.GetChild(i);
                    childTag = child.tag;

                    if (childTag == "Terminal")
                        i = childCount + 1;
                }

                // If we are, we pause the player's movements and set the flag on the terminal to true.
                // I also hate breaking an if statement into multiple lines, ugh ;~;
                if (childTag == "Terminal" && rb.gameObject.GetComponent<BasicMovement>().grounded 
                    && rb.gameObject.GetComponent<BasicMovement>().paused == false)
                {
                    other.gameObject.GetComponent<Terminal>().TerminalDialogue(occupied);
                    rb.gameObject.GetComponent<BasicMovement>().paused = true;
                }

                    // Set flag of the object to "true." We make sure the proper script is present first.
                    if (other.gameObject.GetComponent<Flag>() && childTag != "Terminal")
                {
                    if (other.gameObject.GetComponent<Flag>().status == false)
                    other.gameObject.GetComponent<Flag>().status = true;

                    else if (other.gameObject.GetComponent<Flag>().status)
                        other.gameObject.GetComponent<Flag>().status = false;
                }
            }
        }



        else if (Input.GetButton("XboxB") == false)
        {
            //Release pushable objects.
            if (other.tag == "Pushable")
            {
                Debug.Log("Release");
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                rb.gameObject.GetComponent<BasicMovement>().pushing = false;

                //Destroy fixed joint.
                Destroy(rb.gameObject.GetComponent<FixedJoint>());
            }
        }
    }
}
