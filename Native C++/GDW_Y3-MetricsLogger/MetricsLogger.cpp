#include "MetricsLogger.h"
#include <sstream>
#include <fstream>

// checks to see if the file exists
bool FileAccessible(std::string fileName)
{
	std::ifstream file(fileName, std::ios::in); // opens file for reading
	bool accessible; // checks to see if the file is accessible.

	// if !file is true, then the file couldn't be opened.
	accessible = !file;
	file.close();

	// returns the opposite of 'accessible' since it's showing if the file is accessible.
	return !accessible;
}

// splits the string via its spaces
// std::vector<std::string> SplitString(const std::string str)
// {
// 	// the string vector
// 	std::vector<std::string> vec;
// 
// 	// copies the string
// 	std::string strCpy = str;
// 	std::string divider = " ";
// 
// 	// while there are still spaces
// 	while (strCpy.find(divider) != std::string::npos)
// 	{
// 		// gets the index of the divider
// 		int index = strCpy.find(divider);
// 
// 		// gets the section of the string and adds it to the vector.
// 		std::string temp = strCpy.substr(0, index);
// 		vec.push_back(temp);
// 
// 		// erases portion from overall string.
// 		strCpy.erase(0, index + divider.size());
// 	}
// 
// 	// if there's anything left in the string, then that's added as the final element in the vector.
// 	if (!strCpy.empty())
// 	{
// 		vec.push_back(strCpy);
// 	}
// 
// 	return vec;
// }

// splits the string into a vector
const std::vector<std::string> SplitString(const std::string& str)
{
	std::stringstream ss; // the string stream.
	std::vector<std::string> vec; // the vector used for the vertex.
	std::string value; // used to store the item from the string.

	ss.str(str); // stores the string in the stream

	// while the string stream isn't empty
	while (ss >> value)
	{
		// if the conversion failed, the string stream moves onto the next item.
		if (ss.bad())
			continue;


		vec.push_back(value); // saves in the vector
	}

	return vec; // returns the vector
}

// replaces substring
std::string ReplaceSubstring(std::string str, std::string oldSubstr, std::string newSubstr)
{
	// while the old substring is still present
	while (str.find(oldSubstr) != std::string::npos)
	{
		int index = str.find(oldSubstr); // find the substring
		str.replace(index, oldSubstr.length(), newSubstr); // replaces string
	}

	return str;
}

// string to different type
// template<typename T>
// const T ConvertString(const std::string& str)
// {
// 	std::stringstream ss; // string stream
// 	T value; // value
// 
// 	// puts string into stream and puts it into value
// 	ss.str(str);
// 	ss >> value;
// 
// 	return value;
// }

// string to float conversion
const float StringToFloat(const std::string& str)
{
	std::stringstream ss; // string stream
	float value; // value

	// puts string into stream and puts it into value
	ss.str(str);
	ss >> value;

	return value;
}


// CLASS
// constructor
MetricsLogger::MetricsLogger()
{
}

// adds a metric
void MetricsLogger::AddMetric(std::string key, float value)
{
	metrics[key] = value;
}

// adds a metric
void MetricsLogger::AddMetric(std::string key, float value, bool replaceValue)
{
	// key exists
	if (metrics.find(key) != metrics.end() && replaceValue)
	{
		metrics[key] = value;
	}
	else if(metrics.find(key) == metrics.end()) // key doesn't exist
	{
		metrics[key] = value;
	}
}

// edits a metric
void MetricsLogger::EditMetric(std::string key, float newValue)
{
	if (metrics.find(key) != metrics.end())
		metrics.at(key) = newValue;
}

// removes metric
void MetricsLogger::RemoveMetric(std::string key)
{
	// if the key exists, remove it.
	if (metrics.find(key) != metrics.end())
	{
		metrics.erase(key);
	}
}

// returns the metric at the provided index.
float MetricsLogger::GetMetric(std::string key)
{
	if (metrics.find(key) != metrics.end())
		return metrics[key];
	else
		return 0.0F;
}

// gets the key at the index
const std::string& MetricsLogger::GetKeyAtIndex(int index)
{
	// if the index is out of bounds
	if (index < 0 || index >= metrics.size())
		return "";

	// gets the iterator and increments it
	auto it = metrics.begin();
	std::advance(it, index);

	return (*it).first;
}

// gets the metric at the provided index
float MetricsLogger::GetValueAtIndex(int index)
{
	// if the index is out of bounds
	if (index < 0 || index >= metrics.size())
		return 0.0F;

	// gets the iterator and increments it
	auto it = metrics.begin();
	std::advance(it, index);

	return (*it).second;
}

// gets the amount of metrics
int MetricsLogger::GetMetricCount() const
{
	return metrics.size();
}

// checks to see if the metric logger is empty
bool MetricsLogger::IsEmpty()
{
	return metrics.empty();
}

// clears out the metrics
void MetricsLogger::Clear()
{
	metrics.clear();
}

// generates an array of keys
std::string* MetricsLogger::GenerateKeyArray() const
{
	// if there are no metrics
	if (!metrics.empty())
	{
		std::string* arr = new std::string[metrics.size()];
		int index = 0;

		// goes through all values
		for (auto it = metrics.begin(); it != metrics.end(); it++)
		{
			arr[index] = (*it).first;
			index++;
		}

		return arr;
	}
	else
	{
		return nullptr;
	}
}

// fills an array with values from the map
float* MetricsLogger::GenerateValueArray() const
{
	// if there are no metrics
	if (!metrics.empty())
	{
		float* arr = new float[metrics.size()];
		int index = 0;

		// goes through all values
		for (auto& val : metrics)
		{
			arr[index] = val.second;
			index++;
		}

		return arr;
	}
	else
	{
		return nullptr;
	}
}

// sets the file the metrics logger is saved
void MetricsLogger::SetFile(std::string file)
{
	this->file = file;
}

// returns the file
const std::string& MetricsLogger::GetFile() const
{
	return file;
}

// gets the length of the file name
int MetricsLogger::GetFileNameLength() const
{
	return file.length();
}

// imports metrics from file
bool MetricsLogger::ImportMetrics()
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

	// gets the keys and values
	std::string* keys = GenerateKeyArray();
	float* values = GenerateValueArray();

	// read all metrics
	while (std::getline(f, line))
	{
		// splits the string
		std::vector<std::string> vec = SplitString(line);

		// string and value
		std::string str = "";
		float flt = 0.0F;

		// gets the values from the string.
		if (vec.size() == 2)
		{
			str = vec[0];
			flt = StringToFloat(vec[1]);
		}
		else if (vec.size() > 2)
		{
			// grabs the last value and treats the rest as strings
			for (int i = 0; i < vec.size() - 1; i++)
			{
				// checks to see if it's the last value
				str+= (i + 1 < vec.size()) ? vec[i] : vec[i] + " "; // tab (\t) gives the same result	
			}

			// gets the value as it is the last in the index.
			flt = StringToFloat(vec[vec.size() - 1]);
		}

		// saves the value
		if(str != "")
			metrics[str] = flt;
	}

	// closes
	f.close();

	delete[] keys;
	delete[] values;

	return true;
}

// imports metrics form a file
bool MetricsLogger::ImportMetrics(std::string file)
{
	std::string origFile = this->file;
	this->file = file;

	// sees if the import was successful
	bool success = ImportMetrics();

	// if the import was a failure, don't ave the file name.
	if (!success)
		this->file = origFile;

	return success;
}

// exports metrics from a file
bool MetricsLogger::ExportMetrics()
{
	// checks to see if the file can be accessed.
	// if the file doesn't exist, one is made.
	// if (!FileAccessible(file))
	// 	return false;

	// file, and the line from the file
	std::ofstream f(file);
	std::string line = "";

	// if the file isn't open. Taken out since it can't make folders otherwise.
	if (!f.is_open())
		return false;

	// if there are no values
	if (metrics.empty())
		return false;

	// gets the keys and values
	std::string* keys = GenerateKeyArray();
	float* values = GenerateValueArray();

	// writes all metrics
	for (int i = 0; i < metrics.size(); i++)
	{
		f << keys[i] << " " << values[i] << "\n";
	}

	// closes the file
	f.close();

	delete[] keys;
	delete[] values;

	return true;
}

// exports metrics to a file
bool MetricsLogger::ExportMetrics(std::string file)
{
	std::string origFile = this->file;
	this->file = file;

	// sees if the export was successful
	bool success = ExportMetrics();

	// if the export was a failure, don't ave the file name.
	if (!success)
		this->file = origFile;

	return success;
}
