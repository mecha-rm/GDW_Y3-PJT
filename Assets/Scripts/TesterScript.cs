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
		StringRecordManager fs = new StringRecordManager();
		string file = "Assets/Resources/Saves/record_test.txt";
		int byteIndex = -1;

		fs.ClearAllRecordsFromList();

		fs.AddRecordToList("record1");
		fs.AddRecordToList("record2");
		fs.AddRecordToList("record3");

		if(true)
        {
			// float x = 2.0f;
			Vec3 x;
			x.x = 10;
			x.y = 2;
			x.z = -1;

			byte[] data = StringRecordManager.SerializeObject(x);
			fs.AddRecordToList(data);
			byteIndex = fs.GetAmountOfRecords() - 1;

			x.x = 0;
			x.y = 0;
			x.z = 0;

			x = (Vec3)StringRecordManager.DeserializeObject(data);

        }
		else
        {
			fs.AddRecordToList("record4");
        }

		fs.AddRecordToList("record5");


		Debug.Log("Records Saved.");
		
		{
			int count = fs.GetAmountOfRecords();
			Debug.Log("Record Count: " + count);

			for (int i = 0; i < count; i++)
			{
				Debug.Log("Record " + i + " Size: " + fs.GetRecordLength(i));
			}

			Debug.Log("-----");

			for (int i = 0; i < count; i++)
			{
				Debug.Log("Record " + i + ": " + fs.GetRecordFromList(i));
			}

		}

		fs.SetRecordFile(file);
		fs.SaveRecords();

		Debug.Log("Reloading Records...");
		fs.ClearAllRecordsFromList();
		// Debug.Log("Record Count: " + fs.GetAmountOfRecords());
		
		fs.LoadRecords();
		Debug.Log("Record Count (Reloaded): " + fs.GetAmountOfRecords());
		
		// string str = "abc";
		// byte[] strData;
		// strData = FileStream.SerializeObject(str); // ASCII (size 27)
		// strData = FileStream.SerializeObject(str.ToCharArray()); // ASCII (size 31)
		
		{
			int count = fs.GetAmountOfRecords();
		
			// prints all records
			for(int i = 0; i < count; i++)
            {
				if(i == byteIndex) // byte storage
                {
					byte[] impData = fs.GetRecordFromListInBytes(i);
					Vec3 x2 = new Vec3();
					object obj = StringRecordManager.DeserializeObject(impData);

					x2 = (Vec3)obj;
					Debug.Log("Vec3 Import: " + "(" + x2.x + ", " + x2.y + ", " + x2.z + ")");
				}
                else
                {
					Debug.Log("Record " + i + ": " + fs.GetRecordFromList(i));
                }
				
            }

			// for(int i = 0; i < count; i++)
            // {
			// 	Debug.Log("Record " + i + " Size: " + fs.GetRecordLength(i));
            // }
		
        }		

	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
