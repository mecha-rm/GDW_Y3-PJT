using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// saves and loads objects
public class LevelLoader : MonoBehaviour
{
    // all assets to be saved (or loaded in)
    public string filePath = "Assets/Resources/Saves/"; // file path (from highest directory in Unity folder)
    public string file = "unnamed.dat"; // file (defaults to .txt if not stated)
    public GameObject parent = null;
    public List<GameObject> objects = new List<GameObject>(); // needed to be initialized for some reason
    
    // if 'true', the children are added.
    public bool addChildrenOnSave = true;
    // if 'true', objects are loaded as children of the parent game object.
    public bool loadAsChildren = true;


    // Start is called before the first frame update
    void Start()
    {
        // if the parent is null...
        // then its set to the game object this component is attached to.
        if (parent == null)
            parent = gameObject;

        if(addChildrenOnSave)
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
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            // adds all children that aren't already in the list.
            if (!objects.Contains(parent.transform.GetChild(i).gameObject))
                objects.Add(parent.transform.GetChild(i).gameObject);
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
        DataManager fileStream = new DataManager();
        string fullFile;

        fileStream.ClearAllDataRecordsFromManager(); // clear existing content

        // if children should be added, its updated before saving.
        if (addChildrenOnSave)
            AddChildrenToList();

        // set record file and save
        fullFile = GetFileWithPath();
        fileStream.SetDataFile(fullFile);

        // adds all elements in the list.
        foreach (GameObject element in objects)
        {
            SerializedObject entity = SerializableObject.Pack(element);
            byte[] data = StringRecordManager.SerializeObject(entity);

            // adds data
            fileStream.AddDataRecordToManager(data);
        }

        fileStream.SaveDataRecords();

        // if the save record didn't create or edit a file, the name must be invalid.
        if(!fileStream.FileAvailable())
        {
            Debug.Log("File could not be found, or opened.");
            return;
        }

        //  clear ocntent so it can be reused.
        fileStream.ClearAllDataRecordsFromManager();
        Destroy(fileStream);

        Debug.Log("Save Successful!");
    }


    // load objects
    public void LoadFromFile()
    {

        // file stream object.
        DataManager fileStream = new DataManager();
        int count = 0;
        string fullFile = "";

        // loads content
        fileStream.ClearAllDataRecordsFromManager(); // clear existing content
        fullFile = GetFileWithPath();

        // gets data
        fileStream.SetDataFile(fullFile);

        // if the record file is not available
        if (!fileStream.FileAvailable())
        {
            Debug.LogError(fullFile + " not found.");
            return;
        }

        // loads records and gets count
        fileStream.LoadDataRecords();
        count = fileStream.GetDataRecordAmount();

        // loads children
        bool childLoad = (loadAsChildren && parent != null);

        // grabs all items, with each record representing one item
        for (int i = 0; i < count; i++)
        {
            GameObject newObject = null;
            byte[] byteData = fileStream.GetDataFromManager(i);
            object objectData = StringRecordManager.DeserializeObject(byteData);
            SerializedObject serialData = (SerializedObject)(objectData);

            newObject = SerializableObject.Unpack(serialData);

            // if the objects should be loaded as children of the current object.
            if (childLoad)
                newObject.transform.parent = parent.transform;

            // if a new object was generated, add it to the list.
            if (newObject != null)
                objects.Add(newObject);
        }

        fileStream.ClearAllDataRecordsFromManager();
        Destroy(fileStream);

        Debug.Log("Load Successful!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
