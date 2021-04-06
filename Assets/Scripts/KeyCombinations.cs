using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for key combinations
public class KeyCombinations : MonoBehaviour
{
    // the debug combination
    public bool debugScene_Combo = true;

    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        // debug key combination
        // CTRL + SHIFT + D
        if(debugScene_Combo)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                        Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift) &&
                        Input.GetKey(KeyCode.D))
            {
                // change to the debug scene.
                SceneChanger.ChangeScene("DebugScene");
            }
        }
        
    }
}
