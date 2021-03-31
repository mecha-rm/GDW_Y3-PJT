#include "Wrapper.h"

// stopwatch timer
StopwatchTimer swt;

// gets the current time
PLUGIN_API float GetCurrentTime()
{
    return swt.GetCurrentTime();
}

// sets the current time
PLUGIN_API void SetCurrentTime(float newTime)
{
    swt.SetCurrentTime(newTime);
}

// resets the timer
PLUGIN_API void ResetTimer()
{
    return swt.ResetTimer();
}

// gets the total time
PLUGIN_API float GetTotalTime()
{
    return swt.GetTotalTime();
}

// gets the amount of splits
PLUGIN_API int GetSplitCount()
{
    return swt.GetSplitCount();
}

// gets split and provided index
PLUGIN_API float GetSplit(int index)
{
    return swt.GetSplit(index);
}

// splits timer
PLUGIN_API void Split()
{
    return swt.Split();
}

// updates timer
PLUGIN_API void UpdateTimer(float deltaTime)
{
    return swt.UpdateTimer(deltaTime);
}
