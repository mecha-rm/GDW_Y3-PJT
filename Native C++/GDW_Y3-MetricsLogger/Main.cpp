// Main.cpp : This file contains the 'main' function. Program execution begins and ends there.
/*
* Teammates:
	* Adam Kahil (100655089)
	* Kennedy Adams	(100632983)
	* Roderick “R.J.” Montague (100701758)
*/

//#include <iostream>
//#include "MetricsLogger.h"
//
//// prints out the metrics data
//void PrintMetricsData(MetricsLogger logger)
//{
//	// gets amount of values
//	int count = logger.GetMetricCount();
//
//	// gets the keys and values
//	std::string* keys = logger.GenerateKeyArray();
//	float* vals = logger.GenerateValueArray();
//
//	// prints values
//	for (int i = 0; i < count; i++)
//	{
//		std::cout << keys[i] << ": " << vals[i] << "\n";
//	}
//
//	// deletes data
//	delete[] keys;
//	delete[] vals;
//}
//
//int main()
//{
//	// std::cout << "Hello World!\n";
//
//	// copying values from a string to a char array
//	// {
//	// 	std::string str = "Kevin";
//	// 	char* arr = new char[str.length()];
//	// 	memcpy(arr, str.c_str(), sizeof(char) * str.length());
//	// 	
//	// 	for (int i = 0; i < str.length(); i++)
//	// 		std::cout << arr[i];
//	// 	
//	// 	std::cout << std::endl;
//	// }
//
//	MetricsLogger logger;
//	// std::string file = "test.txt";
//	std::string file = "files/test.txt";
//
//	// adding values
//	logger.AddMetric("A", 15.0F);
//	logger.AddMetric("B", 10);
//	logger.AddMetric("C", -5.0F);
//	logger.AddMetric("D", 1.014F);
//	logger.AddMetric("E", 10.142F);
//	logger.AddMetric("AE", -20.031F);
//	logger.AddMetric("XR", -100.112F);
//
//	// adding and editing values
//	logger.AddMetric("A", -3.142);
//	logger.AddMetric("A", 13.451, true);
//	logger.AddMetric("A", -0.2141, false);
//	logger.EditMetric("XR", -24.01F);
//
//	// adding and editing metrics (invalid operations)
//	logger.EditMetric("AWY", -120);
//
//	// TODO: test index function
//	// shows print at index
//	// {
//	// 	int index = 2;
//	// 	std::cout << "B at Index " << std::endl;
//	// }
//
//	// removing values
//	// logger.RemoveMetric();
//
//	// prints out metrics data
//	PrintMetricsData(logger);
//	std::cout << std::endl;
//
//	{
//		std::cout << "Numerical Order: " << std::endl;
//		int count = logger.GetMetricCount();
//
//		for (int i = 0; i < count; i++)
//		{
//			std::cout << "Metric At Index " << i << " - " <<
//				logger.GetKeyAtIndex(i) << " : " << logger.GetValueAtIndex(i) << "\n";
//		}
//
//		std::cout << std::endl;
//	}
//
//	// writing metrics to file
//	std::cout << "Saving Data from \"" << file << "\"..." << std::endl;
//	std::cout << "File Written Successfully? - " << std::boolalpha << logger.ExportMetrics(file) << std::endl;
//
//	// clearing values
//	std::cout << std::endl;
//	std::cout << "Clearing Values..." << std::endl;
//	logger.Clear();
//	std::cout << "IsEmpty: " << std::boolalpha << logger.IsEmpty() << std::endl;
//	std::cout << std::endl;
//
//	// reading metrics from file
//	std::cout << "Reading Data from \"" << file << "\"..." << std::endl;
//	std::cout << "File Read Successfully? - " << std::boolalpha << logger.ImportMetrics(file) << std::endl;
//
//	// checking again to see if data was read.
//	std::cout << "IsEmpty: " << std::boolalpha << logger.IsEmpty() << std::endl;
//	std::cout << std::endl;
//
//	// prints out the data again
//	std::cout << std::endl;
//	PrintMetricsData(logger);
//	std::cout << std::endl;
//
//	system("pause");
//}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
