// Main.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "FileSystem.h"
#include "Wrapper.h"

// prints records
// void PrintRecords(const FileSystem& fs)
// {
// 	for (int i = 0; i < fs.GetRecordCount(); i++)
// 	{
// 		std::cout << "\nRecord " << i << ": " << fs.GetRecord(i);
// 	}
// 
// 	std::cout << std::endl;
// }
// 
// // prints record sizes
// void PrintRecordSizes(const FileSystem& fs)
// {
// 	for (int i = 0; i < fs.GetRecordCount(); i++)
// 	{
// 		std::cout << "\nRecord " << i << " Size: " << fs.GetRecordSize(i);
// 	}
// 
// 	std::cout << std::endl;
// }
// 
// int main()
// {
// 	// object
// 	FileSystem fs;
// 	std::string file = "text.txt";
// 	bool success = false;
// 
// 	std::cout << "File \"" << fs.GetFile() << "\" Exists (At Start): " << std::boolalpha << fs.FileAccessible() << std::endl;
// 	std::cout << "File \"" << file << "\" Exists (At Start): " << std::boolalpha << FileSystem::FileAccessible(file) << "\n" << std::endl;
// 
// 	// adds record
// 	fs.AddRecord("Adrian");
// 	fs.AddRecord("Beatrice");
// 	fs.AddRecord("Clarence");
// 	fs.AddRecord("Doug");
// 	fs.AddRecord("Eunice");
// 	fs.AddRecord("Fred");
// 	
// 	// record count
// 	std::cout << "Record Count: " << fs.GetRecordCount() << std::endl;
// 	
// 	// prints records
// 	PrintRecordSizes(fs);
// 	PrintRecords(fs);
// 
// 	fs.SetFile(file);
// 	std::cout << "\nFile " << fs.GetFile() << " is Available - " << std::boolalpha << fs.FileAccessible() << "\n" << std::endl;
// 
// 	std::cout << "Saving Content to " << fs.GetFile() << "." << std::endl;
// 	success = fs.ExportRecords();
// 	std::cout << "Save Successful: " << std::boolalpha << success << std::endl;
// 
// 	std::cout << "\nClearing Contents..." << std::endl;
// 	fs.ClearAllRecords();
// 	std::cout << "Record Count: " << fs.GetRecordCount() << std::endl;
// 	std::cout << std::endl;
// 
// 	std::cout << "Reading in Content from " << fs.GetFile() << "." << std::endl;
// 	success = fs.ImportRecords();
// 	std::cout << "Read in Successful: " << std::boolalpha << success << std::endl;
// 	std::cout << "Record Count: " << fs.GetRecordCount() << std::endl;
// 
// 	std::cout << "Printing Record Sizes from Reload: " << std::endl;
// 	PrintRecordSizes(fs);
// 	PrintRecords(fs);
// 
// 	std::cout << "Complete" << std::endl;
// 
// 	{
// 		std::cout << "\nTesting Wrapper" << std::endl;
// 		std::string r1 = "Bibity Bopity";
// 
// 		const int R_LEN = 5;
// 		const char* r2 = new char[R_LEN] {'a', 'b', 'c', 'd', 'e'};
// 		
// 		float x = 5;
// 		float y = 0;
// 		char* r3 = new char[sizeof(x)];
// 		memcpy(r3, &x, sizeof(float));
// 		y = 0;
// 		memcpy(&y, r3, sizeof(float));
// 
// 		if (x == y)
// 			std::cout << "Memcpy Works" << std::endl;
// 
// 		AddRecord(r1.c_str());
// 		AddRecord(r2); // gives garbage
// 		AddRecordInBytes(r3, sizeof(x));
// 
// 		int count = GetRecordCount();
// 		std::cout << "Record Count: " << count << std::endl;
// 
// 		for (int i = 0; i < count; i++)
// 		{
// 			int rsize = GetRecordSize(i);
// 			std::cout << "Record " << i << " Size: " << rsize << "\n";
// 			std::cout << "Record " << i << ": " << GetRecord(i) << "\n" << std::endl;
// 		}
// 
// 
// 		delete[] r2;
// 		delete[] r3;
// 	}
// 
// 	system("pause");
// }

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
