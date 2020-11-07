// class for the player
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    // the model that represents the player (TODO: load it from prefab using filepath)
    public GameObject model;
    public StateMachine stateMachine = null;

    // the player number
    public int playerNumber = 1;
    public float playerScore = 0.0F;
    public Text playerScoreText = null; // TODO: maybe make a dedicated script to handle this.

    // the rigidbody for the player.
    private Rigidbody rigidBody; // maybe make this private since the start() function gets it?
    public float movementSpeed = 12.0F;
    public float jumpForce = 20000.0F;
    public float backupFactor = 0.5F;
    public bool momentumMovement = false;

    // camera controls
    public FollowerCamera camera; // the player's camera
    public Vector3 camPosOffset = new Vector3(0, 3, -7); // the offset for the camera that's attached to the player.

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
    private Vector3 rotSpeed = new Vector3(150.0F, 150.0F, 150.0F);
    private Vector2 xRotLimit = new Vector2(-50.0F, 50.0F);
    private float maxVelocity = 50.0F;

    // animal characteristics
    // TODO: set these to protected for production build
    public float speedMult = 1.0F;
    public float knockbackMult = 1.0F;
    public float jumpMult = 1.0F;
    public float defenseMult = 1.0F;

    // flag object
    // TODO: have score countdown until it hits zero.
    public FlagObject flag = null;
    public GameObject flagIndicator = null; // the indicator that the player is holding the flag.

    // public float score = 1000.0F;
    // public float scoreDecRate = 0.25F;

    // Start is called before the first frame update
    void Start()
    {
        // gets the rigid body attached to this object if one hasn't been set.
        if (rigidBody == null)
            rigidBody = gameObject.GetComponent<Rigidbody>();


        rigidBody.freezeRotation = true;

        // state machine hasn't been set.
        if (stateMachine == null)
            stateMachine = gameObject.GetComponent<StateMachine>();

        // state machine hasn't been set.
        if (playerScoreText == null)
            playerScoreText = gameObject.GetComponent<Text>();

        // camera settings
        if (camera.target != gameObject)
        {
            camera.target = gameObject;
            camera.distance = camPosOffset;
        }

        // gets values to be reset upon spawning
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        spawnScl = transform.localScale;
    }

    // attaches the flag to the player
    public void AttachFlag(FlagObject flag)
    {
        flag.AttachToPlayer(this);
    }

    // detaches the flag from the player
    public void DetachFlag()
    {
        if (flag != null)
            flag.DetachFromPlayer();
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
            // TODO: factor in deltaTime for movement
            // TODO: lerp camera for rotation
            
            // Movement
            {
                // forward and backward movement
                if (Input.GetKey(KeyCode.W))
                {
                    // Vector3 force = Vector3.forward * movementSpeed * speedMult;
                    // // Vector3 force = transform.forward * movementSpeed * speedMult;
                    // rigidBody.AddForce(force);
                    // direcVec += force;
                    Vector3 force = transform.forward * movementSpeed * speedMult;
                    rigidBody.AddForce(force);
                    direcVec += force;
                    stateMachine.SetState(1);

                }
                else if (Input.GetKey(KeyCode.S))
                {
                    // TODO: maybe instead of moving down you just rotate the player.
                    // Vector3 force = Vector3.back * movementSpeed * speedMult;
                    Vector3 force = -transform.forward * movementSpeed * backupFactor * speedMult;


                    rigidBody.AddForce(force);

                    direcVec += force;

                    // TODO: rotate if not facing backward.

                }

                // leftward and rightward movement
                if (Input.GetKey(KeyCode.A))
                {
                    // Vector3 force = Vector3.left * movementSpeed * speedMult;
                    // Vector3 force = -transform.right * movementSpeed * speedMult;
                    // rigidBody.AddForce(force);
                    // direcVec += force;


                    // if there is no velocity, set the player's rotation to -90 degrees.
                    if(Input.GetKey(KeyCode.W)) // if the player is going foward
                    {
                        Vector3 force = -transform.right * movementSpeed * speedMult;
                        rigidBody.AddForce(force);
                        direcVec += force;

                        transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                    }
                    else // the player is not going forward so, rotate instead.
                    {
                        transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    // Vector3 force = Vector3.right * movementSpeed * speedMult;
                    // Vector3 force = transform.right * movementSpeed * speedMult;
                    // rigidBody.AddForce(force);
                    // direcVec += force;

                    // if there is no velocity, set the player's rotation to -90 degrees.
                    if (Input.GetKey(KeyCode.W)) // if the player is going foward
                    {
                        Vector3 force = transform.right * movementSpeed * speedMult;
                        rigidBody.AddForce(force);
                        direcVec += force;
                        // transform.Rotate(Vector3.up, rotSpeed.y * Time.deltaTime);
                    }
                    else // the player is not going forward so, rotate instead.
                    {
                        transform.Rotate(Vector3.up, rotSpeed.y * Time.deltaTime);
                    }
                }

                // unused
                // upward and downward movement
                // if (Input.GetKey(KeyCode.Q))
                // {
                //     Vector3 force = Vector3.up * movementSpeed * speedMult;
                //     // Vector3 force = transform.up * movementSpeed * speedMult;
                //     rigidBody.AddForce(force);
                //     direcVec += force;
                // }
                // if (Input.GetKey(KeyCode.E))
                // {
                //     Vector3 force = Vector3.down * movementSpeed * speedMult;
                //     // Vector3 force = -transform.up * movementSpeed * speedMult;
                //     rigidBody.AddForce(force);
                //     direcVec += force;
                // }
            }

            // Hard Rotation (Snap)
            {
                // rotate to the left (slow)
                if(Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                }
                // rotate to the right
                else if(Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Rotate(Vector3.up, +rotSpeed.y * Time.deltaTime);
                }


                // hard rotation
                // if (Input.GetKeyDown(KeyCode.LeftArrow))
                // {
                //     transform.Rotate(Vector3.up, -90.0F);
                // }
                // else if(Input.GetKeyDown(KeyCode.RightArrow))
                // {
                //     transform.Rotate(Vector3.up, 90.0F);
                // }

                // hard rotation
                // judge player forward distance
                // if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                // {
                //     // hard rotation 90 degrees to the left.
                //     if (Input.GetKeyDown(KeyCode.LeftArrow))
                //         transform.Rotate(Vector3.up, -90.0F);
                // 
                //     // hard rotation 90 degrees to the right.
                //     if (Input.GetKeyDown(KeyCode.RightArrow))
                //         transform.Rotate(Vector3.up, 90.0F);
                // 
                //     // gets the distance between the four axes to see which one the player is closest too.
                //     
                // 
                //     float distF = Vector3.Distance(transform.forward, Vector3.forward);
                //     float distB = Vector3.Distance(transform.forward, Vector3.back);
                //     float distL = Vector3.Distance(transform.forward, Vector3.left);
                //     float distR = Vector3.Distance(transform.forward, Vector3.right);
                // 
                //     // gets the shortest distance
                //     float shortDist = Mathf.Min(distF, distB, distL, distR);
                // 
                //     
                // 
                //     // checks to see what distance the player is closest to.
                //     if (shortDist == distF)
                //         transform.eulerAngles = new Vector3(0.0F, 0.0F, 0.0F);
                //     else if (shortDist == distB)
                //         transform.eulerAngles = new Vector3(0.0F, 90.0F, 0.0F);
                //     else if (shortDist == distL)
                //         transform.eulerAngles = new Vector3(0.0F, 180.0F, 0.0F);
                //     else if (shortDist == distR)
                //         transform.eulerAngles = new Vector3(0.0F, 270.0F, 0.0F);
                // 
                // 
                // }

                // turn backwards
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    transform.Rotate(Vector3.up, 180.0F);
                }
            }

            // jump
            {
                // TODO: check to see if this effects slopes.
                if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && rigidBody.velocity.y == 0.0F)
                {
                    // transform.position = new Vector3(transform.position.x, transform.position.y + 0.1F, transform.position.z);
                    // rigidBody.AddExplosionForce(jumpForce * 1000.0f, transform.position, 1.0F);
                    rigidBody.AddForce(Vector3.up * jumpForce * Time.deltaTime);
                    // rigidBody.AddForce(Vector3.up * jumpForce);

                }
            }

            // gets the angle
            // {
            //     Vector3 currVelo = rigidBody.velocity;
            //     if(currVelo != new Vector3())
            //     {
            //         transform.LookAt(transform.position + currVelo);
            //         transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0F, 1.0F);
            // 
            //         // camera.rotation = transform.rotation.eulerAngles;
            //         // camera.rotation.x = 0.0F;
            //         // camera.rotation.y *= -1;
            //         // camera.rotation.z = 0.0F;
            //     }
            //     
            // }

            // direcVec.Normalize();

            // caps velocity
            if (rigidBody.velocity.magnitude > maxVelocity)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * maxVelocity;

                // float revForce = rigidBody.velocity.magnitude - maxVelocity;
                // rigidBody.AddForce(rigidBody.velocity.normalized * -1 * revForce);
            }
           
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

        // TODO: take this out or make it more efficient.
        // player isn't moving
        if(stateMachine.state != 0 && rigidBody.velocity == new Vector3())
        {
            stateMachine.SetState(0);
        }

        // entered death space
        if(deathSpace.InDeathSpace(transform.position)) 
        {
            // takes away the flag
            if (flag != null)
                flag.DetachFromPlayer();

            // returns player to spawn position
            // TODO: make changes for death
            transform.position = spawnPos;
            transform.rotation = spawnRot;
            transform.localScale = spawnScl;

            // remove all velocity
            rigidBody.velocity = new Vector3();
            rigidBody.angularVelocity = new Vector3();
            stateMachine.SetState(0);
        }

        // if the player has a flag, gain a point.
        if(flag != null)
        {
            playerScore += Time.deltaTime;
            playerScoreText.text = "Player Score: " + Mathf.Floor(playerScore);
        }

        // saves the player's current position
        lastPos = transform.position;
    }
}