#include "Wrapper.h"

FileSystem fs;

// adds a record and returns its index
PLUGIN_API void AddRecord(const char* record)
{
	return fs.AddRecord(std::string(record));
}

// inserts record at index
PLUGIN_API void InsertRecord(const char* record, int index)
{
	return fs.InsertRecord(std::string(record), index);
}

// removes record
PLUGIN_API void RemoveRecord(const char* record)
{
	return fs.RemoveRecord(std::string(record));
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
