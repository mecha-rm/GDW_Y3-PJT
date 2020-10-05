// class for the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float movementSpeed = 10.0F;
    public bool momentumMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody = GetComponent<Rigidbody>();
        // use AddForce on the rigid body 
    }

    // Update is called once per frame
    void Update()
    {
        if (momentumMovement)
        {
            // forward and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.AddForce(Vector3.forward * movementSpeed);
                
            }
            if (Input.GetKey(KeyCode.S))
            {
                rigidBody.AddForce(Vector3.back * movementSpeed);
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                rigidBody.AddForce(Vector3.left * movementSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rigidBody.AddForce(Vector3.right * movementSpeed);
            }

            // upward and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                rigidBody.AddForce(Vector3.up * movementSpeed);
            }
            if (Input.GetKey(KeyCode.E))
            {
                rigidBody.AddForce(Vector3.down * movementSpeed);
            }
        }
        else
        {
            // forward and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -movementSpeed * Time.deltaTime));
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-movementSpeed * Time.deltaTime, 0, 0));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(movementSpeed * Time.deltaTime, 0, 0));
            }

            // upward and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(0, movementSpeed * Time.deltaTime, 0));
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, -movementSpeed * Time.deltaTime, 0));
            }
        }
    }
}
