using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


// this script is purely for testing out content
public class TesterScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
    {
		FileStream fs = new FileStream();
		string file = "Assets/Resources/Saves/record_test.txt";

		fs.ClearAllRecordsFromList();

		fs.AddRecordToList("record1");
		fs.AddRecordToList("record2");
		fs.AddRecordToList("record3");

        {
			// float x = 2.0f;
			Vec3 x;
			x.x = 10;
			x.y = 2;
			x.z = -1;

			byte[] data = FileStream.SerializeObject(x);
			fs.AddRecordToList(data);
        }

		fs.AddRecordToList("record5");

		Debug.Log("Records Saved.");
		Debug.Log("Record Count: " + fs.GetAmountOfRecords());

		fs.SetRecordFile(file);
		fs.SaveRecords();

		Debug.Log("Reloading Records...");
		fs.ClearAllRecordsFromList();
		Debug.Log("Record Count: " + fs.GetAmountOfRecords());
		
		// fs.LoadRecords();
		// Debug.Log("Record Count (Reloaded): " + fs.GetAmountOfRecords());
		// 
		// string str = "abc";
		// byte[] strData;
		// strData = FileStream.SerializeObject(str); // ASCII (size 27)
		// // strData = FileStream.SerializeObject(str.ToCharArray()); // ASCII (size 31)
		// 
		// {
		// 	int count = fs.GetAmountOfRecords();
		// 
		// 	// prints all records
		// 	for(int i = 0; i < count; i++)
        //     {
		// 		Debug.Log("Record " + i + ": " + fs.GetRecordFromList(i));
        //     }
		// 
        // }		

	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
