#include "Wrapper.h"

// countdown timer
CountdownTimer cdt;

// gets the current time
PLUGIN_API float GetCurrentTime()
{
    return cdt.GetCurrentTime();
}

// sets the current time
PLUGIN_API void SetCurrentTime(float newTime)
{
    cdt.SetCurrentTime(newTime);
}

// resets the timer
PLUGIN_API void ResetTimer()
{
    return cdt.ResetTimer();
}

// sets the start time
PLUGIN_API void SetStartTime(float time)
{
    return cdt.SetStartTime(time);
}

// sets the start time and resets the timer.
PLUGIN_API void SetStartTimeAndResetTimer(float time, bool resetTimer)
{
    return cdt.SetStartTime(time, resetTimer);
}

// gets the start time
PLUGIN_API float GetStartTime()
{
    return cdt.GetStartTime();
}

// checks to see if the countdown timer is finished
PLUGIN_API bool IsFinished()
{
    return cdt.IsFinished();
}

// updates the timer
PLUGIN_API void UpdateTimer(float deltaTime)
{
    return cdt.UpdateTimer(deltaTime);
}
