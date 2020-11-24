#pragma once

// #include class header
#include "MetricsLogger.h"

#ifdef __cplusplus
extern "C" // convert to C code.
{
#endif
	// adds the metric using a key (of length 'length'), and sets to the provided value.
	PLUGIN_API void AddMetric(const char* key, float value);
	
	// adds the metric using a key (of length 'length'), and sets to the provided value if 'replaceValue' is true.
	// PLUGIN_API void AddMetric(char* key, int length, float value, int replaceValue);
	
	// edits a metric
	PLUGIN_API void EditMetric(const char* key, float newValue);
	
	// removes a metric based on its key.
	PLUGIN_API void RemoveMetric(const char* key);

	// returns a metric based on the provided key
	PLUGIN_API float GetMetric(const char* key);

	// returns key at provided index
	PLUGIN_API const char* GetKeyAtIndex(int index);

	// returns the metric at the provided index.
	PLUGIN_API float GetValueAtIndex(int index);

	// returns the metric count
	PLUGIN_API int GetMetricCount();
	
	// checks to see if the metrics list is empty
	PLUGIN_API int IsEmpty();
	 
	// clears out all metrics
	PLUGIN_API void Clear();
	
	// returns and returns an array of list keys.
	// this is not saved
	// PLUGIN_API const char** GenerateKeyArray();
	// PLUGIN_API void GenerateKeyArray(const char** arr, int size);
	 
	// generates an array of values in the list.
	// this is not saved
	// PLUGIN_API float* GenerateValueArray();
	// PLUGIN_API void GenerateValueArray(float* arr, int size);
	
	// sets the file this metric logger will save to.
	// must allocate data before calling this function
	// TOOD: change to return char*
	PLUGIN_API void SetFile(const char* file);
	
	// gets the file name, which includes its path.
	// must allocate data before calling this function
	// PLUGIN_API void GetFile(const char* file, int length);

	// gets the file
	PLUGIN_API const char* GetFile();

	// gets the length of the file name
	PLUGIN_API int GetFileNameLength();

	// imports metrics from set file
	PLUGIN_API int ImportMetrics();
	
	// // imports metrics from the provided file. Saves file if successful import.
	// int ImportMetrics(char* file);
 
	// exports metrics to set file.
	// if the key has a space in it, its replaced with an underscore.
	// the file is still made if there are no metrics.
	PLUGIN_API int ExportMetrics();
	
	// // exports metrics to a file (saves provided file)
	// bool ExportMetrics(char* file);

#ifdef __cplusplus
}
#endif

