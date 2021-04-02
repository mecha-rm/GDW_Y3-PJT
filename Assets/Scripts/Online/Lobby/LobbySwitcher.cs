using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySwitcher : MonoBehaviour
{
    // host object
    public GameObject hostObject;

    // join object
    public GameObject joinObject;

    // Start is called before the first frame update
    void Start()
    {
        // looks for host object
        if (hostObject == null)
            GameObject.Find("Host");

        // looks for join object.
        if (joinObject == null)
            GameObject.Find("Host");
    }

    // swaps host and join object activities.
    public void SwitchMode()
    {
        // host is active, the join is not.
        if(hostObject.activeSelf && !joinObject.activeSelf)
        {
            hostObject.SetActive(false);
            joinObject.SetActive(true);
        }
        // host is not active, but the join is.
        else if (!hostObject.activeSelf && joinObject.activeSelf)
        {
            hostObject.SetActive(true);
            joinObject.SetActive(false);
        }
        // both are active
        else if (hostObject.activeSelf && joinObject.activeSelf)
        {
            // deactivates join object.
            joinObject.SetActive(false);
        }
        // both are in active
        else if (!hostObject.activeSelf && !joinObject.activeSelf)
        {
            // activates join object.
            hostObject.SetActive(true);
        }
    }

    // activates the host
    public void ActivateHost()
    {
        hostObject.SetActive(true);
        joinObject.SetActive(false);
    }

    // activates the join
    public void ActivateJoin()
    {
        hostObject.SetActive(false);
        joinObject.SetActive(true);
    }
}
