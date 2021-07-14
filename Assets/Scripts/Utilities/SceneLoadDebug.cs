using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// prints name of active scene when it is switched to.
public class SceneLoadDebug : MonoBehaviour
{
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Scene: " + SceneManager.GetActiveScene().name);
    }
}
