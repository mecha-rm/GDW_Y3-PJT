#pragma once

#include "Timer.h"

#ifdef __cplusplus
extern "C" // convert to C code.
{
#endif
	// get current time
	PLUGIN_API float GetCurrentTime();

	// sets the current itme.
	PLUGIN_API void SetCurrentTime(float newTime);

	// resets the timer.
	PLUGIN_API void ResetTimer();

	// sets the start time
	PLUGIN_API void SetStartTime(float time);

	// sets the start time and resets the timer.
	PLUGIN_API void SetStartTimeAndResetTimer(float time, bool resetTimer);

	// returns the start time
	PLUGIN_API float GetStartTime();

	// checks to see if the countdown timer is finished.
	PLUGIN_API bool IsFinished();

	// update timer
	PLUGIN_API void UpdateTimer(float deltaTime);

#ifdef __cplusplus
}
#endif

