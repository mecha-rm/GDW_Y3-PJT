#pragma once

#include "PluginSettings.h"
#include <vector>

// timer class
class PLUGIN_API Timer
{
public:
	Timer();

	// gets the current time.
	float GetCurrentTime() const;

	// update timer with how much time has passed.
	// to pause the timer, just don't update it.
	virtual void UpdateTimer(float deltaTime) = 0;


private:
	

protected:
	float currTime = 0.0F;

};

// a countdown timer
class PLUGIN_API CountdownTimer : public Timer
{
public:
	// constructor
	CountdownTimer();

	// sets the start time.
	// this sets the current time to the start time.
	void SetStartTime(float time);

	// returns the start time
	float GetStartTime() const;

	// checks to see if the countdown timer is finished.
	bool IsFinished() const;

	// updates the countdown timer
	void UpdateTimer(float deltaTime) override;

private:
	// the start time for the countdown timer.
	float startTime = 0.0F;

protected:

};

// a stopwatch for the timer
class PLUGIN_API StopwatchTimer : public Timer
{
public:
	// constructor
	StopwatchTimer();

	// resets the stopwatch, deleting all splits.
	void ResetTimer();
	 
	// gets the total time that has passed.
	float GetTotalTime() const;

	// gets the total number of splits
	int GetSplitCount() const;

	// gets the split at the provided index.
	// the index is out of bounds, -1 is returned.
	float GetSplit(int index) const;

	// splits the time, recording the current time on the stopwatch.
	void Split();

	// update
	void UpdateTimer(float deltaTime) override;

private:
	// splits the time
	std::vector<float> splits;

	// the split
	float split = 0.0F;

	// the total time that has passed.
	float totalTime = 0.0F;

protected:

};

