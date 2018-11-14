using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Check this in the editor to make the button return true only while it is held down, false if not.
    // Leave it unchecked to make the button return true once it's pressed and from there on, as well.
    // If Left is checked, the button faces left. If Right is checked, the button faces left. Etc. None = up.
    public bool hold, left, right, down;
    public float gravityMult;
    private int dir = 0;
    private Rigidbody rb;


    private void Start()
    {
        // Grab rigidbody for later. Dir is direction button faces.
        rb = gameObject.GetComponent<Rigidbody>();
        if (left)
            dir = -1;
        else
            dir = 1;

        if (gravityMult == 0)
            gravityMult = 1.1f;
    }

    private void Update()
    {
        if (left || right)
            rb.AddForce(dir, 0, 0, ForceMode.Force);


        else if (!down)
            rb.AddForce(0, Physics.gravity.y * -gravityMult, 0, ForceMode.Force);

        else
            rb.AddForce(0, Physics.gravity.y / -gravityMult, 0, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<Flag>().status = true;
        Debug.Log("touched");
    }

    private void OnTriggerExit(Collider other)
    {
        if (hold)
            gameObject.GetComponent<Flag>().status = false;
    }
}
