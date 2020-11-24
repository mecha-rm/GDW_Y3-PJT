#include "Wrapper.h"
StopwatchTimer swt;

// gets the current time
PLUGIN_API float GetCurrentTime()
{
    return swt.GetCurrentTime();
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
