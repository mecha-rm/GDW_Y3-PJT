using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// saves and loads objects
public class LevelLoader : MonoBehaviour
{
    // all assets to be saved (or loaded in)
    public string filePath = "Assets/Resources/Saves/"; // file path (from highest directory in Unity folder)
    public string file = "unnamed.txt"; // file (defaults to .txt if not stated)
    public GameObject parent = null;
    public List<GameObject> objects = new List<GameObject>(); // needed to be initialized for some reason
    
    // if 'true', the children are added.
    public bool addChildrenOnSave = true;
    // if 'true', objects are loaded as children of the parent game object.
    public bool loadAsChildren = true;

    // the limit of the amount of data per line
    // one file is used to store the size of the data, the other is used to store hte data itself.
    public bool ApplyMaxSectionSize = false;
    public int maxSectionSize = 512; // 8 * 64 NOTE: arrays copy in Int64

    const string SECOND_FILE_EXT = "rri"; // the file extension for section limit files
    const string SEC_SIZE_SYMBOL = "S"; // letter representing the maximum length of each record.
    const string REC_COUNT_SYMBOL = "R"; // letter representing the size of a record


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
        FileStream fileStream = new FileStream();
        string fullFile;
        List<int> sections = new List<int>();

        fileStream.ClearAllRecordsFromList(); // clear existing content

        // if children should be added, its updated before saving.
        if (addChildrenOnSave)
            AddChildrenToList();

        // set record file and save
        fullFile = GetFileWithPath();
        fileStream.SetRecordFile(fullFile);

        // adds all elements in the list.
        foreach (GameObject element in objects)
        {
            SerializedObject entity = SerializableObject.Pack(element);
            byte[] data = FileStream.SerializeObject(entity);

            // if the section limiter is on, the data is split into individual records.
            if (ApplyMaxSectionSize)
                fileStream.SplitAndAddData(data, maxSectionSize);
            else
                fileStream.AddRecordToList(data);

            // saves the amount of records
            // TODO: maybe have this odne only if splititng data.
            sections.Add((int)Mathf.Ceil((float)data.Length / maxSectionSize));
        }

        fileStream.SaveRecords();

        // if the save record didn't create or edit a file, the name must be invalid.
        if(!fileStream.RecordFileAvailable())
        {
            Debug.Log("File could not be found, or opened.");
            return;
        }

        //  clear ocntent so it can be reused.
        fileStream.ClearAllRecordsFromList();

        // if the section limit should be used.
        if(ApplyMaxSectionSize && fullFile.Contains("."))
        {
            // string secondFile = fullFile.Substring(0, fullFile.IndexOf("."));
            string secondFile = fullFile; // just double up the file extension instead
            secondFile += "." + SECOND_FILE_EXT;

            fileStream.SetRecordFile(secondFile); // sets file
            fileStream.AddRecordToList(SEC_SIZE_SYMBOL + " " + maxSectionSize);

            // adds records
            for(int i = 0; i < sections.Count; i++)
            {
                fileStream.AddRecordToList(REC_COUNT_SYMBOL + " " + i + " " + sections[i]);
            }

            // saves records
            fileStream.SaveRecords();
        }

        fileStream.ClearAllRecordsFromList();
        Destroy(fileStream);
        Debug.Log("Save Successful!");
    }


    // load objects
    public void LoadFromFile()
    {

        // file stream object.
        FileStream fileStream = new FileStream();
        int count = 0;
        string fullFile = "";

        List<int> recordCounts = new List<int>(); // the amount of records per object (index)
        bool recordLimits = ApplyMaxSectionSize;

        // loads content
        fileStream.ClearAllRecordsFromList(); // clear existing content
        fullFile = GetFileWithPath();

        // loads record count data
        string secondFile = fullFile + "." + SECOND_FILE_EXT; // gets the second file
        fileStream.SetRecordFile(secondFile);
        
        // gets the record limits
        recordLimits = fileStream.RecordFileAvailable();
        
        if(recordLimits) // there were record limits
        {
            fileStream.LoadRecords();

            // the total amount of records
            int secRecordCount = fileStream.GetAmountOfRecords();
        
            // gets the amount of records
            for (int i = 0; i < secRecordCount; i++)
            {
                string str = fileStream.GetRecordFromList(i); // gets the record
        
                if(str.Contains(SEC_SIZE_SYMBOL)) // contains section size symbol
                {
                    // currently does nothing
                }
                else if(str.Contains(REC_COUNT_SYMBOL)) // contains record count symbol
                {
                    // gets spaces
                    int space1 = str.IndexOf(" ");
                    int space2 = str.LastIndexOf(" ");
        
                    // parses data
                    int index = int.Parse(str.Substring(space1 + 1, space2 - space1 - 1));
                    int val = int.Parse(str.Substring(space2 + 1));
        
        
                    // checks to see if this record is the last record.
                    if (recordCounts.Count == index) // last value
                        recordCounts.Add(val);
                    else // not last value
                        recordCounts.Insert(index, val);
                }
            }
        }

        // clears out records
        fileStream.ClearAllRecordsFromList();

        // gets data
        fileStream.SetRecordFile(fullFile);

        // if the record file is not available
        if (!fileStream.RecordFileAvailable())
        {
            Debug.LogError(fullFile + " not found.");
            return;
        }

        // loads records and gets count
        fileStream.LoadRecords();
        count = fileStream.GetAmountOfRecords();

        // if there are record limits
        if(recordLimits)
        {
            int currRecord = 0;
            // List<byte[]> loadedObjects = new List<byte[]>(); // loaded objects

            // goes through all record counts
            for (int i = 0; i < recordCounts.Count; i++)
            {               
                byte[] data = fileStream.GetAndCombineData(currRecord, recordCounts[i]);
                currRecord += recordCounts[i];
                // loadedObjects.Add(data); // adds data to list

                // data exists
                if(data.Length > 0)
                {
                    GameObject newObject;
                    object objectData = FileStream.DeserializeObject(data);
                    SerializedObject serialData = (SerializedObject)(objectData);

                    newObject = SerializableObject.Unpack(serialData);

                    // if the objects should be loaded as children of the current object.
                    // TODO: maybe change this so you don't get a parent of a parent?
                    if (loadAsChildren)
                        newObject.transform.parent = parent.transform;

                    // if a new object was generated, add it to the list.
                    if (newObject != null)
                        objects.Add(newObject);
                }
            }
        }
        else // no record limits
        {
            // loads children
            bool childLoad = (loadAsChildren && parent != null);

            // grabs all items, with each record representing one item
            for (int i = 0; i < count; i++)
            {
                GameObject newObject = null;
                byte[] byteData = fileStream.GetRecordFromListInBytes(i);
                object objectData = FileStream.DeserializeObject(byteData);
                SerializedObject serialData = (SerializedObject)(objectData);

                newObject = SerializableObject.Unpack(serialData);

                // if the objects should be loaded as children of the current object.
                if (childLoad)
                    newObject.transform.parent = parent.transform;

                // if a new object was generated, add it to the list.
                if (newObject != null)
                    objects.Add(newObject);
            }
        }

        fileStream.ClearAllRecordsFromList();
        Destroy(fileStream);

        Debug.Log("Load Successful!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
