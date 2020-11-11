using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for moving platform
// if you don't want the platform to tilt, then freeze the rotation.
// if you don't want the platform to sink, increase its mass so that objects can't weigh it down.
// these are all handled in the rigidbody.
public class PlatformMovement : MonoBehaviour
{
    // the rigid body of the platform. This will automatically be filled if not provided.
    private Rigidbody rigidBody;

    // the force applied to move the platform.
    public float force = 1.0F;

    // the points of travel for the platform
    public List<Vector3> travelPoints;

    // the index of the start point.
    // when the platform reaches its destination, that destination becomes the start point.
    public int destIndex = 0;

    // if 'true', the object's starting point is added as a destination
    public bool addStartPos = true;

    // if 'true', the platform stops moving.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        // gets the rigid body
        rigidBody = GetComponent<Rigidbody>();

        // gets the rigid body if it has not been set, it automatically adds one.
        if (rigidBody == null)
            rigidBody = gameObject.AddComponent<Rigidbody>();

        // adds starting position as destination.
        // doing so will also increase the index to 1 so that 
        if(addStartPos)
        {
            // adds the travel point as the first position.
            travelPoints.Insert(0, transform.position);
        }
    }

    // adds a travel point
    public void AddTravelPoint(Vector3 tpoint)
    {
        travelPoints.Add(tpoint);
    }

    // adds a travel point at the provided index.
    // if the index is negative, it's put at the start of the list.
    // if the index is positive, it's put at the end of the list.
    public void AddTravelPoint(Vector3 tpoint, int insertionIndex)
    {
        travelPoints.Insert(Mathf.Clamp(insertionIndex, 0, travelPoints.Count), tpoint);
    }

    // removes the first instance of this travel point
    public void RemoveTravelPoint(Vector3 tpoint)
    {
        travelPoints.Remove(tpoint);
    }

    // removes a travel point at its provided index.
    // if the index is out of bounds, no removal occurs.
    public void RemoveTravelPoint(int index)
    {
        // if the index is within the bounds, remove the value.
        if (index >= 0 && index < travelPoints.Count)
            travelPoints.RemoveAt(index);
        else
            return;
    }

    // returns the current destination index
    public int GetCurrentDestinationIndex()
    {
        return destIndex;
    }

    // returns current destination
    // if out of bounds, then the object's position will be returned.
    public Vector3 GetCurrentDestination()
    {
        if (destIndex >= 0 && destIndex < travelPoints.Count)
            return travelPoints[destIndex];
        else
            return transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        // if there are travel points
        if(!paused && travelPoints.Count != 0)
        {
            // puts the destination index within the proper bounds
            destIndex = Mathf.Clamp(destIndex, 0, travelPoints.Count - 1);

            // travel point, and direction vector, normalized
            Vector3 destination = travelPoints[destIndex];
            Vector3 direcVec = destination - transform.position;

            // adds force to the rigid body
            rigidBody.AddForce(direcVec.normalized * force * Time.deltaTime, ForceMode.Acceleration);

            // checks to see if the current position has passed on all axes.
            if(
                transform.position.x >= destination.x &&
                transform.position.y >= destination.y &&
                transform.position.z >= destination.z
            )
            {
                // goes onto the next destination
                // if this goes out of bounds, it will be corrected on the next update.
                destIndex++;
            }


        }
    }
}
