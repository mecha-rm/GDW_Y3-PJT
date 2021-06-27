using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used by the stage to know where the spawns are.
public class SpawnPoint : MonoBehaviour
{
    // gets the spawn point.
    public Vector3 GetSpawnPoint()
    {
        return transform.position;
    }

    // sets the spawn point.
    public void SetSpawnPoint(Vector3 newSpawn)
    {
        transform.position = newSpawn;
    }

    // Get X
    public float GetPositionX()
    {
        return transform.position.x;
    }

    // Set X
    public void SetPositionX(float x)
    {
        transform.position.Set(x, transform.position.y, transform.position.z);
    }

    // Get Y
    public float GetPositionY()
    {
        return transform.position.y;
    }

    // Set Y
    public void SetPositionY(float y)
    {
        transform.position.Set(transform.position.x, y, transform.position.z);
    }

    // Get Z
    public float GetPositionZ()
    {
        return transform.position.z;
    }

    // Set Z
    public void SetPositionZ(float z)
    {
        transform.position.Set(transform.position.x, transform.position.y, z);
    }

    // to string function
    public string ToString()
    {
        return transform.position.ToString();
    }

}
