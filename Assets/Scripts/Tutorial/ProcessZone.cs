using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// everything within this zone is processed by the game.
public class ProcessZone : MonoBehaviour
{
    // the centre of the process zone.
    public GameObject centre;
    
    // radius of process zone.
    public float radius = 1000.0F;

    // Start is called before the first frame update
    void Start()
    {
        // if no centre is set, the process zone is set to the game object it's attached to.
        if (centre == null)
            centre = gameObject;
    }

    // checks if an entity is in the zone orn ot.
    // checkX, checkY, and checkZ are used to determine if those axes should be checked or not.
    // if no centre is set, then it returns 'true'
    public bool InZone(Vector3 position, bool checkX = true, bool checkY = true, bool checkZ = true)
    {
        if (centre == null)
            return true;

        Vector3 posA = centre.transform.position;
        Vector3 posB;
        float distAB;

        // checks to see which axes are relevant to the check
        posB.x = (checkX) ? position.x : posA.x;
        posB.y = (checkY) ? position.y : posA.y;
        posB.z = (checkZ) ? position.z : posA.z;

        // checks the distance between the two position
        distAB = Vector3.Distance(posA, posB);

        // process zone check
        return distAB <= radius;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
