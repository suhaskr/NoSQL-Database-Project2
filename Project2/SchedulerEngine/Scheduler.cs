///////////////////////////////////////////////////////////////
// Scheduler.cs - define methods for scheduling the          //
//                  persisting of database contents.         //
// Ver 1.0                                                   //
// Application: Demonstration for CSE681-SMA, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell Inspiron 15, Core-i5, Windows 10        //
// Author:      Suhas Kamasetty Ramesh                       //
//              MS Computer Engineering, Syracuse University //
//              (315) 278-3888 skamaset@syr.edu              //
///////////////////////////////////////////////////////////////
/*
 * Public Interface:
 * -----------------
 *  start() - Method used to trigger the scheduler to save the database.
 *            The database is saved after the thread has waited for timer interval of time.
 *            This saving process will continue after waiting for every timer interval, until it is stopped. 
 *
 *  stop() - Method used to stop the above saving process.
 *
 *
 * Package Operations:
 * --------------------
 *  This package implements the methods that are used
 *  to schedule the persisting of the database.
 *  Once the scheduling is triggered the saving process
 *  will continue until it is explicitly stopped.
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: Scheduler.cs, DBElement.cs, DBEngine.cs
 *                 Display.cs, PersistEngine.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv ProjectTwo.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History
 * --------------------
 * ver 1.0 : 9 Oct 2015
 * First release
 *
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Timers;

namespace ProjectTwo
{
    public class Scheduler<Key, Value, Data>
    {
        private Timer schedular { get; set; } = new Timer();
        private PersistEngine pe = new PersistEngine();

        //-------< Constructor for the scheduler class that will set the interval time to default of 3 sec >-------------
        public Scheduler(DBEngine<Key, Value> db, int num = 3000)
        {
            schedular.Interval = num;
            schedular.AutoReset = true;
            schedular.Elapsed += (object src, ElapsedEventArgs e) =>
            {
                WriteLine("\n  Going to persist the database contents now:");
                string file_name = pe.persistDB<Key, Value, Data>(db);
                WriteLine("  XML file is successfully saved at {0}", file_name.Substring(13));
            };
        }

        //-------< Trigger the scheduler to invoke saving process >-------------
        public void start()
        {
            schedular.Enabled = true;
        }

        //-------< Stop the saving process  >-----------------------------------
        public void stop()
        {
            schedular.Enabled = false;
        }

    }

#if(TEST_SCHEDULER)

    public class TestScheduler {

        //-------< Test stub  >---------------------------------------------------
        static void Main(string[] args)
        {
            "Testing Scheduler Package".title('=');
            WriteLine();
            DBElement<int, string> elem1 = new DBElement<int, string>("Element-1", "Description of Element-1");
            elem1.payload = "Payload of element-1.";
            elem1.children.AddRange(new List<int> { 9, 10, 11 });
            DBElement<int, string> elem2 = new DBElement<int, string>("Element-2", "Description of Element-2");
            elem2.payload = "Payload of element-2.";
            DBElement<int, string> elem3 = new DBElement<int, string>("Element-3", "Description of Element-3");
            elem3.payload = "Payload of element-3.";

            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            db.insert(1, elem1);
            db.insert(2, elem2);
            db.insert(3, elem3);
            Write("The database contents are: ");
            db.showDB();

            WriteLine("Going to trigger the scheduler to save database after every 4 seconds.");
            Scheduler<int, DBElement<int, string>, string> schedule_test = new Scheduler<int, DBElement<int, string>, string>(db, 4000);
            schedule_test.start();
            System.Threading.Thread.Sleep(10000);
            schedule_test.stop();
            WriteLine("Stopped the scheduler after 10seconds. So the database contents are persisted twice.");
            WriteLine();
        }
    }
#endif
}
