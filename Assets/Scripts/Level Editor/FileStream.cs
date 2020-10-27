/*
 * References:
 * https://docs.unity3d.com/ScriptReference/Serializable.html
 * https://answers.unity.com/questions/1278885/how-to-serialize-an-object-to-a-byte-array.html
 * https://bitbucket.org/stupro_hskl_betreuung_kessler/learnit_merged_ss16/raw/e5244ebb38c8fe70759e632ea4224e48f5ca5833/Unity/LearnIT_Merged/Assets/Scripts/Util/ObjectSerializationExtension.cs
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using UnityEngine;

public class FileStream : MonoBehaviour
{
    // the list of objects that are being written to the file.
    public List<GameObject> objects;

    // uses a DLL
    const string DLL_NAME = "GED - ASN01";

    // Functions from DLL
    // Calling these cause Unity to crash.
    [DllImport(DLL_NAME)]
    private static extern bool OpenForReading([In] byte[] arr);

    [DllImport(DLL_NAME)]
    private static extern bool OpenForWriting([In] byte[] arr, bool createFile);

    [DllImport(DLL_NAME)]
    private static extern string GetFilePath();

    // [DllImport(DLL_NAME)]
    // private static extern byte[] GetFilePath();

    // Start is called before the first frame update
    void Start()
    {
        // BinaryFormatter converter = new BinaryFormatter();
        // MemoryStream mStream = new MemoryStream();
        // string file = "test.txt";
        // string f2 = "";
        // 
        // converter.Serialize(mStream, file);
        // 
        // // Test
        // OpenForWriting(mStream.ToArray(), true); // crahsed unity
        // f2 = GetFilePath();
        // Debug.Log(f2);
    }

    // adds an object to the list for saving/loading. An object cannot be put into the list twice.
    public void AddObjectToList(GameObject entity)
    {
        // an object can't be written in twice.
        foreach(GameObject obj in objects)
        {
            if (obj == entity)
                return;
        }

        objects.Add(entity);
    }

    // removes an object to the list for saving/loading. An object cannot be put into the list twice.
    public void RemoveObjectFromList(GameObject entity)
    {
        // removes the entity from the list
        objects.Remove(entity);
    }

    // checks if the object is in the list already.
    public bool ObjectInList(GameObject entity)
    {
        return objects.Contains(entity);
    }

    // converts entity to bytes
    public static byte[] ConvertObjectToBytes(GameObject entity)
    {
        BinaryFormatter converter = new BinaryFormatter();
        MemoryStream mStream = new MemoryStream();
        
        converter.Serialize(mStream, entity);
        return mStream.ToArray();
    }
    
    // convert bytes to a game object
    public static GameObject ConvertBytesToObject(byte[] data)
    {
        BinaryFormatter converter = new BinaryFormatter();
        MemoryStream mStream = new MemoryStream();

        mStream.Write(data, 0, data.Length);
        mStream.Seek(0, 0);

        GameObject entity = (GameObject)converter.Deserialize(mStream);
        return entity;
    }

    // saves all objects in the list
    public void SaveObjects()
    {
        // list of objects (converted to bytes)
        List<byte[]> arrs = new List<byte[]>();

        // converts all objects to bytes
        foreach (GameObject entity in objects)
        {
            byte[] bytes = ConvertObjectToBytes(entity);
            arrs.Add(bytes);
        }

        // SAVE DATA
    }

    // loads all objects
    public void LoadObjects()
    {
        // GET DATA FROM FILE
        // while loop
        {
            // byte[] bytes;
            // 
            // GameObject entity = ConvertBytesToObject(bytes);
        }

        // LOAD DATA
    }

    // Update is called once per frame
    void Update()
    {

    }
}
