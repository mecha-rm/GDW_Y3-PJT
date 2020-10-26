// class for the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    // the model that represents the player
    public GameObject model;

    // the player number
    public int playerNumber = 1;

    // the rigidbody for the player.
    public Rigidbody rigidBody; // maybe make this private since the start() function gets it?
    public float movementSpeed = 10.0F;
    public bool momentumMovement = false;

    // camera controls
    public Camera camera; // the player's camera
    private Vector3 direcVec; // the vector direction
    private Vector3 lastPos; // the player's previous position

    // TODO: TEMPORARY
    // this resets the player's position once they hit the death space. The way this is handled by change.
    public DeathSpace deathSpace;
    public Vector3 spawnPos = new Vector3(); // position upon spawning
    private Quaternion spawnRot = new Quaternion(0, 0, 0, 1); // rotation beyond spawning
    private Vector3 spawnScl = new Vector3(); // scale upon spawning

    // // saves the rotation of the camera
    // private Vector3 camRot = new Vector3(0.0F, 0.0F, 0.0F);
    // private Vector3 rotSpeed = new Vector3(100.0F, 100.0F, 0.0F);
    // private Vector2 xRotLimit = new Vector2(-50.0F, 50.0F);

    // animal characteristics
    // TODO: set these to protected for production build
    public float speedMult = 1.0F;
    public float knockbackMult = 1.0F;
    public float jumpMult = 1.0F;
    public float defenseMult = 1.0F;

    // TODO: add files for model?

    // Start is called before the first frame update
    void Start()
    {
        // gets the rigid body attached to this object if one hasn't been set.
        if (rigidBody == null)
            rigidBody = gameObject.GetComponent<Rigidbody>();

        // gets values to be reset upon spawning
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        spawnScl = transform.localScale;
    }

    // sets the speed multiplier
    public float GetSpeedMultiplier()
    {
        return speedMult;
    }

    // sets the speed multiplier
    public void SetSpeedMultiplier(float newMult)
    {
        speedMult = (newMult > 0.0F) ? newMult : speedMult;
    }

    // gets the knockback multiplier
    public float GetKnockbackMultiplier()
    {
        return knockbackMult;
    }

    // gets the knockback multiplier
    public void SetKnockbackMultiplier(float newMult)
    {
        knockbackMult = (newMult > 0.0F) ? newMult : knockbackMult;
    }

    // gets the jump multiplier
    public float GetJumpMultiplier()
    {
        return jumpMult;
    }

    // sets the jump multiplier
    public void SetJumpMultiplier(float newMult)
    {
        jumpMult = (newMult > 0.0F) ? newMult : jumpMult;
    }

    // gets the defense multiplier
    public float GetDefenseMultiplier()
    {
        return defenseMult;
    }

    // sets the defense multiplier
    public void SetDefenseMultiplier(float newMult)
    {
        defenseMult = (newMult > 0.0F) ? newMult : defenseMult;
    }

    // called when two objects collide with one another.
    // private void OnTriggerEnter(Collider other)
    // {
    //    if (other.GetComponent<Player>() != null) // player has collided with another player.
    //    {
    //         Vector3 p0Vel = rigidBody.velocity;
    //         Vector3 p1Vel = other.GetComponent<Rigidbody>().velocity;
    // 
    //         
    // 
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (momentumMovement)
        {
            // TODO: factor in deltaTime
            // forward and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 force = Vector3.forward * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;

            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 force = Vector3.back * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 force = Vector3.left * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 force = Vector3.right * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;
            }

            // upward and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 force = Vector3.up * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;
            }
            if (Input.GetKey(KeyCode.E))
            {
                Vector3 force = Vector3.down * movementSpeed * speedMult;
                rigidBody.AddForce(force);
                direcVec += force;
            }

            // normalize the direction vector.
            direcVec.Normalize();
        }
        else
        {
            // forward and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 shift = new Vector3(0, 0, movementSpeed * speedMult * Time.deltaTime);
                transform.Translate(shift);
                direcVec += shift;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 shift = new Vector3(0, 0, -movementSpeed * speedMult * Time.deltaTime);
                transform.Translate(shift);
                direcVec += shift;
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 shift = new Vector3(-movementSpeed * speedMult * Time.deltaTime, 0, 0);
                transform.Translate(shift);
                direcVec += shift;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Vector3 shift = new Vector3(movementSpeed * speedMult * Time.deltaTime, 0, 0);
                transform.Translate(shift);
                direcVec += shift;
            }

            // upward and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 shift = new Vector3(0, movementSpeed * speedMult * Time.deltaTime, 0);
                transform.Translate(shift);
                direcVec += shift;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 shift = new Vector3(0, -movementSpeed * speedMult * Time.deltaTime, 0);
                transform.Translate(shift);
                direcVec += shift;
            }
        }

        // gets the change in position - player has moved.
        // if(transform.position != lastPos)
        // {
        //     // we only care about the x and z axis position
        //     Vector2 v0 = new Vector3(transform.position.x, transform.position.z);
        //     Vector2 v1 = new Vector3(lastPos.x, lastPos.z);
        //     Vector2 v2 = v1 - v0;
        //     
        //     
        //     float angle = Vector3.Angle(v0, v1); // angle between the player's old and new position
        // 
        //     // gets follower camera component.
        //     FollowerCamera fc = camera.GetComponent<FollowerCamera>();
        //     if(fc != null)
        //     {
        //         fc.SetRotation(0.0F, -angle, 0.0F); // changes camera rotation
        //     }
        // }

        // entered death space
        if(deathSpace.InDeathSpace(transform.position)) 
        {
            // TODO: make changes for death
            transform.position = spawnPos;
            transform.rotation = spawnRot;
            transform.localScale = spawnScl;

            // remove all velocity
            rigidBody.velocity = new Vector3();
            rigidBody.angularVelocity = new Vector3();
        }

        // saves the player's current position
        lastPos = transform.position;
    }
}