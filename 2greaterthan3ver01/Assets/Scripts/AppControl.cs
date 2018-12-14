using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppControl : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Hitting escape quits the game. Don't ask why it's here, it's pretty much just last minute.
        if (Input.GetKey("escape"))
        {
            Quit();
        }
    }

    public void Quit()
    {
        Debug.Log("Application quit.");
        Application.Quit();        
    }

    public void Launch()
    {
        Debug.Log("Loading scene...");
        SceneManager.LoadScene("MechanicsTest");
    }
}
