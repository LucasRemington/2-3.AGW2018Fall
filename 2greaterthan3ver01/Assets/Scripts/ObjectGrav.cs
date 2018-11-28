using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrav : MonoBehaviour
{
    private Rigidbody rb;
    public float gravMult = 2.5f;

	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	

	void Update ()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gravMult - 1) * Time.deltaTime;
        }
    }
}
