using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    //Initialize movement variables. These can be edited in the inspector.
    public float runSpeed = 3.0f;
    public float jumpForce = 8.0f;
    public float airMoveForce = 4.0f;

    //For setting up movement. Bool checks if player is touching ground. Our rigidbody is referenced in Start().
    private Vector3 movement = Vector3.zero;

    //Setting up the flag for being paused; we don't want to move if we're paused.
    [HideInInspector] public bool paused = false;

    //Used in jumping.
    private float lastVel = 0.0f;
    private float lastVelX = 0.0f;
    private bool lastGrounded;
    private Rigidbody rb;
    [HideInInspector] public bool grounded;
    private int groundedCount;
    RaycastHit hitCenter, hitFront, hitBack;
    private int groundBack, groundFront, groundCenter;
    public float coyoteTime = 20f;
    private float coyote;
    public float maxJumpTime = 20f;
    private float jumpTime;
    private bool jumpReleased = true;

    //For the Raycast, we want to cast twice, from the front and from the back of the player.
    public GameObject castFront, castBack, castCenter;
    private float groundDistFront, groundDistBack, groundDistCenter;

    //What direction is the player facing? -1 is left, 1 is right. Grab the model, too.
    private int facing = 1;
    private int lastFace = 1;
    public GameObject model;

    [HideInInspector] public bool pushing;
   

    //Animation stuff, will use it later.
    public Animator anim;

    void Start ()
    {
        // To save typing, we grab the rigidbody component once here.
        // At start, our coyote timer and jump frame incrementer are set to their max values.
        rb = GetComponent<Rigidbody>();
        coyote = coyoteTime;
        jumpTime = maxJumpTime;
    }
	

	void Update ()
    {
        if (!paused)
        {
            // If the player is grounded and has released the jump button since their last jump, we can leave the ground.
            // Alternatively, if the player is airborne, as long as the jump timer is not at max or run out, player can jump.
            // These might be redundant. Heads up.
            Movement();
            if ((grounded || (jumpTime > 0 && jumpTime < maxJumpTime)) && !pushing)
            {
                Jump();
            }

            // Once we release the jump button we can jump again. This prevents infinite jumping just by holding down Jump.
            // It also sets our jump frame incrementer to zero, so pressing the button again won't suddenly give us upward momentum if we're falling.
            if (Input.GetButtonUp("XboxA"))
            {
                jumpReleased = true;
                jumpTime = 0.0f;
            }


            //Cast rays from the front, center, and rear end of the player's base.
            //Each ray sets its own unique true/false int as long as the object the ray hits counts as a platform and is immediately below the player.
            if (Physics.Raycast(castFront.transform.position, Vector3.down, out hitFront))
            {
                groundDistFront = hitFront.distance;
                if (groundDistFront < 0.1f && hitFront.transform.gameObject.tag != "Fallthrough")
                    groundFront = 1;
                else
                    groundFront = 0;
            }
            else groundFront = 0;
            if (Physics.Raycast(castBack.transform.position, Vector3.down, out hitBack))
            {
                groundDistBack = hitBack.distance;
                if (groundDistBack < 0.1f && hitBack.transform.gameObject.tag != "Fallthrough")
                    groundBack = 1;
                else
                    groundBack = 0;
            }
            else groundBack = 0;
            if (Physics.Raycast(castCenter.transform.position, Vector3.down, out hitCenter))
            {
                groundDistCenter = hitCenter.distance;
                if (groundDistCenter < 0.1f && hitCenter.transform.gameObject.tag != "Fallthrough")
                    groundCenter = 1;
                else
                    groundCenter = 0;
            }
            else groundCenter = 0;

            //We combine the unique grounded values to determine how jumping acts going forward.
            groundedCount = (groundBack + groundCenter + groundFront);


            //If at least one grounded count is true, jumping acts as normal. To consider: if only 2, use a "balancing on edge" animation.
            //Coyote time is set to 0 if we jump, and only returns if we are firmly grounded again.
            if (groundedCount >= 1)
            {
                grounded = true;
                coyote = coyoteTime;
                rb.useGravity = true;
                jumpTime = maxJumpTime;
            }

            //if we aren't grounded, we initiate coyote time and temporarily disable gravity. This gives the player a little extra leeway with jumping.
            //If coyote time runs out, or if the player jumps, gravity is enabled again.

            //An additional counter is present here so we can jump higher if we hold the button down, and increments by frame.
            else if (coyote > 0.0f && jumpReleased && grounded)
            {
                coyote -= 1.0f;
                rb.useGravity = false;
            }
            else if (jumpTime > 0.0f)
            {
                grounded = false;
                rb.useGravity = true;
                jumpTime -= 1.0f;
            }
            else
            {
                grounded = false;
                rb.useGravity = true;
            }

            //Added this bit so that the character only runs when he's on the ground. It seems inefficient, will try to figure out a better way to do this. -- Lucas
            if (grounded == true)
            {
                anim.SetBool("Grounded", true);
            } else
            {
                anim.SetBool("Grounded", false);
            }

            // This section just turns our model around to face the direction we're moving.
            // If we're pushing/pulling an object we don't bother.
            if (rb.velocity.x > 0.2f && !pushing)
            {
                facing = 1;
                if (lastFace == -1)
                {
                    model.transform.rotation = Quaternion.Slerp(Quaternion.AngleAxis(180, Vector3.up), Quaternion.AngleAxis(0, Vector3.up), 1f);
                }
            }
            else if (rb.velocity.x < -0.2f && !pushing)
            {
                facing = -1;
                if (lastFace == 1)
                {
                    model.transform.rotation = Quaternion.Slerp(Quaternion.AngleAxis(0, Vector3.up), Quaternion.AngleAxis(180, Vector3.up), 1f);
                }
            }


            // This must be the last line in Update(). This keeps track of the last frame of velocity.
            // It's fully possibly these will go completely unused, and that's okay. This is leftover code, just in case.
            lastVel = rb.velocity.y;
            lastFace = facing;
            lastGrounded = grounded;
        }
    }

    void Jump()
    {
        // When we first tap the jump button, we jump. Some flags are set.
        // If we continue to hold the jump button, we jump higher than we could just by tapping it!
        if (Input.GetButtonDown("XboxA") == true)
        {
            if (jumpReleased && grounded)
            {
                jumpReleased = false;
                jumpTime -= 1.0f;
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                rb.useGravity = true;
                anim.SetTrigger("Jump");
            }
        }

        // There are a lot of bools here and I know it looks scary.
        // First, check that our jump frame timer is within correct bounds; it hasn't run out but it has been incremented.
        // Then, make sure we our velocity is upward, not downward or zero. This way we don't have buggy hops after we land.
        // Finally, we make sure we've previously released the jump button and that we are currently holding said button down.
        // The jump force applied is currently set to half the base jump force! Bear that in mind!!
        else if ((jumpTime > 0 && jumpTime < maxJumpTime) && rb.velocity.y > 0 && (Input.GetButton("XboxA") == true) && !jumpReleased)
        {
            rb.AddForce(transform.up * (jumpForce / 2), ForceMode.Impulse);
            //anim.SetTrigger("Jump");
        }
    }

    void Movement()
    {
        //Grab left stick horizontal input. We won't use vertical (for now). We only multiply X axis by runspeed, and don't overwrite y axis.
        //Can't move mid-air. If we're pushing something, movespeed is cut in half.
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (!pushing)
            movement = new Vector3(moveHorizontal * runSpeed, rb.velocity.y, 0.0f);
        else if (pushing)
            movement = new Vector3(moveHorizontal * runSpeed * 0.5f, rb.velocity.y, 0.0f);

        if (grounded)
        {
            rb.velocity = movement;
            if (moveHorizontal != 0.0f)
            {
                anim.SetBool("Running", true);
                //Idea for future: when animating, multiply animation speed by moveHorizontal, since that's a 0 to 1 scale
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }
        else
        {
            //Change momentum in the air. Velocity is capped at running speed. Air Force is just the amount of control the player has.
            if ((rb.velocity.x > airMoveForce || rb.velocity.x < -airMoveForce))
            {
                rb.velocity = new Vector3(airMoveForce * facing, rb.velocity.y, rb.velocity.z);
            }
            /*else if (rb.velocity.x >= (lastVelX * facing))
            {
                rb.velocity = new Vector3(lastVelX, rb.velocity.y, rb.velocity.z);
            }*/
            else
            {
                rb.AddForce(rb.transform.right * moveHorizontal * 8 * facing);
            }
        }
    }
}
