using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: implement interpolation for multiple objects along the same path.
public class InterpolationMultiple : Interpolation // MonoBehaviour
{
    // interpolation type
    public interType mode;

    // list of objects
    public List<InterpolationMember> objectList = new List<InterpolationMember>();

    // list of travel points
    public List<Vector3> travelPoints = new List<Vector3>();

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
        InterpolationMember comp = newObject.GetComponent<InterpolationMember>();

        // component doesn't exist.
        if (comp == null)
            comp = newObject.AddComponent<InterpolationMember>();

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
        for(int i = 0; i < objectList.Count; i++)
        {
            // game object has been found.
            if(objectList[i].gameObject == entity)
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
            foreach (InterpolationMember member in objectList)
            {
                // member shouldn't be paused.
                if (member.paused)
                    continue;

                // clamps t value
                member.t += Time.deltaTime;
                member.t = Mathf.Clamp(member.t, 0.0F, 1.0F);

                // clamps index
                member.index = Mathf.Clamp(member.index, 0, travelPoints.Count - 1);

                // gets new position
                Vector3 newPos = InterpolateByType(mode, member.startPos, travelPoints[member.index], member.t);
                transform.position = newPos;

                // end of path reached
                if (member.t >= 1.0F)
                {
                    member.index++;
                    member.t = 0.0F;
                }

                // list has been gone through, so go back to the beginning.
                if (member.index >= travelPoints.Count)
                {
                    member.index = 0;
                }
            }
        }
    }
}
