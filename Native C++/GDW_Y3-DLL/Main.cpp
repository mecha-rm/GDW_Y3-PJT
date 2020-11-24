// Main.cpp : This file contains the 'main' function. Program execution begins and ends there.
// This project isf or testing DLLs. When the DLL is complete, it will be put into its own project.

#include <iostream>
#include "Timer.h"

int main()
{
    std::cout << "Hello World!\n";

    // countdown timer test
    {
        CountdownTimer cdt;
        cdt.SetStartTime(10.0F);
        
        // while the countdown is continuing.
        while (!cdt.IsFinished())
        {
            std::cout << "Countdown Time: " << cdt.GetCurrentTime() << std::endl;
            cdt.UpdateTimer(1.0F);
        }

        std::cout << "Countdown Time: " << cdt.GetCurrentTime() << std::endl;
        cdt.SetStartTime(5.0F);
        std::cout << "New Countdown Time: " << cdt.GetCurrentTime() << std::endl;
        cdt.UpdateTimer(0.5F);
        cdt.UpdateTimer(0.25F);
        std::cout << "New Countdown Time: " << cdt.GetCurrentTime() << std::endl;
        cdt.SetStartTime(10.0F);
        std::cout << "New Countdown Time: " << cdt.GetStartTime() << std::endl;
        std::cout << "New Countdown Time: " << cdt.GetCurrentTime() << std::endl;
        std::cout << std::endl;
    }

    // stopwatch timer test
    {
        StopwatchTimer swt;
        
        // stopwatch timer
        swt.UpdateTimer(1.0F);
        swt.UpdateTimer(1.0F);
        swt.UpdateTimer(1.0F);

        std::cout << "Stopwatch Current Time: " << swt.GetCurrentTime() << std::endl;

        swt.Split();
        swt.UpdateTimer(1.0F);
        swt.UpdateTimer(1.0F);
        
        swt.Split();

        swt.UpdateTimer(1.0F);
        swt.UpdateTimer(1.0F);
        
        std::cout << "Split Count: " << swt.GetSplitCount() << std::endl;
        std::cout << "Stopwatch Current Time: " << swt.GetCurrentTime() << std::endl;
        for (int i = 0; i < swt.GetSplitCount(); i++)
        {
            std::cout << "Stopwatch Split [" << i << "]: " << swt.GetSplit(i) << std::endl;
        }
        std::cout << "Stopwatch Total Time: " << swt.GetTotalTime() << std::endl;
        swt.ResetTimer();
        std::cout << "Reset Stopwatch Time: " << swt.GetCurrentTime() << std::endl;
        std::cout << "Reset Stopwatch Split: " << swt.GetSplitCount() << std::endl;
        std::cout << std::endl;
    }

    system("pause");
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
