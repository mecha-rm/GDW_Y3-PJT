using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyLerpMember : MonoBehaviour
{
    // the rigid body of the platform. This will automatically be filled if not provided.
    public Rigidbody rigidBody;

    // the force applied to move the platform.
    public float force = 1.0F;

    // the index of the start point.
    // when the platform reaches its destination, that destination becomes the start point.
    public int destIndex = 0;

    // if 'true', the platform stops moving.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        // gets the rigid body
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();

            // adds rigid body if one has not been found.
            if (rigidBody == null)
                rigidBody = gameObject.AddComponent<Rigidbody>();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
