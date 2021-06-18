// class for the player
using UnityEngine;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    // the player number
    public int playerNumber = 0;

    // player name and descrition
    public string playerName = "";
    public string description = "";

    // the model that represents the player (TODO: load it from prefab using filepath)
    public GameObject model;
    public StateMachine stateMachine = null;

    public float playerScore = 0.0F;
    public Text playerScoreText = null; // TODO: maybe make a dedicated script to handle this.

    // the jump cycle - these are meant to fix the jump.
    // this is currently unused, but might be implemented later.
    private bool onGround = false;

    // the rigidbody for the player.
    public Rigidbody rigidBody = null; // rigid body (why is this private?)
    // if 'true', the player can be controlled.
    public bool controllablePlayer = true;
    // public Collider playerCollider = null;

    // various movement factors
    public float movementSpeed = 2500.0F;
    public float jumpForce = 10.0F;
    public float backupFactor = 0.5F;
    public bool momentumMovement = true;

    // force applied when two players collide.
    // the knockback and the defense multipliers are applied here.
    public float bounceForce = 7.25F;

    // camera controls
    public FollowerCamera playerCamera; // the player's camera (TODO: generate a dedicated camera for the player)
    public Vector3 cameraDistance = new Vector3(0, 3, -7); // the offset for the camera that's attached to the player.

    // private Vector3 direcVec; // the vector direction
    // private Vector3 lastPos; // the player's previous position

    // TODO: TEMPORARY (?)
    // TODO: the gameplay manager or death space should look at the player objects and see if they're dead.
    // TODO: the spawn positions should be in the gameplay manager since it should have the map data. Move it later.
    // this resets the player's position once they hit the death space. The way this is handled by change.
    public Vector3 spawnPos = new Vector3(); // position upon spawning
    public Quaternion spawnRot = new Quaternion(0, 0, 0, 1); // rotation beyond spawning
    public Vector3 spawnScl = new Vector3(); // scale upon spawning

    // // saves the rotation of the camera
    // private Vector3 camRot = new Vector3(0.0F, 0.0F, 0.0F);
    private Vector3 rotSpeed = new Vector3(150.0F, 150.0F, 150.0F);
    private Vector2 xRotLimit = new Vector2(-50.0F, 50.0F);

    // normal max velocity is the normal speed limit.
    // flag held velocity is used when the player is holding the flag.
    // flag held factor reduces acceleration when holding the flag.
    private float maxVelocity = 40.0F;
    private float flagMaxVelocity = 35.0F;
    private float flagHeldFactor = 0.85F;

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

    // the amount of time to takes for the idle animation and sound to play
    public float idleEffectWaitTime = 5.0F;
    private float idleEffectCurrTime = 0.0F;

    // sound effects
    public AudioSource sfx_Idle = null;

    public AudioSource sfx_Run = null;
    public AudioSource sfx_FlagGet = null; // TODO: this could maybe be static.
    public AudioSource sfx_Death = null;

    // public AudioSource sfx_FlagLoss = null;

    // item icon to be activated upon being applied.
    protected GameObject playerIcon = null;

    // the prefab for the player icon
    protected string playerIconPrefab = "";


    // Start is called before the first frame update
    protected void Start()
    {
        // gets the rigid body attached to this object if one hasn't been set.
        if (rigidBody == null)
            rigidBody = gameObject.GetComponent<Rigidbody>();


        rigidBody.freezeRotation = true;

        // gets the collider attached to the player
        // if (playerCollider == null)
        //     playerCollider = gameObject.GetComponent<Collider>();


        // state machine hasn't been set.
        if (stateMachine == null)
            stateMachine = gameObject.GetComponent<StateMachine>();

        // state machine hasn't been set.
        // TODO: take this out.
        if (playerScoreText == null)
        {

            // TEMP: change the way this works for final build.
            GameObject temp = GameObject.Find("P1 Score Text");

            if(temp != null)
            {
                playerScoreText = temp.GetComponent<Text>();
            }
            else
            {
                temp = new GameObject("Player " + playerNumber + " Score Text");
                playerScoreText = temp.AddComponent<Text>();
            }

        }

        // camera settings
        // if (camera.target != null)
        // {
        //     camera.target = gameObject;
        //     camera.distance = camPosOffset;
        // }

        // if no player camera has been set, a new one will be created.
        // it first checks if the main camera has the right script

        // TODO: for some reason if you go back and change your character it causes there to be no camera with an audio listener.
        {
            // used if the camera couldn't be foun.
            bool setCamera = true;

        // Camera:
            if (playerCamera == null && setCamera)
            {
                // player number is 0, so it may not make an new camera.
                // grabs main camera if it has the right script.
                if (playerNumber == 0)
                {
                    // gets main camera
                    GameObject mainCam = GameObject.Find("Main Camera");
                    Camera camComp = mainCam.GetComponent<Camera>();

                    // makes the main camera the follower component.
                    if (camComp != null)
                    {
                        // if(!camComp.isActiveAndEnabled)
                        //     MakeFollowerCamera(camComp);

                        SetFollowerCamera(camComp);
                    }
                    else
                    {
                        // generates follower camera
                        GetFollowerCamera();
                    }

                    // main cmaera wasn't found, so generate a new main camera.
                    // if (mainCam == null)
                    // {
                    //     setCamera = false;
                    //     goto Camera;
                    // }

                    // gets camera follower
                    // FollowerCamera mainCamFollower = mainCam.GetComponent<FollowerCamera>();
                    // 
                    // // main camera
                    // if (mainCamFollower != null && mainCamFollower.isActiveAndEnabled)
                    // {
                    //     // if the component is active and enabled, use it.
                    //     if (mainCamFollower.isActiveAndEnabled)
                    //     {
                    //         playerCamera = mainCamFollower;
                    //     }
                    //     else
                    //     {
                    //         mainCamFollower.enabled = true;
                    //         playerCamera = mainCamFollower;
                    //     }
                    // }
                    // else
                    // {
                    //     // adds the follwoer camera script to the main cmaera
                    //     mainCamFollower = mainCam.AddComponent<FollowerCamera>();
                    //     playerCamera = mainCamFollower;
                    // }
                    // 
                    // mainCamFollower.target = gameObject;
                    // 
                    // // goes to the label if the player found a camera
                    // if (playerCamera != null)
                    // {
                    //     setCamera = false;
                    //     goto Camera;
                    // }

                }
                else
                {
                    // creates an empty player object and gives it a camera.
                    GetFollowerCamera();
                }

                

            }
            else if (playerCamera.target != gameObject)
            {
                playerCamera.target = gameObject;
                playerCamera.distance = cameraDistance;
            }
        }
        

        // gets values to be reset upon spawning
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        spawnScl = transform.localScale;

        // gets idleEffectTime.
        // idleEffectCurrTime = idleEffectWaitTime;

        // initializes audio if not set
        {
            AudioSource[] audios = GetComponentsInChildren<AudioSource>();

            // initializes sound effects if not set.
            if(sfx_Run == null && audios.Length >= 1)
                sfx_Run = audios[0];

            if (sfx_Idle== null && audios.Length >= 2)
                sfx_Idle = audios[1];

            if (sfx_FlagGet == null && audios.Length >= 3)
                sfx_FlagGet = audios[2];

            if (sfx_Death == null && audios.Length >= 4)
                sfx_Death = audios[3];
        }
    }

    // there is a primary collider, and a secondary (ground collider).
    // the first collider is for general collisions to stop the player from going through obstacles.
    // the second collider is for tracking whether the player is on the ground or not.
    // the second collider uses onTrigger, so it does not physically stop the player at all.
    
    // checks to see if the player collided with another player.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerObject p2 = collision.gameObject.GetComponent<PlayerObject>();

            // transfers the flag to the other player.
            if (flag != null)
                flag.TransferFlag(p2);

            // bounce back between players.
            // Vector3 diff = transform.position - p2.transform.position;
            // rigidBody.AddExplosionForce(diff); // explosive
            // p2.rigidBody.AddExplosionForce(diff.Scale(new Vector3(-1, -1, -1))); // explosive
            
            // applies an explosive force to move the player back.
            // the knockback strength of player 2 is applied 
            rigidBody.AddExplosionForce(bounceForce * p2.knockbackMult / defenseMult, p2.transform.position, 10.0F, 1.0F, ForceMode.Impulse);
            // rigidBody.AddExplosionForce(bounceForce * p2.knockbackMult / defenseMult, collision.GetContact(0).point, 10.0F, 1.0F, ForceMode.Impulse);

            // p2.rigidBody.AddExplosionForce(bounceForce, transform.position, 35.0F, 2.0F, ForceMode.Impulse);
        }
    }


    // ground collider has been triggered.
    // private void OnTriggerEnter(Collider other)
    private void OnTriggerStay(Collider other) // changed to 'Stay' for more reliability.
    {
        // this should only be checked if onGround has been set to false.
        if(!onGround)
        {
            if (other.gameObject.tag == "StageObject" || other.gameObject.tag == "Untagged")
                onGround = true;
        }
        

        // Debug.Log("OnGround");
    }

    // ground collider has been left.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "StageObject" || other.gameObject.tag == "Untagged")
            onGround = false;

        // Debug.Log("OffGround");
    }

    // gets the player number
    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    // sets the new player number
    public void SetPlayerNumber(int newNumber)
    {
        playerNumber = newNumber;

        // sets the target display of the camera object.
        if (playerCamera != null && newNumber > 0)
        {
            // temporary camera object
            Camera cam = playerCamera.GetCamera();

            // if there is a camera, set its target display.
            if(cam != null)
                playerCamera.cameraObject.targetDisplay = newNumber;
        }
    }

    // loads the player icon
    public void LoadPlayerIcon(string prefab)
    {
        playerIconPrefab = prefab;
        LoadPlayerIcon();
    }

    // loads the player icon
    public void LoadPlayerIcon()
    {
        playerIcon = Instantiate(Resources.Load(playerIconPrefab)) as GameObject;

        // gets text object.
        if (playerIcon != null)
        {
            playerScoreText = playerIcon.GetComponentInChildren<Text>();
        }
    }

    // positions the player icon using player spaces based on player number.
    public void ParentIconToPlayerSpace()
    {
        // no player icon set.
        if (playerIcon == null)
            return;

        // space object.
        GameObject pSpace = null;

        switch(playerNumber)
        {
            case 0:
            case 1: // p1 space
                pSpace = GameObject.Find("Player 1 Space");
                break;

            case 2: // p2 space
                pSpace = GameObject.Find("Player 2 Space");
                break;

            case 3: // p3 space
                pSpace = GameObject.Find("Player 3 Space");
                break;

            case 4: // p4 space
                pSpace = GameObject.Find("Player 4 Space");
                break;
        }

        // sets parent.
        if (playerIcon != null && pSpace != null)
            playerIcon.transform.parent = pSpace.transform;
    }

    // gets the follower camera, generating a new one if it doesn't exist.
    public FollowerCamera GetFollowerCamera()
    {
        // player camera
        if (playerCamera != null)
            return playerCamera;

        // tries to get the camera
        playerCamera = GetComponent<FollowerCamera>();

        // returns the player camera if it's not set.
        if (playerCamera != null)
            return playerCamera;

        GameObject newCamera = new GameObject("Player " + playerNumber + " Camera");
        newCamera.tag = "Camera";

        Camera camComp = newCamera.AddComponent<Camera>();
        camComp.depth = -1;

        if (playerNumber > 0)
            camComp.targetDisplay = playerNumber;

        // attaches the camera
        // adds a follower camera script, and gives it the base value.
        playerCamera = newCamera.AddComponent<FollowerCamera>();
        playerCamera.cameraObject = camComp;
        
        playerCamera.distance = cameraDistance;
        playerCamera.target = gameObject;
        playerCamera.useParentRotation = true;

        return playerCamera;
    }


    // sets the follower camera
    // this does NOT change the target display.
    public void SetFollowerCamera(FollowerCamera cam)
    {
        // provided camera is null.
        if(cam == null)
        {
            return;
        }

        // attaches the camera
        playerCamera = cam;
        playerCamera.cameraObject = playerCamera.gameObject.GetComponent<Camera>();

        // attaches the camera
        // adds a follower camera script, and gives it the base value.

        playerCamera.distance = cameraDistance;
        playerCamera.target = gameObject;
        playerCamera.useParentRotation = true;
    }

    // sets the follower camera using a regular camera.
    // this adds a follower component if it doesn't exist.
    // this does NOT change the target display.
    public FollowerCamera SetFollowerCamera(Camera camera)
    {
        // player camera
        if (camera == null)
            return null;

        // tries to get the camera
        playerCamera = camera.gameObject.GetComponent<FollowerCamera>();

        // if the camera didn't have a follower camera component, add one.
        if (playerCamera == null)
            playerCamera = camera.gameObject.AddComponent<FollowerCamera>();

        camera.tag = "Camera";
        camera.depth = -1;

        // attaches the camera
        playerCamera.cameraObject = camera;

        // set values
        playerCamera.distance = cameraDistance;
        playerCamera.target = gameObject;
        playerCamera.useParentRotation = true;

        return playerCamera;
    }



    // attaches the flag to the player
    public void AttachFlag(FlagObject flag)
    {
        flag.AttachToPlayer(this);

        // plays the flag sound effect.
        sfx_FlagGet.Play();
    }

    // detaches the flag from the player
    public void DetachFlag()
    {
        if (flag != null)
            flag.DetachFromPlayer();
    }

    // checks to see if the player has the flag.
    public bool HasFlag()
    {
        return flag != null;
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

    // respawns the player. Doing so takes away the flag.
    // this is called when entering the death space.
    public void Respawn()
    {
        // takes away the flag
        if (flag != null)
            flag.DetachFromPlayer();

        // returns player to spawn position
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        transform.localScale = spawnScl;

        // remove all velocity
        rigidBody.velocity = new Vector3();
        rigidBody.angularVelocity = new Vector3();
        stateMachine.SetState(0);

        // play death sound effect.
        if(sfx_Death != null)
            sfx_Death.Play();
    }

    // TODO: set spawn position
    public void SetSpawn(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        spawnPos = pos;
        spawnRot = rot;
        spawnScl = scl;
    }

    // Update is called once per frame
    protected void Update()
    {
        // the max velocity is different depending on whether the flag is being held or not.
        float speedLimit = (flag == null) ? maxVelocity : flagMaxVelocity;

        // factor by which the user is slowed. If not holding the flag, the user moves at normal speed.
        float slowedFactor = (flag == null) ? 1.0F : flagHeldFactor;

        // sets 'hasChanged' to false to check if the object's transform has been updated later.
        transform.hasChanged = false;

        // TODO: take this out; it's unnecessary for the final game, since it will always be momentum based to a point.
        if (controllablePlayer)
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
                        Vector3 force = transform.forward * movementSpeed * slowedFactor * speedMult * Time.deltaTime;
                        rigidBody.AddForce(force, ForceMode.Force);
                        // direcVec += force;
                        stateMachine.SetState(1);

                        //if (rigidBody.velocity.magnitude > 2.8)
                        //{
                        //    stateMachine.SetState(2);
                        //}

                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        Vector3 force = -transform.forward * movementSpeed * backupFactor * slowedFactor * speedMult * Time.deltaTime;
                        rigidBody.AddForce(force);
                        // direcVec += force;

                    }
                    if (Input.GetKey(KeyCode.W) && (rigidBody.velocity.magnitude >= speedLimit))
                    {
                        stateMachine.SetState(2);
                    }

                    // leftward and rightward movement
                    if (Input.GetKey(KeyCode.A)) // turn left
                    {
                        // if there is no velocity, set the player's rotation to -90 degrees.
                        if (Input.GetKey(KeyCode.W)) // if the player is going foward
                        {
                            Vector3 force = -transform.right * movementSpeed * slowedFactor * speedMult * Time.deltaTime;
                            rigidBody.AddForce(force);
                            // direcVec += force;

                            transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                        }
                        else // the player is not pushing themself forward, so rotate instead.
                        {
                            transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                        }
                    }
                    else if (Input.GetKey(KeyCode.D)) // turn right
                    {
                        // if there is no velocity, set the player's rotation to -90 degrees.
                        if (Input.GetKey(KeyCode.W)) // if the player is going foward
                        {
                            Vector3 force = transform.right * movementSpeed * slowedFactor * speedMult * Time.deltaTime;
                            rigidBody.AddForce(force);
                            // direcVec += force;

                            transform.Rotate(Vector3.up, rotSpeed.y * Time.deltaTime);
                        }
                        else // the player is not pushing themself forward, so rotate instead.
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
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.Rotate(Vector3.up, -rotSpeed.y * Time.deltaTime);
                    }
                    // rotate to the right
                    else if (Input.GetKey(KeyCode.RightArrow))
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
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        transform.Rotate(Vector3.up, 180.0F);
                    }
                }

                // jump
                {

                    // if the player is on the on the gound, and can jump.
                    // jump does NOT rely on delta time for consistency sake
                    if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && onGround)
                    {
                        rigidBody.AddForce(Vector3.up * jumpForce * jumpMult, ForceMode.Impulse);
                        onGround = false;
                    }
                }

                // caps velocity
                // TODO: this might need to be changed for the jump.
                if (rigidBody.velocity.magnitude > speedLimit)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * speedLimit;

                    // float revForce = rigidBody.velocity.magnitude - maxVelocity;
                    // rigidBody.AddForce(rigidBody.velocity.normalized * -1 * revForce);
                }

            }
            else
            {
                // forward and backward movement
                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 shift = new Vector3(0, 0, movementSpeed * slowedFactor * speedMult * Time.deltaTime);
                    transform.Translate(shift);
                    // direcVec += shift;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Vector3 shift = new Vector3(0, 0, -movementSpeed * slowedFactor * speedMult * Time.deltaTime);
                    transform.Translate(shift);
                    // direcVec += shift;
                }

                // leftward and rightward movement
                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 shift = new Vector3(-movementSpeed * slowedFactor * speedMult * Time.deltaTime, 0, 0);
                    transform.Translate(shift);
                    // direcVec += shift;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Vector3 shift = new Vector3(movementSpeed * slowedFactor * speedMult * Time.deltaTime, 0, 0);
                    transform.Translate(shift);
                    // direcVec += shift;
                }

                // upward and downward movement
                if (Input.GetKey(KeyCode.Q))
                {
                    Vector3 shift = new Vector3(0, movementSpeed * slowedFactor * speedMult * Time.deltaTime, 0);
                    transform.Translate(shift);
                    // direcVec += shift;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    Vector3 shift = new Vector3(0, -movementSpeed * slowedFactor * speedMult * Time.deltaTime, 0);
                    transform.Translate(shift);
                    // direcVec += shift;
                }

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

        // if the camera distance has changed.
        if(cameraDistance != playerCamera.distance)
        {
            playerCamera.distance = cameraDistance;
        }

        // TODO: take this out or make it more efficient.
        // player isn't moving
        if(stateMachine.state != 0 && rigidBody.velocity == new Vector3())
        {
            stateMachine.SetState(0);
        }

        // if the player has a flag, gain a point.
        if(flag != null)
        {
            playerScore += Time.deltaTime;
            playerScoreText.text = "Player Score: " + Mathf.Floor(playerScore); // "Player Score: " +
        }

        // playing idle sound
        if(idleEffectCurrTime >= idleEffectWaitTime) // wait time reached
        {
            if (sfx_Idle != null) // play effect
                sfx_Idle.Play();

            idleEffectCurrTime = 0;
        }
        else //
        {
            idleEffectCurrTime += Time.deltaTime;
        }

        // saves the player's current position
        // lastPos = transform.position;

        // Gameplay Audio 

        // if the player has stopped moving on these axes, turn off the audio.
        if ((rigidBody.velocity.x == 0 && rigidBody.velocity.z == 0) || onGround == false) // player not moving
        {
            sfx_Run.Stop();
        }
        else if (onGround) // player moving, and on ground.
        {
            if (!sfx_Run.isPlaying)
                sfx_Run.Play();
        }
    }
}