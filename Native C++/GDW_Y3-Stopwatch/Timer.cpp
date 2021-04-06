#include "Timer.h"

// constructor
Timer::Timer()
{
}

// returns the current time.
float Timer::GetCurrentTime() const
{
	return currTime;
}

// sets the current time.
void Timer::SetCurrentTime(float newTime)
{
	currTime = newTime;
}

// STOP WATCH //

// constructor
StopwatchTimer::StopwatchTimer() : Timer()
{
	currTime = 0.0F;
}

// resets the stopwatch, deleting all splits.
void StopwatchTimer::ResetTimer()
{
	splits.clear();
	currTime = 0.0F;
	totalTime = 0.0F;
}

// returns the total time that has passed.
float StopwatchTimer::GetTotalTime() const
{
	return totalTime;
}

// gets the total number of splits.
int StopwatchTimer::GetSplitCount() const
{
	return splits.size();
}

// gets the split at the provided index.
// if there is no value at the provided index, then 0 is returned.
float StopwatchTimer::GetSplit(int index) const
{
	// returns the split value.
	if (index >= 0 && index < splits.size())
		return splits[index];
	else
		return -1;
}

// splits the time.
void StopwatchTimer::Split()
{
	splits.push_back(currTime); // saves the current time
	currTime = 0.0F; // sets the current time back to 0.
}

// update
void StopwatchTimer::UpdateTimer(float deltaTime)
{
	currTime += deltaTime;
	totalTime += deltaTime;
}

// COUNTDOWN TIMER //

// constructor
CountdownTimer::CountdownTimer() : Timer()
{
}

// resets the start timer.
void CountdownTimer::ResetTimer()
{
	currTime = startTime;
}

// set the start time
void CountdownTimer::SetStartTime(float time)
{
	startTime = (time > 0.0F) ? time : 0.0F; // time can't be negative.
	// currTime = startTime; // call ResetTimer() to change start time.
}

// sets the star time and resets the timer.
void CountdownTimer::SetStartTime(float time, bool resetTimer)
{
	// sets start time.
	startTime = (time > 0.0F) ? time : 0.0F;

	// resets timer is 'true'. 
	if (resetTimer)
		ResetTimer();
}

// gets the start time
float CountdownTimer::GetStartTime() const
{
	return startTime;
}

// checks to see if the countdown timer is finished.
bool CountdownTimer::IsFinished() const
{
	return currTime <= 0.0F;
}

// update
void CountdownTimer::UpdateTimer(float deltaTime)
{
	currTime -= deltaTime;

	// if the current time has reached 0.
	if (currTime < 0.0F)
		currTime = 0.0F;
}
