using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// saves and loads objects
public class LevelLoadManager : MonoBehaviour
{
    // all assets to be saved (or loaded in)
    public string filePath = "Assets/Resources/Saves/"; // file path (from highest directory in Unity folder)
    public string file = ""; // file (defaults to .txt if not stated)
    public List<GameObject> objects;
    public bool addChildren = true;

    // Start is called before the first frame update
    void Start()
    {
        if(addChildren)
        {
            AddChildrenToList();
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
    public void AddChildrenToList()
    {
        // TODO: should add children effect this?

        // adds all children not already in the list
        for (int i = 0; i < transform.childCount; i++)
        {
            // adds all children that aren't already in the list.
            if (!objects.Contains(transform.GetChild(i).gameObject))
                objects.Add(transform.GetChild(i).gameObject);
        }
    }

    // clears the list of objects
    public void ClearList()
    {
        objects.Clear();
    }

    // destroys all objects
    public void DestroyAllObjects()
    {
        for(int i = 0; i < objects.Count; i++)
            Destroy(objects[i]);

        objects.Clear();
    }

    // returns the file with its file path
    private string GetFileWithPath()
    {
        string str1 = filePath;
        string str2 = file;
        string strx = "";
        
        // if the file path isn't empty
        if(filePath.Length != 0)
        {
            // if there isn't a slash at the end, add one.
            if (filePath[filePath.Length - 1] != '/')
                str1 += "/";
        }

        // if the file does not contain a file extension
        // if(!str2.Contains("."))
        // {
        //     str2 += ".txt";
        // }

        // combines strings
        strx = str1 + str2;
        return (strx);
    }

    // save objects
    public void SaveToFile()
    {
        // file stream
        FileStream fileStream = new FileStream();
        string fullFile = "";

        // if children should be added, its updated before saving.
        if (addChildren)
            AddChildrenToList();

        // adds all elements in the list.
        foreach (GameObject element in objects)
        {
            byte[] data = SerializableObject.PackToBytes(element, element.GetType());
            fileStream.AddRecordToList(data);
        }

        // set record file and save
        fullFile = GetFileWithPath();
        fileStream.SetRecordFile(fullFile);
        fileStream.SaveRecords();

        Destroy(fileStream);
    }


    // load objects
    public void LoadFromFile()
    {
        // file stream
        FileStream fileStream = new FileStream();
        int count = 0;
        string fullFile = "";

        // loads content
        fullFile = GetFileWithPath();
        fileStream.SetRecordFile(fullFile);
        fileStream.LoadRecords();

        // gets the count of records
        count = fileStream.GetAmountOfRecords();

        // grabs all items
        for(int i = 0; i < count; i++)
        {
            byte[] data = fileStream.GetRecordFromListAsBytes(i);
            GameObject newObject = SerializableObject.UnpackFromBytes(data);

            // if a new object was generated, add it to the list.
            if (newObject != null)
                objects.Add(newObject);
        }

        Destroy(fileStream);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
