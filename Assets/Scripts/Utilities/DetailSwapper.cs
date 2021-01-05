using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// swaps out a model from a high poly version to a low poly version within a given distance.
public class DetailSwapper : MonoBehaviour
{
    // high detail and low detail models
    public GameObject highDetail;
    public GameObject lowDetail;

    // reference object for distance and current object.
    public GameObject referenceObject;
    public GameObject detailObject;

    // if 'true', high detail is being used.
    public bool usingHighDetail;

    // distance needed to go from high detail to low detail
    public float distance = 1000.0F;

    // global distance for all detail swappers (is a static variable)
    public static float globalDistance = 1000.0F;

    // if 'true', the global distance for the detail swapper is used instead of the local distance.
    public bool useGlobalDistance = false;

    // if 'true', the local distance variable is made into the global distance
    public bool makeGlobalDistance = false;

    // if detail swapper should be enabled
    public bool swapperEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the swapper is enabled
        if (swapperEnabled)
        {
            // if the global distance should be used
            if (makeGlobalDistance)
                globalDistance = distance;

            // checks to see if the global distance should be used
            float dist = useGlobalDistance ? globalDistance : distance;

            // distance threshold surpassed
            if (Vector3.Distance(detailObject.transform.position, referenceObject.transform.position) >= dist)
            {
                if (usingHighDetail) // currently using high detail object
                {
                    lowDetail.SetActive(true);
                    highDetail.SetActive(false);
                    usingHighDetail = false;
                }
            }
            else // below distance threshold
            {
                if (!usingHighDetail) // currently using low detail object
                {
                    lowDetail.SetActive(false);
                    highDetail.SetActive(true);
                    usingHighDetail = true;
                }
            }
        }

    }
}
