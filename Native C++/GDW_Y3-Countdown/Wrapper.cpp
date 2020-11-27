#include "Wrapper.h"

// countdown timer
CountdownTimer cdt;

// gets the current time
PLUGIN_API float GetCurrentTime()
{
    return cdt.GetCurrentTime();
}

// sets the start time
PLUGIN_API void SetStartTime(float time)
{
    return cdt.SetStartTime(time);
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