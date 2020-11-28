#pragma once

#include "FileSystem.h"

#ifdef __cplusplus
extern "C" // convert to C code.
{
#endif
	// adds a record and returns its index in the list.
	PLUGIN_API void AddRecord(const char* record);

	// adds a record in bytes
	PLUGIN_API void AddRecordInBytes(char* data, int size);

	// inserts a record at the provided index.
	// this returns the index as well.
	PLUGIN_API void InsertRecord(const char* record, int index);

	// inserts a record in bytes
	PLUGIN_API void InsertRecordInBytes(char* data, int size, int index);

	// removes the record based on its string. If there are multiple instances of it, it only removes the first one.
	PLUGIN_API void RemoveRecord(const char* record);

	// removes the record in bytes
	PLUGIN_API void RemoveRecordInBytes(char* data, int size);

	// removes a record at its index. This is the same as 'RemoveRecord'.
	PLUGIN_API void RemoveRecordAtIndex(int index);

	// gets the record at the provided index. Returns empty string if invalid.
	PLUGIN_API const char* GetRecord(int index);

	// gets the record in byte form.
	PLUGIN_API void GetRecordInBytes(int index, char* arr, int size);

	// gets the length of a record.
	PLUGIN_API int GetRecordSize(int index);

	// gets the amount of records
	PLUGIN_API int GetRecordCount();

	// returns (1) if contains record, (0) if record is not in list.
	PLUGIN_API int ContainsRecord(const char* record);

	// checks if the list contains the record (in bytes).
	PLUGIN_API int ContainsRecordInBytes(char* data, int size);

	// clears out all records
	PLUGIN_API void ClearAllRecords();

	// gets the file tied to this record system.
	PLUGIN_API const char* GetFile();

	// sets the file for this file system
	PLUGIN_API void SetFile(const char* file);

	// checks to see if the file is accessible.
	PLUGIN_API int FileAccessible();

	// imports the records from the provided file
	PLUGIN_API int ImportRecords();

	// exports records to saved file.
	PLUGIN_API int ExportRecords();

#ifdef __cplusplus
}
#endif

