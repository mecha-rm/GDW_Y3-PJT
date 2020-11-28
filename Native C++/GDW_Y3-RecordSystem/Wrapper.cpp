#include "Wrapper.h"

FileSystem fs;

// adds a record and returns its index
PLUGIN_API void AddRecord(const char* record)
{
	return fs.AddRecord(std::string(record));
}

// adds a record in bytes
PLUGIN_API void AddRecordInBytes(char* data)
{
	return fs.AddRecord(data);
}

// inserts record at index
PLUGIN_API void InsertRecord(const char* record, int index)
{
	return fs.InsertRecord(std::string(record), index);
}

// inserts the record in bytes
PLUGIN_API void InsertRecordInBytes(char* data, int index)
{
	return fs.InsertRecord(data, index);
}

// removes record
PLUGIN_API void RemoveRecord(const char* record)
{
	return fs.RemoveRecord(std::string(record));
}

// removes a record in bytes
PLUGIN_API void RemoveRecordInBytes(char* data)
{
	return fs.RemoveRecord(data);
}

// removes record at index
PLUGIN_API void RemoveRecordAtIndex(int index)
{
	return fs.RemoveRecordAtIndex(index);
}

// gets record at index.
PLUGIN_API const char* GetRecord(int index)
{
	return fs.GetRecord(index).c_str();
}

// gets the record in byte form.
PLUGIN_API void GetRecordInBytes(int index, char* arr, int size)
{
	return fs.GetRecordInBytes(index, arr, size);
}

// returns the length of the record.
PLUGIN_API int GetRecordSize(int index)
{
	fs.GetRecordSize(index);
}

// gets the amount of records
PLUGIN_API int GetRecordCount()
{
	return fs.GetRecordCount();
}

// returns '1' if contains record, 0 if not.
PLUGIN_API int ContainsRecord(const char* record)
{
	return (int)fs.ContainsRecord(std::string(record));
}

// clears out all records
PLUGIN_API void ClearAllRecords()
{
	return fs.ClearAllRecords();
}

// returns the file
PLUGIN_API const char* GetFile()
{
	return fs.GetFile().c_str();
}

// sets the file
PLUGIN_API void SetFile(const char* file)
{
	return fs.SetFile(std::string(file));
}

// checks to see if the file is accessible.
PLUGIN_API int FileAccessible()
{
	return (int)fs.FileAccessible();
}

// imports records
PLUGIN_API int ImportRecords()
{
	return (int)fs.ImportRecords();
}

// exports records
PLUGIN_API int ExportRecords()
{
	return (int)fs.ExportRecords();
}
