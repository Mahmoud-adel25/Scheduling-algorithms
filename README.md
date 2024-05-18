# Scheduling-algorithms
Overview
This project aims to implement several CPU scheduling algorithms to manage process execution in a multi-programming environment. The implemented algorithms will include First Come First Serve (FCFS), Shortest Job First (SJF) in both preemptive and non-preemptive forms, Priority Scheduling in both preemptive and non-preemptive forms, and Round Robin (RR). For each algorithm, the waiting time and turnaround time for each process will be calculated. Additionally, the average waiting time and average turnaround time for all processes will be computed.

Scheduling Algorithms
First Come First Serve (FCFS)

Description: Processes are executed in the order they arrive in the ready queue.
Characteristics: Non-preemptive, simple to implement, may cause long wait times due to the "convoy effect."
Shortest Job First (SJF)

Preemptive (Shortest Remaining Time First, SRTF):
Description: The process with the shortest remaining execution time is selected next.
Characteristics: Preemptive, can lead to lower average waiting time but may cause starvation for longer processes.
Non-Preemptive:
Description: The process with the shortest burst time is selected next and runs to completion.
Characteristics: Non-preemptive, minimizes average waiting time but can lead to starvation.
Priority Scheduling

Preemptive:
Description: The process with the highest priority (smallest integer value) is selected next. If a new process arrives with a higher priority than the currently running process, the running process is preempted.
Characteristics: Preemptive, can ensure important tasks are completed first but may lead to starvation of lower priority processes.
Non-Preemptive:
Description: The process with the highest priority is selected next and runs to completion.
Characteristics: Non-preemptive, simpler but may still cause starvation.
Round Robin (RR)

Description: Processes are executed in a cyclic order for a fixed time quantum.
Characteristics: Preemptive, ensures fair CPU time distribution among processes, suitable for time-sharing systems.
Metrics Calculation
Waiting Time (WT): The total time a process spends waiting in the ready queue.
Turnaround Time (TAT): The total time from the arrival of the process to its completion (TAT = Completion Time - Arrival Time).
For each algorithm, the following will be calculated:

Individual Process Metrics:
Waiting Time for each process.
Turnaround Time for each process.
Overall Metrics:
Average Waiting Time.
Average Turnaround Time.
Implementation Details
Input:

A list of processes, each with attributes: Process ID, Arrival Time, Burst Time, (Priority for priority scheduling).
Output:

A schedule showing the order of process execution.
A table showing each process's Waiting Time and Turnaround Time.
Average Waiting Time and Average Turnaround Time for all processes.
Example
For each algorithm, an example run will demonstrate the calculation of the above metrics using a sample set of processes.
