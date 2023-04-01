using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    public GameObject blizzardAttack;
    public GameObject impaleObject = null;
    //Movement
    public float speed = 10.0f;
    private float translation;
    private float straffe;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(blizzardAttack, transform.position + blizzardAttack.GetComponent<BlizzardSpawner>().spawnOffset, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            var spawnCalculation = transform.position + transform.forward;
            //Create the first impale object that wil be the parent and will duplicate itself
            Instantiate(impaleObject, spawnCalculation, transform.rotation);
        }
    }
}
