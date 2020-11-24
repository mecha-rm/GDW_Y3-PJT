using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class LevelFileSystem : MonoBehaviour
{
	public ObjectScript temp;

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

	// insert record at index
	[DllImport(DLL_NAME)]
	private static extern void InsertRecord([MarshalAs(UnmanagedType.LPStr)] string record, int index);

	// removes the record.
	[DllImport(DLL_NAME)]
	private static extern void RemoveRecord([MarshalAs(UnmanagedType.LPStr)] string record);

	// removes record at index 
	[DllImport(DLL_NAME)]
	private static extern void RemoveRecordAtIndex(int index);

	// gets record
	[DllImport(DLL_NAME, EntryPoint = "GetRecord")]
	private static extern System.IntPtr GetRecord(int index);

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

	// imports the records from the provided file
	[DllImport(DLL_NAME)]
	private static extern int ImportRecords();

	// exports records to saved file.
	[DllImport(DLL_NAME)]
	private static extern int ExportRecords();

	// XXXXX //
	// adds record at index
	public void AddRecordToList(string record)
    {
		AddRecord(record);
    }

	// insert record at index
	public void InsertRecordIntoList(string record, int index)
    {
		InsertRecord(record, index);
    }

	// removes record
	public void RemoveRecordFromList(string record)
    {
		RemoveRecord(record);
    }

	// removes record at index 
	public void RemoveRecord(int index)
    {
		RemoveRecordAtIndex(index);
    }

	// gets record
	public string GetRecordFromList(int index)
    {
		return Marshal.PtrToStringAnsi(GetRecord(index));
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


	// converts object to bytes (requires seralizable object)
	static public byte[] SerializeObject(GameObject entity)
    {
		BinaryFormatter converter = new BinaryFormatter();
		MemoryStream mStream = new MemoryStream();

		converter.Serialize(mStream, entity);
		return mStream.ToArray();
	}


	// Start is called before the first frame update
	void Start()
    {
		// string str = (GetAsBytes()).ToString();
		// Debug.Log(str);

		string file = "Assets/Resources/Saves/level_test.txt";

		SetRecordFile(file);

		// Saving Data
		// AddRecordToList("Record1");
		// AddRecord("Record2");
		// AddRecordToList("Record3");
		// SaveRecords();

		// LoadingData
		// LoadRecords();
		// int count = GetAmountOfRecords();

		// TODO: this is NOT sending the data over properly.
		// for(int i = 0; i < count; i++)
		// {
		// 	Debug.Log("Record " + i + " : " + GetRecordFromList(i));
		// }

		// BinaryFormatter converter = new BinaryFormatter();
		// MemoryStream mStream = new MemoryStream();
		// 
		// converter.Serialize(mStream, temp);
		// byte[] arr = mStream.ToArray();
		// 
		// // byte[] arr = SerializeObject(temp.gameObject);
		// Debug.Log(arr.ToString());

	}

	// converts object to bytes (serializable objects only)
	public byte[] GetAsBytes()
    {
		BinaryFormatter converter = new BinaryFormatter();
		MemoryStream mStream = new MemoryStream();
		
		converter.Serialize(mStream, gameObject);
		return mStream.ToArray();
	}


	// Update is called once per frame
	void Update()
    {
        
    }

	// OnDestroy is called when the object is being destroyed.
    private void OnDestroy()
    {
		if(saveData)
			SaveRecords();
    }
}
