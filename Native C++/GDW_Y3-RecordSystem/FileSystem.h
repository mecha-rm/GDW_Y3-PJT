#pragma once

#include "PluginSettings.h"
#include <fstream>
#include <string>
#include <vector>

// file system class
// this clears out the entire file and rewrites it entirely if set to write.
class PLUGIN_API FileSystem
{
public:
	// constructor
	FileSystem();

	// adds a record and returns its index in the list.
	void AddRecord(std::string record);

	// adds a record as a byte array. This does not delete the data.
	void AddRecord(char* arr);

	// inserts a record at the provided index.
	// this returns the index as well.
	void InsertRecord(std::string record, int index);

	// inserts a record in bytes
	void InsertRecord(char* arr, int index);

	// removes the record based on its string. If there are multiple instances of it, it only removes the first one.
	void RemoveRecord(std::string record);

	// removes record based on byte data.
	void RemoveRecord(char* arr);

	// removes the record based on its index
	void RemoveRecord(int index);

	// removes a record at its index. This is the same as 'RemoveRecord'.
	void RemoveRecordAtIndex(int index);

	// gets the record at the provided index. Returns empty string if invalid.
	const std::string& GetRecord(int index) const;

	// gets the record in byte form. Make sure to allocate the data first.
	void GetRecordInBytes(int index, char* arr, int size);

	// gets the length of a record.
	int GetRecordSize(int index) const;

	// gets the amount of records
	int GetRecordCount() const;

	// checks to see if a given record is in the file system.
	// this returns 'true' if the record is in the system.
	bool ContainsRecord(std::string record) const;

	// clears out all records
	void ClearAllRecords();

	// gets the file tied to this record system.
	const std::string& GetFile() const;

	// sets the file for this file system
	void SetFile(std::string file);

	// checks to see if the file is available. This does NOT save the file.
	bool FileAccessible() const;

	// checks to see if the file is available. This does NOT save the file.
	static bool FileAccessible(std::string file);

	// imports the records from the provided file
	bool ImportRecords();

	// imports records from file provided, and saves file.
	bool ImportRecords(std::string file);

	// exports records to saved file.
	bool ExportRecords();

	// exports records to provided file, and saves file.
	bool ExportRecords(std::string file);

private:
	// the file
	std::string file;

	// vector of lines for the file
	std::vector<std::string> records;

protected:
};

