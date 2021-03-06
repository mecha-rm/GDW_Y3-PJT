#include "FileSystem.h"
#include <iostream>

// replaces substring and returns new string.
std::string ReplaceAllSubstrings(const std::string& str, std::string oldSubstr, std::string newSubstr)
{
	// string result
	std::string strRes = str;

	// while there are still instances of the old substring
	while (strRes.find(oldSubstr) != std::string::npos)
	{
		// gets the substring index and replaces the string
		int index = strRes.find(oldSubstr);
		strRes.replace(index, oldSubstr.length(), newSubstr);
	}

	// returns the resulting string
	return strRes;
}

// constructor
FileSystem::FileSystem()
{
}

// adds a record, and returns its index.
void FileSystem::AddRecord(std::string record)
{
	records.push_back(record);
}

// adds a record as a byte array. This does not delete the data.
void FileSystem::AddRecord(char* arr, int size)
{
	// adds the record.
	records.push_back(std::string(arr, size));
}

// inserts a record at the provided index.
void FileSystem::InsertRecord(std::string record, int index)
{
	if (index < 0) // beginning of list
		records.insert(records.begin(), record);
	else if (index >= records.size()) // end of list
		records.push_back(record);
	else // provided index
		records.insert(records.begin() + index, record);
}

// inserts a record in bytes
void FileSystem::InsertRecord(char* arr, int size, int index)
{
	InsertRecord(std::string(arr, size), index);
}

// removes a record
void FileSystem::RemoveRecord(std::string record)
{
	// the index of the record to be removed.
	int index = -1;

	// finds the record
	for (int i = 0; i < records.size(); i++)
	{
		if (records[i] == record)
		{
			index = i;
			break;
		}
	}

	// removes the record at the index.
	if (index != -1)
		records.erase(records.begin() + index);
}

// removes record based on byte data
void FileSystem::RemoveRecord(char* arr, int size)
{
	RemoveRecord(std::string(arr, size));
}

// removes the record ath the provided index
void FileSystem::RemoveRecord(int index)
{
	// since this is for a DLL, overloading functions is not allowed.
	// as such, a function with a unique name is called index.
	RemoveRecordAtIndex(index);
}

// removes the record at the provided index.
void FileSystem::RemoveRecordAtIndex(int index)
{
	// valid index
	if (index >= 0 && index < records.size())
		records.erase(records.begin() + index);
}

// gets record at provided index.
const std::string& FileSystem::GetRecord(int index) const
{
	if (index >= 0 && index < records.size())
		return records.at(index);
	else
		return std::string("");
}

// gets the record in bytes
// TODO: find out if you can return an array instead of filling one.
void FileSystem::GetRecordInBytes(int index, char* arr, int size)
{
	std::string record = ""; // record to be received

	// index bounds check
	if (index >= 0 && index < records.size())
		record = records.at(index);
	else
		return;

	// adds in values while there is still space.
	for (int i = 0; i < size && i < record.length(); i++)
		arr[i] = record[i]; // copy data
}

// returns the length of a record.
int FileSystem::GetRecordSize(int index) const
{
	if (index >= 0 && index < records.size())
		return records.at(index).length();
	else
		return -1;
}

// gets the amount of records.
int FileSystem::GetRecordCount() const
{
	return records.size();
}

// returns bool to show if record is in list.
bool FileSystem::ContainsRecord(std::string record) const
{
	// finds the record
	for (int i = 0; i < records.size(); i++)
	{
		if (records[i] == record) // record is in list.
		{
			return true;
		}
	}

	return false;
}

// checks to see if the record contains the record (in bytes).
bool FileSystem::ContainsRecord(char* arr, int size) const
{
	return ContainsRecord(std::string(arr, size));
}

// clears out all records
void FileSystem::ClearAllRecords()
{
	records.clear();
}

// gets the file
const std::string& FileSystem::GetFile() const
{
	return file;
}

// sets the file for this file system
void FileSystem::SetFile(std::string file)
{
	this->file = file;
}

// checks to see if the file is accessible
bool FileSystem::FileAccessible() const
{
	return FileAccessible(file);
}

// checks to see if the file exists
bool FileSystem::FileAccessible(std::string fileName)
{
	std::ifstream file(fileName, std::ios::in); // opens file for reading
	bool accessible; // checks to see if the file is accessible.

	// if !file is true, then the file couldn't be opened.
	accessible = !file;
	file.close();

	// returns the opposite of 'accessible' since it's showing if the file is accessible.
	return !accessible;
}

// imports the records
bool FileSystem::ImportRecords()
{
	// checks to see if the file can be accessed.
	if (!FileAccessible(file))
		return false;

	// file, and the line from the file
	std::ifstream f(file);
	std::string line = "";

	// if the file isn't open
	if (!f.is_open())
		return false;

	// read all records
	// it now replaces all instances of "\\n" with "\n" since that's how the file stores it.
	// it's done this way because each record is put on a new line.
	while (std::getline(f, line))
	{
		// records.push_back(line);
		
		records.push_back(ReplaceAllSubstrings(line, "\\n", "\n"));

		// replaces all instances of "\\n" with "\n" and all instances of "\\\n" with "\\n"
		// std::string imp = ReplaceAllSubstrings(line, "\\n", "\n");
		// imp = ReplaceAllSubstrings(imp, "\\\n", "\\n");
		// records.push_back(imp);
	}

	// closes file
	f.close();

	return true;
}

// imports file and saves it
bool FileSystem::ImportRecords(std::string file)
{
	std::string origFile = this->file;
	this->file = file;

	// sees if the import was successful
	bool success = ImportRecords();

	// if the import was a failure, don't ave the file name.
	if (!success)
		this->file = origFile;

	return success;
}

// exports the records.
bool FileSystem::ExportRecords()
{
	// this creates the file if it doesn't exist.

	// file, and the line from the file
	std::ofstream f;
	std::string line = "";

	// opens the file and clears out existing content in it.
	f.open(file, std::ios::out | std::ios::trunc);

	// if the file isn't open, return false.
	if (!f.is_open())
		return false;

	// if there are no values
	if (records.empty())
		return false;

	// writes all records
	// in order to be read properly, all instances of "\n" are replaced with "\\n"
	// this allows the "\n" symbol to be put into the file instead of a new line instead
	for (int i = 0; i < records.size(); i++)
	{
		// f << records[i] << "\n";

		f << ReplaceAllSubstrings(records[i], "\n", "\\n") << "\n";
		
		// replaces all instances of "\\n" with "\\\n" first, then replaces all instances of "\n" with "\\n"
		// std::string exp = ReplaceAllSubstrings(records[i], "\\n", "\\\n");;
		// exp = ReplaceAllSubstrings(records[i], "\n", "\\n");
		// 
		// f << exp << "\n";
	}

	// closes the file
	f.close();

	return true;
}

// exports the records and saves the file.
bool FileSystem::ExportRecords(std::string file)
{
	std::string origFile = this->file;
	this->file = file;

	// sees if the export was successful
	bool success = ExportRecords();

	// if the export was a failure, don't ave the file name.
	if (!success)
		this->file = origFile;

	return success;
}
