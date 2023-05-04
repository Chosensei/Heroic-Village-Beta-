using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public static bool isInShop; // flag to track whether the player is in the shop or not
    public float moveSpeed = 5f;  // player movement speed
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // get the Rigidbody component on this object
    }

    void FixedUpdate()
    {
        // read user input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // calculate movement direction
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // apply movement to the Rigidbody
        rb.AddForce(movement * moveSpeed);
    }
}
