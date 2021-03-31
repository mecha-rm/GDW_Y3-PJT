#pragma once

// #include class header
#include "Timer.h"

#ifdef __cplusplus
extern "C" // convert to C code.
{
#endif
	// gets the current time.
	PLUGIN_API float GetCurrentTime();

	// sets the current time
	PLUGIN_API void SetCurrentTime(float newTime);

	// resets the timer
	PLUGIN_API void ResetTimer();

	// gets the total time
	PLUGIN_API float GetTotalTime();

	// gets the total number of splits
	PLUGIN_API int GetSplitCount();

	// gets the split at the provided index
	PLUGIN_API float GetSplit(int index);

	// splits the time, recording the current time on the stopwatch.
	PLUGIN_API void Split();

	// update
	PLUGIN_API void UpdateTimer(float deltaTime);

#ifdef __cplusplus
}
#endif

