using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpace : MonoBehaviour
{
    // the death plane position
    public Vector3 position = new Vector3(0.0F, -25.0F, 0.0F);

    // set to 'true' to find out what asset to use.
    public bool useX = false, useY = true, useZ = false;

    // if true, the 'death area' is less than the given position value of the given axis.
    // this determines what is considered 'behind' for the death plane
    public bool lessThanX = true, lessThanY = true, lessThanZ = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // returns 'true' if the object is past the death plane on any axis.
    public bool InDeathSpace(Vector3 objectPos)
    {
        bool death = false;

        // x-axis should be used.
        if(useX)
        {
            // checks to see if what direction is considered 'behind' the death plane.
            if (lessThanX)
                death = objectPos.x <= position.x;
            else
                death = objectPos.x >= position.x;

            if (death) // the death plane has been triggered
                return true;
        }

        // y-axis should be used.
        if(useY)
        {
            // checks to see if what direction is considered 'behind' the death plane.
            if (lessThanY)
                death = objectPos.y <= position.y;
            else
                death = objectPos.y >= position.y;

            if (death) // the death plane has been triggered
                return true;
        }

        // z-axis should be used.
        if(useZ)
        {
            // checks to see if what direction is considered 'behind' the death plane.
            if (lessThanZ)
                death = objectPos.z <= position.z;
            else
                death = objectPos.z >= position.z;

            if (death) // the death plane has been triggered
                return true;
        }

        // no collision has occurred.
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
