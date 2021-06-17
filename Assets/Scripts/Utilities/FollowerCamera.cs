using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the follower camera script.
public class FollowerCamera : MonoBehaviour
{
    // the camera object.
    public Camera cameraObject;

    // the camera distance from the target
    public Vector3 distance = new Vector3(0, 0, 0); // camera distance from player
   
    // the target the camera is following.
    public GameObject target;

    // the camera's rotation
    public Vector3 rotation = new Vector3(0, 0, 0); // the camera's default orientation

    // if 'true', the follower camera goes off of the parent rotation.
    public bool useParentRotation = true;


    // Start is called before the first frame update
    void Start()
    {
        // if the camera object has not been set.
        if (cameraObject == null)
            cameraObject = GetComponent<Camera>();


        // if a target has been set, position properly now.
        if (target != null)
        {
            transform.position = target.transform.position + distance;
            transform.LookAt(target.transform);
        }
    }

    // searches for the camera component
    public Camera GetCamera()
    {
        // if the camera object is null, search for it.
        if(cameraObject == null)
        {
            cameraObject = GetComponent<Camera>();
        }

        // returns the camera object
        return cameraObject;
    }

    // rotates the camera
    public void Rotate(float x, float y, float z)
    {
        Rotate(new Vector3(x, y, z));
    }

    // rotates the camera
    public void Rotate(Vector3 rot)
    {
        rotation += rot;
    }

    // sets the camera's rotation
    public void SetRotation(float x, float y, float z)
    {
        SetRotation(new Vector3(x, y, z));
    }

    // sets the camera rotation
    public void SetRotation(Vector3 newRot)
    {
        rotation = newRot;
    }

    // Update is called once per frame
    void Update()
    {
        // vector for world origin (0, 0, 0)
        
        // searches for player
        if(target == null)
        {
            PlayerObject[] plyrs = FindObjectsOfType<PlayerObject>();
            PlayerObject plyrMissingCam = null;
            bool attached = false;

            for(int i = 0; i < plyrs.Length; i++)
            {
                // player's camera is not null
                if(plyrs[i].playerCamera == this)
                {
                    // if they have no target, give it this camera.
                    if(plyrs[i].playerCamera.target == null)
                    {
                        target = plyrs[i].gameObject;
                        plyrs[i].playerCamera.target = plyrs[i].gameObject;
                        attached = true;
                        break;
                    }    
                }
                else if(plyrs[i].playerCamera == null) // no camera set.
                {
                    plyrMissingCam = plyrs[i];
                }
            }

            // gives player with no camera, this camera.
            if (attached == false && plyrMissingCam != null)
            {
                plyrMissingCam.playerCamera = this;
                attached = true;
            }

            // destroy self if no game object is found.
            if (attached == false)
            {
                // destory self.
                Destroy(gameObject);
            }

        }

        // saves the original position and rotation
        Vector3 origPos = transform.position;
        Quaternion origRot = transform.rotation;

        // bring the camera to the world origin for rotation
        transform.position = distance;
        transform.rotation = new Quaternion(0, 0, 0, 1);

        {
            Vector3 worldOrigin = new Vector3();
            Vector3 newRot = rotation;

            if (useParentRotation && target != null)
                newRot = target.transform.rotation.eulerAngles + newRot;

            // rotations
            transform.RotateAround(worldOrigin, Vector3.right, newRot.x); // x-axis
            transform.RotateAround(worldOrigin, Vector3.up, newRot.y); // y-axis
            transform.RotateAround(worldOrigin, Vector3.forward, newRot.z); // z-axis
        }

        // gets the new position offset from the rotations.
        Vector3 offset = transform.position;

        // returns values back to originals
        transform.position = origPos;
        transform.rotation = origRot;

        // new position. New rotation is set by looking at the target.
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform);
    }

    // destroyed
    // private void OnDestroy()
    // {
    //     Debug.Log("Follower Camera Destroyed");
    // }
}
