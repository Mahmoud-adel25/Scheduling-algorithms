using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace OperatingSystems_Scheduler
{
    public partial class CPU_Scheduler : Form
    {
        int numberOfProcesses;
        string type;
        double q;
        Button btn_st;
        TextBox quantum;
        TextBox interrupt;
        TextBox[] names;
        TextBox[] arrivals;
        TextBox[] bursts;
        TextBox[] priorities;
        TextBox newname;
        TextBox newburst;
        TextBox newpriority;
        Chart timeline;
        int counter;
        Label waiting;
        Scheduler cpu = new Scheduler();
        Stack<Control> objects = new Stack<Control>();
        Scheduler newProcesses = new Scheduler();
        public CPU_Scheduler()
        {

            InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
            timeline = new Chart();
            waiting = new Label();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                numberOfProcesses = Int32.Parse(txt_num.Text);
                if (numberOfProcesses <= 0)
                    throw new System.Exception("Number of Processes must be positive integer");
                type = cmb_select.SelectedItem.ToString();
                names = new TextBox[numberOfProcesses];
                arrivals = new TextBox[numberOfProcesses];
                bursts = new TextBox[numberOfProcesses];
                priorities = new TextBox[numberOfProcesses];
                quantum = new TextBox();
                interrupt = new TextBox();
                TextBox newname = new TextBox();
                TextBox newburst = new TextBox();
                TextBox newpriority = new TextBox();
                AddNewProcess(numberOfProcesses, type);
                btn_OK.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is Exception)
                {
                    string message = "Invalid Input Data \nPlease select a Scheduler and Enter a positive Integer";
                    string title = "ERROR";
                    // Show message box
                    MessageBox.Show(ex.Message + message, title);
                }
            }
        }

        void AddNewProcess(int n, string t)
        {
            if (n <= 0) throw new System.Exception("Number of Processes must be Positive integer");
            if (t == "First Come First Served" || t == "Shortest Job first (P)" || t == "Shortest Job first (NP)" || t == "Priority (P)" || t == "Priority (NP)" || t == "Round Robin")
            {
                System.Windows.Forms.Label l1 = new System.Windows.Forms.Label();
                this.Controls.Add(l1);
                l1.Top = 80;
                l1.Left = 10;
                l1.Text = "Name ";
                System.Windows.Forms.Label l2 = new System.Windows.Forms.Label();
                this.Controls.Add(l2);
                l2.Top = 80;
                l2.Left = 160;
                l2.Text = "Arrival Time";
                System.Windows.Forms.Label l3 = new System.Windows.Forms.Label();
                this.Controls.Add(l3);
                l3.Top = 80;
                l3.Left = 310;
                l3.Text = "Burst Time";
                if (t == "Priority (P)" || t == "Priority (NP)")
                {
                    System.Windows.Forms.Label l4 = new System.Windows.Forms.Label();
                    this.Controls.Add(l4);
                    l4.Top = 80;
                    l4.Left = 460;
                    l4.Text = "Priority";
                }
                for (int i = 0; i < n; i++)
                {
                    System.Windows.Forms.TextBox txt1 = new System.Windows.Forms.TextBox();
                    names[i] = txt1;
                    this.Controls.Add(txt1);
                    txt1.Top = (i * 30) + 105;
                    txt1.Left = 10;
                    txt1.Text = "P" + (i).ToString();
                    System.Windows.Forms.TextBox txt2 = new System.Windows.Forms.TextBox();
                    arrivals[i] = txt2;
                    this.Controls.Add(txt2);
                    txt2.Top = (i * 30) + 105;
                    txt2.Left = 160;
                    txt2.Text = (i + 1).ToString();
                    System.Windows.Forms.TextBox txt3 = new System.Windows.Forms.TextBox();
                    bursts[i] = txt3;
                    this.Controls.Add(txt3);
                    txt3.Top = (i * 30) + 105;
                    txt3.Left = 310;
                    txt3.Text = (i + 1).ToString();
                    if (t == "Priority (P)" || t == "Priority (NP)")
                    {
                        System.Windows.Forms.TextBox txt4 = new System.Windows.Forms.TextBox();
                        priorities[i] = txt4;
                        this.Controls.Add(txt4);
                        txt4.Top = (i * 30) + 105;
                        txt4.Left = 460;
                        txt4.Text = (i + 1).ToString();
                    }

                }
                if (t == "Round Robin")
                {
                    System.Windows.Forms.Label l5 = new System.Windows.Forms.Label();
                    this.Controls.Add(l5);
                    l5.Top = 80;
                    l5.Left = 460;
                    l5.Text = "Quantum Time ";
                    System.Windows.Forms.TextBox txt5 = new System.Windows.Forms.TextBox();
                    quantum = txt5;
                    this.Controls.Add(txt5);
                    txt5.Top = 105;

                    txt5.Left = 460;
                    txt5.Text = "1";
                }



                System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
                btn.Name = "btn_start";
                btn.Font = new System.Drawing.Font("Century", 15, System.Drawing.FontStyle.Regular);
                btn.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn);
                btn.Top = (numberOfProcesses * 30) + 120;

                btn.Left = 10;
                btn.Size = new System.Drawing.Size(400, 30);
                btn.Text = "START";
                btn_st = btn;
                btn.Click += new EventHandler(Button_Click_st);


                System.Windows.Forms.Button btn3 = new System.Windows.Forms.Button();
                btn3.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                btn3.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn3);
                btn3.Top = (numberOfProcesses * 30) + 120;
                btn3.Left = 440;
                btn3.Size = new System.Drawing.Size(150, 30);
                btn3.Text = "INTERRUPT";
                btn3.Click += new EventHandler(Button_Click_add);

                System.Windows.Forms.Button b2 = new System.Windows.Forms.Button();
                b2.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                b2.ForeColor = System.Drawing.Color.Maroon;
                b2.Size = new System.Drawing.Size(150, 30);
                this.Controls.Add(b2);
                b2.Top = (numberOfProcesses * 30) + 120;
                b2.Left = 620;
                b2.Text = "UNDO Interrupt";
                b2.Click += new EventHandler(Button_Click_con);

                /* System.Windows.Forms.Button howtouseInt = new System.Windows.Forms.Button();
                 howtouseInt.Font = new System.Drawing.Font("Century", 10, System.Drawing.FontStyle.Regular);
                 howtouseInt.ForeColor = System.Drawing.Color.Maroon;
                 howtouseInt.Size = new System.Drawing.Size(100, 30);
                 this.Controls.Add(howtouseInt);
                 howtouseInt.Top = (numberOfProcesses * 30) + 120;
                 howtouseInt.Left = 690;
                 howtouseInt.Text = "How to Use";
                 howtouseInt.Click += new EventHandler(Button_Click_howtouse);*/



            }
            else throw new System.Exception("Type of Scheduler is invalid");
        }

        private void Button_Click_st(object sender, EventArgs e)
        {
            try
            {
                if (type == "Round Robin")
                {
                    q = Convert.ToDouble(quantum.Text);
                    if (q <= 0)
                        throw new System.Exception("Quantum must be positive integer");
                }
                fillScheduler();
                scheduleCPU();
                btn_st.Enabled = false;

                /*System.Windows.Forms.Button btn3 = new System.Windows.Forms.Button();                
                btn3.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                btn3.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn3);
                btn3.Top = (numberOfProcesses * 30) + 120;
                btn3.Left = 620;
                btn3.Size = new System.Drawing.Size(130, 30);
                btn3.Text = "INTERRUPT";
                btn3.Click += new EventHandler(Button_Click_add);
                System.Windows.Forms.Button b2 = new System.Windows.Forms.Button();
                b2.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                b2.ForeColor = System.Drawing.Color.Maroon;
                b2.Size = new System.Drawing.Size(160, 30);
                this.Controls.Add(b2);                
                b2.Top = (numberOfProcesses * 30) + 120;
                b2.Left = 440;
                b2.Text = "UNDO Interrupt";
                b2.Click += new EventHandler(Button_Click_con);*/
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void FCFS(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = s.sort("arrival");

            node p = temp.getProcesses().getChain();
            if (p == null)
            {
                // Handle empty process list case
                return;
            }

            double i = p.getData().getArrival();
            int counter = 0;
            double totalWaitingTime = 0;
            double increment = p.getData().getArrival();
            double totalTurnAroundTime = 0;

            Series idle = new Series();
            createSeries(idle, i);

            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();

            while (p != null)
            {
                Series barSeries = new Series();
                barSeries.ChartType = SeriesChartType.StackedBar;
                barSeries.XValueType = ChartValueType.String;
                barSeries.Points.AddXY("timeline", p.getData().getDuration());
                barSeries.Label = p.getData().getName();
                timeline.Series.Add(barSeries);

                double waitingTime = i - p.getData().getArrival();
                totalWaitingTime += waitingTime;
                waitingTimes.Add("Process " + p.getData().getName() + " Waiting Time = " + waitingTime.ToString());

                increment += p.getData().getDuration();
                double turnAroundTime = increment - p.getData().getArrival();
                totalTurnAroundTime += turnAroundTime;
                turnAroundTimes.Add("Process " + p.getData().getName() + " Turnaround Time = " + turnAroundTime.ToString());

                i += p.getData().getDuration();

                counter++;
                p = p.getNext();
            }

            // Set waiting display size and final text
            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = " + (totalWaitingTime / counter).ToString();
            waiting.Text += "\nAverage Turnaround Time = " + (totalTurnAroundTime / counter).ToString();
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);

            // Clear CPU (if necessary)
            cpu.clear();
        }



        void SJF_NPrem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();

            double first = temp.getProcesses().getChain().getData().getArrival();
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            double totalWaitingTime = 0;
            double totalTurnAroundTime = 0;
            double bursttimes = 0;
            int flag = 0;
            Series idle = new Series();
            createSeries(idle, i);

            node pro = temp.getProcesses().getChain();
            while (pro != null)
            {
                bursttimes += pro.getData().getDuration();
                pro = pro.getNext();
            }

            while (temp.getProcesses().getChain() != null)
            {
                Scheduler temp2 = new Scheduler();
                while (temp.getProcesses().getChain() != null && temp.getProcesses().getChain().getData().getArrival() <= first)
                {
                    temp2.add(temp.getProcesses().getChain().getData());
                    temp.remove(temp.getProcesses().getChain().getData().getName());
                }
                temp2 = temp2.sort("duration");
                node p2 = temp2.getProcesses().getChain();

                while (p2 != null)
                {
                    Series barSeries = new Series();
                    barSeries.ChartType = SeriesChartType.StackedBar;
                    barSeries.XValueType = ChartValueType.String;
                    barSeries.Points.AddXY("timeline", p2.getData().getDuration());
                    barSeries.Label = p2.getData().getName();
                    timeline.Series.Add(barSeries);

                    double waitingTime = i - p2.getData().getArrival();
                    totalWaitingTime += waitingTime;
                    waitingTimes.Add("Process " + p2.getData().getName() + " Waiting Time = " + waitingTime.ToString());

                    i = i + p2.getData().getDuration();
                    double turnAroundTime = i - p2.getData().getArrival();
                    totalTurnAroundTime += turnAroundTime;
                    turnAroundTimes.Add("Process " + p2.getData().getName() + " Turnaround Time = " + turnAroundTime.ToString());

                    counter++;
                    if (temp.getProcesses().getChain() != null)
                    {
                        temp2.remove(p2.getData().getName());
                        flag = 1;
                        p2 = p2.getNext();
                        break;
                    }
                    p2 = p2.getNext();
                }

                first = i;
                if (flag == 1)
                {
                    while (p2 != null)
                    {
                        temp.add(p2.getData());
                        p2 = p2.getNext();
                    }
                    flag = 0;
                }
                temp2.clear();
            }

            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = " + (totalWaitingTime / counter).ToString();
            waiting.Text += "\nAverage Turnaround Time = " + (totalTurnAroundTime / counter).ToString();
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);

            temp.clear();
            cpu.clear();
        }

        void Priority_NPrem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();

            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");

            node firstNode = temp.getProcesses().getChain();
            if (firstNode == null)
            {
                // Handle empty process list case
                return;
            }

            double first = firstNode.getData().getArrival();
            double i = firstNode.getData().getArrival();
            int counter = 0;
            double totalWaitingTime = 0;
            double totalTurnAroundTime = 0;

            Series idle = new Series();
            createSeries(idle, i);

            double bursttimes = 0;
            node pro = temp.getProcesses().getChain();
            while (pro != null)
            {
                bursttimes += pro.getData().getDuration();
                pro = pro.getNext();
            }

            while (temp.getProcesses().getChain() != null)
            {
                Scheduler temp2 = new Scheduler();
                while (temp.getProcesses().getChain() != null && temp.getProcesses().getChain().getData().getArrival() <= first)
                {
                    temp2.add(temp.getProcesses().getChain().getData());
                    temp.remove(temp.getProcesses().getChain().getData().getName());
                }
                temp2 = temp2.sort("priority");

                node p2 = temp2.getProcesses().getChain();
                while (p2 != null)
                {
                    Series barSeries = new Series();
                    barSeries.ChartType = SeriesChartType.StackedBar;
                    barSeries.XValueType = ChartValueType.String;
                    barSeries.Points.AddXY("timeline", p2.getData().getDuration());
                    barSeries.Label = p2.getData().getName();
                    timeline.Series.Add(barSeries);

                    double waitingTime = i - p2.getData().getArrival();
                    totalWaitingTime += waitingTime;
                    waitingTimes.Add("Process " + p2.getData().getName() + " Waiting Time = " + waitingTime.ToString());

                    i += p2.getData().getDuration();
                    double turnAroundTime = i - p2.getData().getArrival();
                    totalTurnAroundTime += turnAroundTime;
                    turnAroundTimes.Add("Process " + p2.getData().getName() + " Turnaround Time = " + turnAroundTime.ToString());

                    counter++;
                    if (temp.getProcesses().getChain() != null)
                    {
                        temp2.remove(p2.getData().getName());
                        p2 = p2.getNext();
                        break;
                    }
                    p2 = p2.getNext();
                }
                first = i;
                while (p2 != null)
                {
                    temp.add(p2.getData());
                    p2 = p2.getNext();
                }
                temp2.clear();
            }

            // Set waiting display size and final text
            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = " + (totalWaitingTime / counter).ToString();
            waiting.Text += "\nAverage Turnaround Time = " + (totalTurnAroundTime / counter).ToString();
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);

            // Clear CPU (if necessary)
            cpu.clear();
        }

        /// <summary>
        /// /////////////////////
        /// </summary>
        /// <param name="s"></param>

        void SJF_Prem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();
            List<int> interval = new List<int>();

            for (int l = 0; l < numberOfProcesses; l++)
            {
                interval.Add(-1);
            }

            Scheduler temp = s.sort("arrival");

            node firstNode = temp.getProcesses().getChain();
            if (firstNode == null)
            {
                // Handle empty process list case
                return;
            }

            double i = firstNode.getData().getArrival();
            int counter = 0;
            double totalWaitingTime = 0;
            double totalTurnAroundTime = 0;
            Series idle = new Series();
            createSeries(idle, i);

            List<Process> pro = new List<Process>();
            List<Process> departure = new List<Process>();
            List<double> depart = new List<double>();

            // Dictionaries to keep track of waiting times and the last active time of processes
            Dictionary<string, double> waitingTimeDict = new Dictionary<string, double>();
            Dictionary<string, double> lastActiveTimeDict = new Dictionary<string, double>();
            Dictionary<string, double> arrivalTimeDict = new Dictionary<string, double>();

            node pp = temp.getProcesses().getChain();
            double bursttimes = 0;

            // Initialize process lists and dictionaries
            while (pp != null)
            {
                Process t = pp.getData();
                pro.Add(t);
                bursttimes += t.getDuration();
                pp = pp.getNext();
            }

            for (var k = 0; k < pro.Count; k++)
            {
                waitingTimeDict.Add(pro[k].getName(), 0);
                lastActiveTimeDict.Add(pro[k].getName(), -1);
                arrivalTimeDict.Add(pro[k].getName(), pro[k].getArrival());
            }

            while (pro.Count != 0)
            {
                List<Process> pro2 = new List<Process>();

                for (var j = 0; j < pro.Count; j++)
                {
                    if (pro[j].getArrival() <= i)
                    {
                        pro2.Add(pro[j]);
                        pro.RemoveAt(j);
                        j--;
                    }
                }

                // Sort pro2 based on duration, then arrival time
                pro2.Sort((a, b) => a.getDuration() != b.getDuration() ? a.getDuration().CompareTo(b.getDuration()) : a.getArrival().CompareTo(b.getArrival()));

                for (var k = 0; k < pro2.Count; k++)
                {
                    if (pro.Count != 0 && pro2[k].getDuration() + i > pro[0].getArrival())
                    {
                        Series barSeries = new Series
                        {
                            ChartType = SeriesChartType.StackedBar,
                            XValueType = ChartValueType.String
                        };

                        double barDuration = Math.Min(pro[0].getArrival() - i, pro2[k].getDuration());
                        barSeries.Points.AddXY("timeline", barDuration);
                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);

                        pro2[k].setDuration(pro2[k].getDuration() - barDuration);

                        if (pro2[k].getDuration() > 0)
                        {
                            pro.Add(pro2[k]);
                        }

                        // Update last active time
                        lastActiveTimeDict[pro2[k].getName()] = i + barDuration;

                        i = pro[0].getArrival();
                        depart.Add(i);
                        departure.Add(pro2[k]);
                        break;
                    }
                    else
                    {
                        Series barSeries = new Series
                        {
                            ChartType = SeriesChartType.StackedBar,
                            XValueType = ChartValueType.String
                        };

                        barSeries.Points.AddXY("timeline", pro2[k].getDuration());
                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);

                        // Calculate waiting time
                        double lastActiveTime = lastActiveTimeDict[pro2[k].getName()];
                        double waitingTime = lastActiveTime == -1 ? (i - arrivalTimeDict[pro2[k].getName()]) : (i - lastActiveTime);
                        waitingTimeDict[pro2[k].getName()] += waitingTime;

                        // Update last active time
                        lastActiveTimeDict[pro2[k].getName()] = i + pro2[k].getDuration();

                        i += pro2[k].getDuration();
                        double turnAroundTime = i - arrivalTimeDict[pro2[k].getName()];
                        totalTurnAroundTime += turnAroundTime;
                        turnAroundTimes.Add("Process " + pro2[k].getName() + " Turnaround Time = " + turnAroundTime.ToString());

                        counter++;
                        depart.Add(i);
                        departure.Add(pro2[k]);
                        break;
                    }
                }

                if (pro2.Count > 1)
                {
                    for (var l = 1; l < pro2.Count; l++)
                    {
                        if (pro2[l].getDuration() > 0)
                            pro.Add(pro2[l]);
                    }
                }
                pro2.Clear();
            }

            // Calculate total waiting time
            foreach (var kvp in waitingTimeDict)
            {
                totalWaitingTime += kvp.Value;
                waitingTimes.Add("Process " + kvp.Key + " Waiting Time = " + kvp.Value.ToString());
            }

            // Set waiting display size and final text
            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = " + (totalWaitingTime / counter).ToString();
            waiting.Text += "\nAverage Turnaround Time = " + (totalTurnAroundTime / counter).ToString();
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);

            // Clear CPU (if necessary)
            temp.clear();
            cpu.clear();
        }




        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        /// <param name="s"></param>
        /// 

        void Priority_Prem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();
            temp = s.sort("arrival");
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            double totalWaitingTime = 0;
            double totalTurnAroundTime = 0;
            double bursttimes = 0;
            Series idle = new Series();
            createSeries(idle, i);
            List<Process> pro = new List<Process>();
            List<Process> departure = new List<Process>();
            List<double> depart = new List<double>();
            Dictionary<string, int> flags = new Dictionary<string, int>();
            node pp = temp.getProcesses().getChain();

            // Loop through processes and initialize dictionaries and lists
            while (pp != null)
            {
                Process t = pp.getData();
                pro.Add(t);
                pp = pp.getNext();
                bursttimes += t.getDuration();
                flags.Add(t.getName(), 0);
                waitingTimes.Add("Process " + t.getName() + " Waiting Time = ");
                turnAroundTimes.Add("Process " + t.getName() + " Turnaround Time = ");
            }

            // Main scheduling loop
            while (pro.Count != 0)
            {
                Scheduler temp2 = new Scheduler();
                List<Process> pro2 = new List<Process>();

                // Select processes that have arrived by the current time i
                for (var j = 0; j < pro.Count; j++)
                {
                    if (pro[j].getArrival() <= i)
                    {
                        pro2.Add(pro[j]);
                        pro.RemoveAt(j);
                        j--;
                    }
                }

                // Sort pro2 based on priority and arrival time
                pro2.Sort((a, b) => a.getPriority().CompareTo(b.getPriority()) != 0 ? a.getPriority().CompareTo(b.getPriority()) : a.getArrival().CompareTo(b.getArrival()));

                // Schedule processes
                foreach (Process p in pro2)
                {
                    // If the process can complete before the next process arrives
                    if (pro.Count != 0 && p.getDuration() + p.getArrival() > i)
                    {
                        flags[p.getName()] = 1;

                        double waitingTime = i - p.getArrival();
                        waitingTimes[counter] += waitingTime.ToString() + ", ";
                        totalWaitingTime += waitingTime;

                        Series barSeries = new Series
                        {
                            ChartType = SeriesChartType.StackedBar,
                            XValueType = ChartValueType.String
                        };
                        double barDuration = Math.Min(pro[0].getArrival() - i, p.getDuration());
                        barSeries.Points.AddXY("timeline", barDuration);
                        barSeries.Color = p.getColor();
                        barSeries.Label = p.getName();
                        timeline.Series.Add(barSeries);

                        if (pro[0].getArrival() - p.getArrival() < p.getDuration())
                        {
                            i = pro[0].getArrival();
                            depart.Add(i);
                            departure.Add(p);
                        }
                        else
                        {
                            i += p.getDuration();
                            depart.Add(i);
                            departure.Add(p);
                        }

                        counter++;
                    }
                    // If the process cannot complete before the next process arrives
                    else
                    {
                        double waitingTime = i - p.getArrival();
                        waitingTimes[counter] += waitingTime.ToString() + ", ";
                        totalWaitingTime += waitingTime;

                        Series barSeries = new Series
                        {
                            ChartType = SeriesChartType.StackedBar,
                            XValueType = ChartValueType.String
                        };
                        barSeries.Points.AddXY("timeline", p.getDuration());
                        barSeries.Color = p.getColor();
                        barSeries.Label = p.getName();
                        timeline.Series.Add(barSeries);

                        totalTurnAroundTime += i - p.getArrival() + p.getDuration();
                        turnAroundTimes[counter] += (i - p.getArrival() + p.getDuration()).ToString() + ", ";

                        i += p.getDuration();
                        depart.Add(i);
                        departure.Add(p);

                        counter++;
                    }
                }
            }

            // Set waiting display size and final text
            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = " + (totalWaitingTime / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (totalTurnAroundTime / counter).ToString();

            // Display individual waiting and turnaround times
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);


            temp.clear();
            cpu.clear();
        }


        void RoundRobin(Scheduler s, double q)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            List<string> waitingTimes = new List<string>();
            List<string> turnAroundTimes = new List<string>();
            temp = s.sort("arrival");
            node p = temp.getProcesses().getChain();
            double first = p.getData().getArrival();
            double i = p.getData().getArrival();
            double j = 0;
            int counter = 0;
            double average = 0;
            double working = 0;
            double arrival = 0;
            double bursts = 0;
            Series idle = new Series();
            createSeries(idle, i);
            List<Process> pro = new List<Process>();
            Dictionary<string, int> flags = new Dictionary<string, int>();

            // Loop through processes and initialize dictionaries and lists
            while (p != null)
            {
                Process t = p.getData();
                pro.Add(t);
                arrival += t.getArrival();
                bursts += t.getDuration();
                p = p.getNext();
                flags.Add(t.getName(), 0);
                waitingTimes.Add("Process " + t.getName() + " Waiting Time = ");
                turnAroundTimes.Add("Process " + t.getName() + " Turnaround Time = ");
            }

            while (pro.Count != 0)
            {
                flags[pro[0].getName()] = 1;
                j = (pro[0].getDuration() <= q) ? pro[0].getDuration() : q;
                Series barSeries = new Series();
                barSeries.ChartType = SeriesChartType.StackedBar;
                barSeries.XValueType = ChartValueType.String;
                barSeries.Points.AddXY("timeline", j);
                barSeries.Color = pro[0].getColor();
                barSeries.Label = pro[0].getName();
                timeline.Series.Add(barSeries);

                if (pro[0].getDuration() <= q)
                {
                    average += i;
                    i += pro[0].getDuration();
                    counter++;

                    // Calculate waiting time for the process
                    double waitingTime = i - pro[0].getArrival() - pro[0].getDuration();
                    waitingTimes[counter - 1] += waitingTime.ToString() + ", ";
                }
                else
                {
                    i += q;
                    working += q;
                    pro[0].setDuration(pro[0].getDuration() - q);
                    pro[0].setArrival(i);
                    pro.Add(pro[0]);
                }

                pro.RemoveAt(0);

                // Update the order of processes based on arrival time
                pro.Sort((a, b) => a.getArrival().CompareTo(b.getArrival()));

                // Update the waiting and turnaround times for processes
                for (int k = 0; k < pro.Count; k++)
                {
                    if (flags[pro[k].getName()] == 0)
                    {
                        double waitingTime = i - pro[k].getArrival();
                        waitingTimes[k] += waitingTime.ToString() + ", ";
                    }
                    else
                    {
                        double turnAroundTime = i - pro[k].getArrival();
                        turnAroundTimes[k] += turnAroundTime.ToString() + ", ";
                    }
                }
            }

            // Set waiting display size and final text
            waiting.Size = new System.Drawing.Size(counter * 100, 350);  // Adjusted size to fit more text
            waiting.Top = (numberOfProcesses * 30) + 350;
            waiting.Left = 10;
            waiting.Text = "Average Waiting Time = ( " + (average - arrival).ToString() + " - " + working.ToString() + " ) / " + counter.ToString() + " = " + ((average - arrival - working) / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average - arrival - working + bursts).ToString() + " / " + counter.ToString() + " = " + ((average - arrival - working + bursts) / counter).ToString();

            // Display individual waiting and turnaround times
            waiting.Text += "\n\nIndividual Waiting Times:\n" + string.Join("\n", waitingTimes);
            waiting.Text += "\n\nIndividual Turnaround Times:\n" + string.Join("\n", turnAroundTimes);

            cpu.clear();
        }




        private void Button_Click_res(object sender, EventArgs e)
        {
            this.Controls.Clear();
            cpu.clear();
            newProcesses.clear();
            timeline = new Chart();
            waiting = new Label();
            this.InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
        }

        void createTimeline()
        {
            timeline.Size = new System.Drawing.Size(820, 100);
            ChartArea area = new ChartArea(counter.ToString());
            counter++;
            timeline.ChartAreas.Add(area);
            timeline.Top = 220 + (30 * numberOfProcesses);
            this.Controls.Add(timeline);
        }

        void createWaiting()
        {
            waiting.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
            waiting.ForeColor = System.Drawing.Color.Maroon;
            waiting.Top = 275 + (30 * numberOfProcesses);
            waiting.Size = new System.Drawing.Size(500, 20);
            this.Controls.Add(waiting);
        }

        void createSeries(Series idle, double i)
        {
            idle.ChartType = SeriesChartType.StackedBar;
            idle.XValueType = ChartValueType.String;
            idle.Points.AddXY("timeline", i);
            idle.Label = "Idle";
            idle.Color = System.Drawing.Color.White;
            timeline.Series.Add(idle);
        }

        private void Button_Click_int(object sender, EventArgs e)
        {




        }

        private void Button_Click_add(object sender, EventArgs e)
        {

            this.Controls.Remove(timeline);
            this.Controls.Remove(waiting);

            System.Windows.Forms.Label processLabel = new System.Windows.Forms.Label();
            this.Controls.Add(processLabel);
            objects.Push(processLabel);
            processLabel.Top = (numberOfProcesses * 30) + 175;
            processLabel.Left = 10;
            processLabel.Text = "Process Name";

            System.Windows.Forms.TextBox processText = new System.Windows.Forms.TextBox();
            this.Controls.Add(processText);
            objects.Push(processText);
            processText.Top = (numberOfProcesses * 30) + 200;
            processText.Left = 10;
            processText.Text = "New ";
            newname = processText;

            System.Windows.Forms.Label arrivalLabel = new System.Windows.Forms.Label();
            this.Controls.Add(arrivalLabel);
            objects.Push(arrivalLabel);
            arrivalLabel.Top = 175 + (30 * numberOfProcesses); //500
            arrivalLabel.Left = 160;            //100
            arrivalLabel.Text = "Arrival Time";

            System.Windows.Forms.TextBox t2 = new System.Windows.Forms.TextBox();
            this.Controls.Add(t2);
            objects.Push(t2);
            t2.Top = (numberOfProcesses * 30) + 200;
            t2.Left = 160;
            t2.Text = "3";
            interrupt = t2;

            System.Windows.Forms.Label burstLabel = new System.Windows.Forms.Label();
            this.Controls.Add(burstLabel);
            objects.Push(burstLabel);
            burstLabel.Top = 175 + (30 * numberOfProcesses); //500
            burstLabel.Left = 310;            //100
            burstLabel.Text = "Burst Time";

            System.Windows.Forms.TextBox t3 = new System.Windows.Forms.TextBox();
            this.Controls.Add(t3);
            objects.Push(t3);
            t3.Top = (numberOfProcesses * 30) + 200;
            t3.Left = 310;
            t3.Text = "5";
            newburst = t3;

            System.Windows.Forms.Button bt = new System.Windows.Forms.Button();


            if (type == "Priority (P)" || type == "Priority (NP)")
            {
                System.Windows.Forms.Label priorityLabel = new System.Windows.Forms.Label();
                this.Controls.Add(priorityLabel);
                objects.Push(priorityLabel);
                priorityLabel.Top = 175 + (30 * numberOfProcesses); //500
                priorityLabel.Left = 460;            //100
                priorityLabel.Text = "Priority";

                System.Windows.Forms.TextBox t4 = new System.Windows.Forms.TextBox();
                this.Controls.Add(t4);
                objects.Push(t4);
                t4.Top = (numberOfProcesses * 30) + 200;
                t4.Left = 460;
                t4.Text = "1";
                newpriority = t4;


                bt.ForeColor = System.Drawing.Color.Maroon;
                bt.Size = new System.Drawing.Size(150, 25);
                this.Controls.Add(bt);
                objects.Push(bt);
                bt.Top = (numberOfProcesses * 30) + 200;
                bt.Left = 610;
                bt.Text = "ADD Process";
                bt.Click += new EventHandler(Button_Click_new);

            }

            else
            {
                bt.ForeColor = System.Drawing.Color.Maroon;
                bt.Size = new System.Drawing.Size(150, 25);
                this.Controls.Add(bt);
                objects.Push(bt);
                bt.Top = (numberOfProcesses * 30) + 200;
                bt.Left = 460;
                bt.Text = "ADD Process";
                bt.Click += new EventHandler(Button_Click_new);
            }
        }

        private void Button_Click_howtouse(object sender, EventArgs e)
        {
            Tips t = new Tips();
            t.Show();
        }
        private void Button_Click_con(object sender, EventArgs e)
        {
            this.Controls.Remove(timeline);
            this.Controls.Remove(waiting);
            timeline = new Chart();
            waiting = new Label();
            cpu.clear();
            fillScheduler();
            while (objects.Count > 0)
                this.Controls.Remove(objects.Pop());
            objects.Clear();
            scheduleCPU();
        }

        private void Button_Click_new(object sender, EventArgs e)
        {
            try
            {
                timeline = new Chart();
                waiting = new Label();
                Process Pn = new Process();
                Pn.setName(newname.Text);
                Pn.setArrival(Convert.ToDouble(interrupt.Text));
                if (Pn.getArrival() < 0)
                    throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Arrival time must be positive integer");
                Pn.setDuration(Convert.ToDouble(newburst.Text));
                if (Pn.getDuration() <= 0)
                    throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Burst time must be positive integer");
                Random rnd = new Random(18 * System.Environment.TickCount);
                Pn.setColor(System.Drawing.Color.FromArgb(255, rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                if (type == "Priority (P)" || type == "Priority (NP)")
                {
                    Pn.setPriority(Int32.Parse(newpriority.Text));
                    if (Pn.getPriority() <= 0)
                        throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Priority must be positive integer");
                }
                fillScheduler();
                node p = new node();
                p = newProcesses.getProcesses().getChain();
                while (p != null)
                {
                    cpu.add(p.getData());
                    p = p.getNext();
                }
                cpu.add(Pn);
                newProcesses.add(Pn);
                while (objects.Count > 0)
                    this.Controls.Remove(objects.Pop());
                objects.Clear();
                scheduleCPU();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void scheduleCPU()
        {
            if (type == "First Come First Served")
                FCFS(cpu);
            else if (type == "Shortest Job first (NP)")
                SJF_NPrem(cpu);
            else if (type == "Priority (NP)")
                Priority_NPrem(cpu);
            else if (type == "Shortest Job first (P)")
                SJF_Prem(cpu);
            else if (type == "Priority (P)")
                Priority_Prem(cpu);
            else if (type == "Round Robin")
            {
                RoundRobin(cpu, q);
            }
        }

        void fillScheduler()
        {
            for (int i = 0; i < numberOfProcesses; i++)
            {
                Process p = new Process();
                p.setName(names[i].Text);
                p.setArrival(Convert.ToDouble(arrivals[i].Text));
                if (p.getArrival() < 0)
                    throw new System.Exception("Process " + "[ " + p.getName().ToString() + " ]" + " Arrival time must be positive integer");
                p.setDuration(Convert.ToDouble(bursts[i].Text));
                if (p.getDuration() <= 0)
                    throw new System.Exception("Process " + "[ " + p.getName().ToString() + " ]" + " Burst time must be positive integer");
                Random rnd = new Random(i * (System.Environment.TickCount));
                p.setColor(System.Drawing.Color.FromArgb(255, rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                if (type == "Priority (P)" || type == "Priority (NP)")
                {
                    p.setPriority(Int32.Parse(priorities[i].Text));
                    if (p.getPriority() <= 0)
                        throw new System.Exception("Process " + "[ " + p.getName().ToString() + " ]" + " Priority must be positive integer");
                }
                cpu.add(p);
            }
        }

        private void cmb_select_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            cpu.clear();
            newProcesses.clear();
            timeline = new Chart();
            waiting = new Label();
            this.InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Tips t = new Tips();
            t.Show();
        }
    }
}
