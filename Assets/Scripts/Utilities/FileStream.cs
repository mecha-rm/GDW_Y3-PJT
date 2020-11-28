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

// puts lines into a file
public class FileStream : MonoBehaviour
{
    // the file for the metrics logger. Make sure to include the file path from highest hierachy.
    public string file = "";

    // if 'true', data is loaded.
    public bool loadData = false;

    // if 'true', data is saved upon destruction.
    public bool saveData = true;

    // the DLL
    private const string DLL_NAME = "GDW_Y3-RecordSystem";

    // adds record at index
    [DllImport(DLL_NAME)]
    private static extern void AddRecord([MarshalAs(UnmanagedType.LPStr)] string record);

    // adds a record in bytes
    [DllImport(DLL_NAME)]
    private static extern void AddRecordInBytes(byte[] arr);

    // insert record at index
    [DllImport(DLL_NAME)]
    private static extern void InsertRecord([MarshalAs(UnmanagedType.LPStr)] string record, int index);

    // insert a record in bytes
    [DllImport(DLL_NAME)]
    private static extern void InsertRecordInBytes(byte[] arr, int index);

    // removes the record.
    [DllImport(DLL_NAME)]
    private static extern void RemoveRecord([MarshalAs(UnmanagedType.LPStr)] string record);

    // remove a record in bytes
    [DllImport(DLL_NAME)]
    private static extern void RemoveRecordInBytes(byte[] arr);

    // removes record at index 
    [DllImport(DLL_NAME)]
    private static extern void RemoveRecordAtIndex(int index);

    // gets record
    [DllImport(DLL_NAME, EntryPoint = "GetRecord")]
    private static extern System.IntPtr GetRecord(int index);

    // gets the record in byte form.
    [DllImport(DLL_NAME)]
    private static extern void GetRecordInBytes(int index, byte[] arr, int size);

    // gets the length of a record.
    [DllImport(DLL_NAME)]
    private static extern int GetRecordSize(int index);

    // gets the amount of records
    [DllImport(DLL_NAME)]
    private static extern int GetRecordCount();

    // returns (1) if contains record, (0) if record is not in list.
    [DllImport(DLL_NAME)]
    private static extern int ContainsRecord([MarshalAs(UnmanagedType.LPStr)] string record);

    // clears out all records
    [DllImport(DLL_NAME)]
    private static extern void ClearAllRecords();

    // gets the file tied to this record system.
    [DllImport(DLL_NAME)]
    private static extern System.IntPtr GetFile();

    // sets the file for this file system
    [DllImport(DLL_NAME)]
    private static extern void SetFile([MarshalAs(UnmanagedType.LPStr)] string file);

    // returns (1) if the set file is accessible, (0) if the set file is not accessible.
    [DllImport(DLL_NAME)]
    private static extern int FileAccessible();

    // imports the records from the provided file
    [DllImport(DLL_NAME)]
    private static extern int ImportRecords();

    // exports records to saved file.
    [DllImport(DLL_NAME)]
    private static extern int ExportRecords();

    // adds record at index
    public void AddRecordToList(string record)
    {
        AddRecord(record);
    }

    // adds a record to the list in bytes
    public void AddRecordToList(byte[] data)
    {
        AddRecordInBytes(data);
    }

    // insert record at index
    public void InsertRecordIntoList(string record, int index)
    {
        InsertRecord(record, index);
    }

    // insert record at provided index in bytes
    public void InsertRecordIntoList(byte[] data, int index)
    {
        InsertRecordInBytes(data, index);
    }

    // removes record
    public void RemoveRecordFromList(string record)
    {
        RemoveRecord(record);
    }

    // removes record in bytes
    public void RemoveRecordFromList(byte[] data)
    {
        RemoveRecordInBytes(data);
    }

    // removes record at index 
    public void RemoveRecordFromListAtIndex(int index)
    {
        RemoveRecordAtIndex(index);
    }

    // gets record
    public string GetRecordFromList(int index)
    {
        return Marshal.PtrToStringAnsi(GetRecord(index));
    }

    // returns record in bytes
    public byte[] GetRecordFromListInBytes(int index)
    {
        byte[] data = null;
        int size = GetRecordSize(index);

        if(size > 0)
        {
            data = new byte[size];
            GetRecordInBytes(index, data, size);
            return data;
        }
        else
        {
            return null;
        }
    }

    // gets the size of a record
    public int GetRecordLength(int index)
    {
        return GetRecordSize(index);
    }

    // gets the amount of records
    public int GetAmountOfRecords()
    {
        return GetRecordCount();
    }

    // returns (1) if contains record, (0) if record is not in list.
    public bool ListContainsRecord(string record)
    {
        int res = ContainsRecord(record);
        return (res == 0) ? false : true;
    }

    // clears out all records
    public void ClearAllRecordsFromList()
    {
        ClearAllRecords();
    }

    // gets save file path
    public string GetRecordFile()
    {
        return Marshal.PtrToStringAnsi(GetFile());
    }

    // sets the file
    public void SetRecordFile(string file)
    {
        SetFile(file);
    }

    // checks to see if the set file is available.
    public bool RecordFileAvailable()
    {
        int res = FileAccessible();
        return (res == 0) ? false : true;
    }

    // imports records from set file.
    public bool LoadRecords()
    {
        int res = ImportRecords();
        return (res == 0) ? false : true;
    }

    // exports records to saved file.
    public bool SaveRecords()
    {
        int res = ExportRecords();
        return (res == 0) ? false : true;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        // sets file
        SetRecordFile(file);

        // if data should be loaded.
        if (loadData)
            LoadRecords();
    }

    // converts object to bytes (requires seralizable object)
    static public byte[] SerializeObject(object entity)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        bf.Serialize(ms, entity);
        return ms.ToArray();
    }

    // deserialize provided object
    // bytes in C# is the equivalent of chars in C++.
    static public object DeserializeObject(byte[] data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        ms.Write(data, 0, data.Length); // write data
        ms.Seek(0, 0); // return to start

        return bf.Deserialize(ms); // return content
    }

    // CONVERSIONS //
    // convert char array to string
    static public byte[] ConvertCharArrayToBytes(char[] chars)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(chars);
        return data;
    }

    // convert string to bytes
    static public byte[] ConvertStringToBytes(string str)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(str);
        return data;
    }

    // converts bytes to char array
    static public char[] ConvertBytesToCharArray(byte[] data)
    {
        char[] chars = System.Text.Encoding.ASCII.GetChars(data);
        return chars;
    }

    // convert byte array to string
    static public string ConvertBytesToString(byte[] data)
    {
        string str = System.Text.Encoding.ASCII.GetString(data);
        return str;
    }

    // converts char array to string
    static public char[] ConvertStringToCharArray(string str)
    {
        return str.ToCharArray();
    }

    // converts char array to string
    static public string ConvertCharArrayToString(char[] chs)
    {
        string str = new string(chs);
        return str;
    }


    // Overloaded Functions //
    // no longer needed
    // adds a record as a string
    // public void AddRecordToList(byte[] data)
    // {
    //     string str = ConvertBytesToString(data);
    //     AddRecord(str);
    // }
    // 
    // // insert record at index
    // public void InsertRecordIntoList(byte[] record, int index)
    // {
    //     string str = ConvertBytesToString(record);
    //     InsertRecord(str, index);
    // }
    // 
    // // removes a record (converts to string to check)
    // public void RemoveRecordFromList(byte[] data)
    // {
    //     string str = ConvertBytesToString(data);
    //     RemoveRecord(str);
    // }

    // gets the record from the list as bytes
    public byte[] GetRecordFromListAsBytes(int index)
    {
        return ConvertStringToBytes(GetRecordFromList(index));
    }

    // returns (1) if contains record, (0) if record is not in list.
    public bool ListContainsRecord(byte[] data)
    {
        string str = ConvertBytesToString(data);
        return ListContainsRecord(str);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnDestroy is called when the object is being destroyed.
    private void OnDestroy()
    {
        // if data should be saved.
        if (saveData)
            SaveRecords();
    }
}
