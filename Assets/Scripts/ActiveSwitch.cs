using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// switches from one active object to another.
public class ActiveSwitch : MonoBehaviour
{
    // objects to activate
    public List<GameObject> switchableObjects = new List<GameObject>();
    
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // switch object
    public void SwitchOnObject(int index)
    {
        // index out of bounds
        if (index < 0 || index >= switchableObjects.Count)
            return;

        switchableObjects[index].SetActive(true);
        gameObject.SetActive(false);
    }

    // switches on an object based on its name.
    public void SwitchOnObject(string name)
    {
        // finds active object based on its name.
        for(int i = 0; i < switchableObjects.Count; i++)
        {
            if(switchableObjects[i].name == name)
            {
                switchableObjects[i].SetActive(true);
                gameObject.SetActive(false);
                break;
            }
        }  
    }

    // switches active object.
    public void SwitchOnAllObjects()
    {
        // activates all objects
        foreach(GameObject nextObject in switchableObjects)
            nextObject.SetActive(true);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
