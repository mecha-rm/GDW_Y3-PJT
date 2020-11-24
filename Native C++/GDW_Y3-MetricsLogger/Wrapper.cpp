#include "Wrapper.h"

MetricsLogger logger;

// adds a metric
PLUGIN_API void AddMetric(const char* key, float value)
{
	logger.AddMetric(std::string(key), value);
}

// adds a metric
// PLUGIN_API void AddMetric(char* key, int length, float value, int replaceValue)
// {
// 	// any value other than 0 will convert to a 'true' bool. A zero makes it false.
// 	logger.AddMetric(std::string(key), value, (bool)replaceValue);
// }

// edits a metric
void EditMetric(const char* key, float newValue)
{
	logger.AddMetric(std::string(key), newValue);
}

// removes a metric
PLUGIN_API void RemoveMetric(const char* key)
{
	logger.RemoveMetric(key);
}

// gets the metric
PLUGIN_API float GetMetric(const char* key)
{
	return logger.GetMetric(std::string(key));
}

// gets the key at the index
PLUGIN_API const char* GetKeyAtIndex(int index)
{
	return logger.GetKeyAtIndex(index).c_str();
}

// gets the value at the index
PLUGIN_API float GetValueAtIndex(int index)
{
	return logger.GetValueAtIndex(index);
}

// gets the metric count
PLUGIN_API int GetMetricCount()
{
	return logger.GetMetricCount();
}

// checks to see if the logger is empty.
PLUGIN_API int IsEmpty()
{
	return (int)logger.IsEmpty();
}

// clears out metrics
PLUGIN_API void Clear()
{
	return logger.Clear();
}

// generates a array of keys
//PLUGIN_API const char** GenerateKeyArray()
//{
//	std::string* arr1 = logger.GenerateKeyArray();
//	int count = logger.GetMetricCount();
//	const char** arr2 = new const char*[count];
//
//	// fills the arrays
//	for (int i = 0; i < count; i++)
//	{
//		arr2[i] = arr1[i].c_str();
//	}
//	
//	return arr2;
//}

// fills a key array
// PLUGIN_API void GenerateKeyArray(const char** arr, int size)
// {
// 	// instead of generating an array of values, it just copies the values to the provided array
// 	int count = logger.GetMetricCount();
// 
// 	// fills the array
// 	for (int i = 0; i < count && i < size; i++)
// 	{
// 		arr[i] = logger.GetKeyAtIndex(i).c_str();
// 	}
// }

// generates a value array and returns it
// PLUGIN_API float* GenerateValueArray()
// {
// 	float* arr = logger.GenerateValueArray();
// 	return arr;
// }

// fills a value array
// PLUGIN_API void GenerateValueArray(float* arr, int size)
// {
// 	int count = logger.GetMetricCount();
// 
// 	// fills the array
// 	for (int i = 0; i < count && i < size; i++)
// 	{
// 		arr[i] = logger.GetValueAtIndex(i);
// 	}
// }

// sets the file
PLUGIN_API void SetFile(const char* file)
{
	logger.SetFile(std::string(file));
}

// gets the file
// PLUGIN_API void GetFile(char* file, int length)
// {
// 	memcpy(file, logger.GetFile().c_str(), sizeof(char) * length);
// }

// return the file
PLUGIN_API const char* GetFile()
{
	return logger.GetFile().c_str();
}

// gets the length of the file name.
PLUGIN_API int GetFileNameLength()
{
	return logger.GetFileNameLength();
}

// imports metrics from set file
PLUGIN_API int ImportMetrics()
{
	return logger.ImportMetrics();
}

// exports metrics
PLUGIN_API int ExportMetrics()
{
	return logger.ExportMetrics();
}