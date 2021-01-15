using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyLerpMultiple : MonoBehaviour
{
    // list of objects
    public List<RigidbodyLerpMember> objectList = new List<RigidbodyLerpMember>();

    // list of travel points
    public List<Vector3> travelPoints = new List<Vector3>();

    // master force for the platform chain
    public float masterForce = 1.0F;

    // if 'true', the master force overrides the individual forces.
    public bool useMasterForce = true;

    // becomes 'true' if the interpolation should be paused.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // checks to see if the path is paused.
    public bool IsPaused()
    {
        return paused;
    }

    // sets whether the process should be paused or not.
    public void SetPause(bool pause)
    {
        paused = pause;
    }

    // adds a game object to the list
    public void AddGameObject(GameObject newObject)
    {
        // checks for null object
        if (newObject == null)
            return;

        // gets (or adds) the component.
        RigidbodyLerpMember comp = newObject.GetComponent<RigidbodyLerpMember>();

        // component doesn't exist.
        if (comp == null)
            comp = newObject.AddComponent<RigidbodyLerpMember>();

        // adds component to the list.
        objectList.Add(comp);
    }

    // removs provided game object
    public void RemoveGameObject(GameObject entity)
    {
        // checks for null object
        if (entity == null)
            return;

        // index of game object
        int index = -1;

        // finds the index of the entity
        for (int i = 0; i < objectList.Count; i++)
        {
            // game object has been found.
            if (objectList[i].gameObject == entity)
            {
                index = i;
                break;
            }
        }

        // removes the object at the index if the index is valid.
        if (index >= 0 && index < objectList.Count)
            objectList.RemoveAt(index);
    }

    // Update is called once per frame
    void Update()
    {
        // the process is not paused
        if (!paused)
        {
            // processes each member.
            foreach (RigidbodyLerpMember member in objectList)
            {
                // member shouldn't be paused.
                if (member.paused)
                    continue;

                // puts the destination index within the proper bounds
                member.destIndex = Mathf.Clamp(member.destIndex, 0, travelPoints.Count - 1);

                // travel point, and direction vector, normalized
                Vector3 destination = travelPoints[member.destIndex];
                Vector3 direcVec = destination - member.transform.position;

                // adds force to the rigid body
                // if 'useMasterForce' is enabled, all objects move at the same speed.
                if(useMasterForce)
                    member.rigidBody.AddForce(direcVec.normalized * masterForce * Time.deltaTime, ForceMode.Acceleration);
                else
                    member.rigidBody.AddForce(direcVec.normalized * member.force * Time.deltaTime, ForceMode.Acceleration);

                // checks to see if the current position has passed on all axes.
                if (
                    member.transform.position.x >= destination.x &&
                    member.transform.position.y >= destination.y &&
                    member.transform.position.z >= destination.z
                )
                {
                    // goes onto the next destination
                    // if this goes out of bounds, it will be corrected on the next update.
                    member.destIndex++;
                }
            }
        }
    }
}
