using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Reference to the menu Canvas.
    public Canvas menuCanvas;

    // Update is called once per frame.
    void Update()
    {
        // Replace this condition with your menu activation input (e.g., pressing a specific key).
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Toggle the menu visibility by enabling/disabling the Canvas.
            menuCanvas.enabled = !menuCanvas.enabled;

            // Toggle character movement by enabling/disabling the character's Rigidbody or CharacterController component.
            // Here, we assume that the character has a Rigidbody component for movement.
            Rigidbody2D characterRigidbody = GetComponent<Rigidbody2D>();
            if (characterRigidbody != null)
            {
                characterRigidbody.simulated = !menuCanvas.enabled;
            }
        }
    }
}



