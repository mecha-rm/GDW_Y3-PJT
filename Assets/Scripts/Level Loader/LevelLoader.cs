using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // the parent of the loaded content
    public GameObject parent;

    // list of objects
    public List<GameObject> objects;

    // loaded children
    public bool loadAsChildren = true;

    // Start is called before the first frame update
    void Start()
    {
        // if the parent is null, then its set to the attached object.
        if (parent == null)
            parent = gameObject;
    }

    // deletes all content
    public void DeleteContents()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }

        objects.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
