using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadManager : MonoBehaviour
{
    // all assets to be saved (or loaded in)
    string fileName = "";
    public List<GameObject> objects;
    public bool useChildren = true;

    // Start is called before the first frame update
    void Start()
    {
        if(useChildren)
        {
            LoadChildrenIntoList();
        }
    }
    
    // adds an object to the list
    public void AddObjectToList(GameObject newObject)
    {
        if(!objects.Contains(newObject))
            objects.Add(newObject);

    }

    // removes an object to the list
    public void RemoveObjectFromList(GameObject newObject)
    {
        objects.Remove(newObject);
    }

    // removes an object to the list
    public void RemoveObjectFromList(int index)
    {
        objects.RemoveAt(index);
    }

    // returns 'true' if list contains object
    public bool ListContainsObject(GameObject gameObject)
    {
        return objects.Contains(gameObject);
    }

    // returns the object count
    public int ObjectCount()
    {
        return objects.Count;
    }

    // loads all children into the list.
    public void LoadChildrenIntoList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // adds all children
            objects.Add(transform.GetChild(i).gameObject);
        }
    }

    // clears the list of objects
    public void ClearList()
    {
        objects.Clear();
    }


    // TODO:
    // load objects
    // save objects

    // Update is called once per frame
    void Update()
    {
        
    }
}
