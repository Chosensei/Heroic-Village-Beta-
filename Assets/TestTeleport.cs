using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleport : MonoBehaviour
{
    public Transform teleportTarget; // The empty GameObject to teleport the player to

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player detected");
            Rigidbody rb = other.attachedRigidbody;
            Vector3 velocity = rb.velocity;
            Vector3 angularVelocity = rb.angularVelocity;
            // Teleport the player to the target position
            other.transform.position = teleportTarget.position;
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }
}
