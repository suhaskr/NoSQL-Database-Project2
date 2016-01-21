///////////////////////////////////////////////////////////////
// TestExec.cs - Test Requirements for Project #2            //
// Ver 1.2                                                   //
// Application: Demonstration for CSE681-SMA, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell Inspiron 15, Core-i5, Windows 10        //
// Author:      Suhas Kamasetty Ramesh                       //
//              MS Computer Engineering, Syracuse University //
//              (315) 278-3888 skamaset@syr.edu              //
// Source:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
///////////////////////////////////////////////////////////////
/*
 * Public Interface:
 * -----------------
 * This package does not contain methods that are used by other packages.
 *
 * Package Operations:
 * -------------------
 * This package demonstrates all the requirements.
 * 
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   TestExec.cs,  DBElement.cs, DBEngine, Display, ItemEditor.cs
 *   DBExtensions.cs, UtilityExtensions.cs, QueryEngine.cs
 *
 * Build Process:  devenv ProjectTwo.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : 9 Oct 15
 * - added code for the demonstration of requirements
 * ver 1.1 : 24 Sep 15
 * ver 1.0 : 18 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Xml;
using System.Xml.Linq;

namespace ProjectTwo
{
    class TestExec
    {
        private DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
        private DBEngine<string, DBElement<string, List<string>>> db2 = new DBEngine<string, DBElement<string, List<string>>>();
        public DateTime tm1, tm2;

        void TestR2()
        {
            "Demonstrating Requirement #2".title();
            DBElement<int, string> elem1 = new DBElement<int, string>();
            Write("\n --- DBElement<int,string> ---");
            elem1.name = "Element-1";
            elem1.descr = "DB Element whose key type is integer and payload type is string.";
            elem1.timeStamp = DateTime.Now;
            elem1.children.AddRange(new List<int> { 9, 10, 11 });
            elem1.payload = "Elements payload - It is of string type.";
            elem1.showElement();
            db.insert(1, elem1);
            WriteLine();

            DBElement<int, string> elem2 = new DBElement<int, string>();
            elem2.name = "Element-2";
            elem2.descr = "DB Element with no children.";
            elem2.payload = "Elements payload is of string type.";
            elem2.showElement();
            db.insert(2, elem2);

            Write("\n\n --- DBEngine<int,string> ---");
            db.showDB();

            Write("\n\n --- DBElement<string,List<string>> ---");
            DBElement<string, List<string>> elem_str = new DBElement<string, List<string>>();
            elem_str.name = "Element-One";
            elem_str.descr = "DB Element whose key type is string and payload type is List of strings.";
            elem_str.timeStamp = DateTime.Now;
            elem_str.children = new List<string> { "Two", "Three", "Four" };
            elem_str.payload = new List<string> { "Element payload is of type List of strings.", "This is string two.", "And third" };
            elem_str.showEnumerableElement();

            DBElement<string, List<string>> elem_str2 = new DBElement<string, List<string>>();
            elem_str2.name = "Element-Two";
            elem_str2.descr = "Description of second element.";
            elem_str2.children = new List<string> { "Ten", "Eleven", "Twelve" };
            elem_str2.payload = new List<string> { "Alpha", "Beta", "Gamma" };
            WriteLine();
            elem_str2.showEnumerableElement(); 

            db2.insert("One", elem_str);
            db2.insert("Two", elem_str2);
            Write("\n\n --- DBEngine<string,List<string>> ---");
            db2.showEnumerableDB();
            WriteLine();
        }
        void TestR3()
        {
            "Demonstrating Requirement #3".title();
            WriteLine();
            Write("  The following are the Database contents presently.(Two elements which were added in previous requirement)");
            db.showDB();
            DBElement<int, string> elem3 = new DBElement<int, string>("Element-3", "Newly added element.");
            elem3.payload = "Payload of element 3.";
            DBElement<int, string> elem4 = new DBElement<int, string>("Element-4", "Newly added element.");
            elem4.payload = "Payload of element 4.";
            DBElement<int, string> elem5 = new DBElement<int, string>("Element-5", "Will not be added to database.");

            bool add1 = db.insert(3, elem3);
            bool add2 = db.insert(4, elem4);
            bool add3 = db.insert(1, elem5); 
            //This element will not be added to the database because there is already
            //  an element in the database with key=1, hence elem5 will not be added to the database.

            Write("\n\n  The following are the Database contents after trying to add three new elements with key=3, key=4 and key=1");
            Write("\n  Adding new element with key=1 will fail because there is an element with key=1 present in database.");
            db.showDB();
            if (add1 && add2 && add3)
                WriteLine("\n  All elements added to database properly.");
            else
                WriteLine("\n  Adding of elements to database failed in one of the cases.");

            bool del1 = db.remove(2);
            bool del2 = db.remove(77);
            //Since there is no element in the database with key=77, it will return false.

            Write("\n  The following are the Database contents after trying to delete two elements with key=2 and key=77");
            Write("\n  Deleting element with key=77 will fail because, there is no element in the database with key=77");
            db.showDB();
            if (del1 && del2)
                WriteLine("\n  Both elements deleted from database successfully");
            else
                WriteLine("\n  Could not delete one of the elements as it was not present in the database.");
            WriteLine();
        }
        void TestR4()
        {
            System.Threading.Thread.Sleep(1500);
            tm1 = DateTime.Now;
            System.Threading.Thread.Sleep(1500);
            "Demonstrating Requirement #4".title();
            //System.Threading.Thread.Sleep(3000);
            Write("\n  The following are the Database contents presently.(Three elements)");
            db.showDB();
            bool rel1 = db.addRelation<int, DBElement<int, string>, string>(1, 3);
            bool rel2 = db.addRelation<int, DBElement<int, string>, string>(3, 4);
            bool rel3 = db.addRelation<int, DBElement<int, string>, string>(3, 88); 
            //Child relationship of 5 will not be added to element-3 because there is no element with key=5.
                                                                        
            Write("\n\n  The following are the database contents after adding child relationships to element-1(3) and element-3(4,88)");
            Write("\n  Adding child relationship of 88 to element-3 will fail because there is no element in database with key=88");
            db.showDB();
            if (rel1 && rel2 && rel3)
                WriteLine("\n  Child relationship added successfully in all three cases.");
            else
                WriteLine("\n  Adding of child relationship failed in one of the cases.");


            bool rem1 = db.removeRelation<int, DBElement<int, string>, string>(1, 11);
            bool rem2 = db.removeRelation<int, DBElement<int, string>, string>(1, 10);
            bool rem3 = db.removeRelation<int, DBElement<int, string>, string>(1, 99);
            //Element-1 does not have 99 in its list of children, hence when we try to 
            // delete 99 from element-1 it will return false.

            WriteLine("\n\n  The following are the database contents after deleting child relationships of 10, 11 and 99 from element-1");
            Write("  Removing of childrelation with 99 from element1 will fail because 99 is not present in child list of element1.");
            db.showDB();
            if (rem1 && rem2 && rem3)
                WriteLine("\n  Child relationship removed successfully in all three cases.");
            else
                WriteLine("\n  Removing of child relationship failed in one of the cases.");
            

            Write("\n  Now going to test edition of name, description and replacing instance of payload with new instance in element1.");
            db.editName<int, DBElement<int, string>, string>(1, "Elemen-1_Renamed.");
            db.editDescr<int, DBElement<int, string>, string>(1, "Edited description for element 1.");
            db.editInstance<int, DBElement<int, string>, string>(1, "New instance of payload for element-1.");
            db.showDB();
            WriteLine();
            System.Threading.Thread.Sleep(1500);
            tm2 = DateTime.Now;
        }
        void TestR5()
        {
            "Demonstrating Requirement #5".title();
            WriteLine("\n  As shown above there are three elements in the dabase.");
            WriteLine("  Send command to persist database contents to an XML File.");
            
            PersistEngine pe = new PersistEngine();
            WriteLine();
            //pe.persistDB<string, DBElement<string, List<string>>, List<string>>(db2);
            string file_name = pe.persistDB<int, DBElement<int, string>, string> (db);
            WriteLine("  XML file is successfully saved at {0}",file_name.Substring(13));

            XDocument xml = XDocument.Load(file_name);
            WriteLine(xml.ToString());

            WriteLine("\n  Now will remove all the DBElements from the database and load the XML File.");
            db.remove(1);
            db.remove(3);
            db.remove(4);
            Write("  Now the database is empty: ");
            db.showDB();

            WriteLine("\n\n  Send command to load the database from the XML file saved above at - {0}", file_name.Substring(13));
            pe.loadDB<int, DBElement<int, string>, string>(db, file_name);

            WriteLine("  Database contents after loading the XML file:");
            Write("  We can see that all three elements with key=1, 3 and 4 are added in the database.");
            db.showDB();

            string append_file = "./../../../../SavedXMLFiles/Append_test.xml";
            WriteLine("\n\n  Now demonstrate appending of the existing database contents with data from a different XML file saved at - {0}",append_file.Substring(13));
            pe.loadDB<int, DBElement<int, string>, string>(db, append_file);

            WriteLine("  The database contents after loading the XML file are shown below:");
            Write("  We can see that three new elements with key=7, 8 and 9 are added in the database.");
            db.showDB();

            WriteLine();
        }
        void TestR6()
        {
            "Demonstrating Requirement #6".title();
            WriteLine("\n  Trigger the scheduler to save the database contents to XML files every 5 seconds. ");
            WriteLine("  This saving process will continue until it is explicitly stopped.");
            //This will schedule the interval to 5 seconds. If 5000 is not passed, by default interval is set to 3 seconds.
            Scheduler<int, DBElement<int, string>, string> schedule = new Scheduler<int, DBElement<int, string>, string>(db, 5000);

            //This will trigger the scheduled event to be invoked.
            schedule.start();

            //Wait for 12 seconds, in the mean time, scheduler will save the database contents two times.
            System.Threading.Thread.Sleep(12000);

            WriteLine("\n  Stop the scheduled saving process of the database.");
            schedule.stop();
            WriteLine();
        }
        void TestR7()
        {
            "Demonstrating Requirement #7".title();
            //Going to remove the 3 elements that were appended in requirement-5.
            db.remove(7);
            db.remove(8);
            db.remove(9);
            Write("\n  Going to send queries to the following Database:");
            db.showDB();
            QueryEngine sendQuery = new QueryEngine();
            DBElement<int, string> result_elem = new DBElement<int, string>();

            Write("\n\n  Query -> Find element with key = 1");
            if (sendQuery.findValue<int, DBElement<int, string>, string>(db, 1) != null)
                sendQuery.findValue<int, DBElement<int, string>, string>(db, 1).showElement();
            else
                WriteLine("\n  Element with key = 1 not found in database.");

            Write("\n\n  Query -> Find element with key = 77");
            if (sendQuery.findValue<int, DBElement<int, string>, string>(db, 77) != null)
                sendQuery.findValue<int, DBElement<int, string>, string>(db, 77).showElement();
            else
                WriteLine("\n  Element with key = 77 not found in database.");

            WriteLine("\n  Query -> Get children of element with key = 1");
            if (sendQuery.getChildren<int, DBElement<int, string>, string>(db, 1).Count > 0)
            {
                Write("  Children of element with key = 1 are ");
                foreach (int i in sendQuery.getChildren<int, DBElement<int, string>, string>(db, 1))
                    Write("{0} ", i);
            }
            else
                WriteLine("\n Element with key = 1 has no children.");

            WriteLine("\n\n  Query -> Get children of element with key = 4");
            if (sendQuery.getChildren<int, DBElement<int, string>, string>(db, 4).Count > 0)
            {
                Write("  Children of element with key = 4 are ");
                foreach (int i in sendQuery.getChildren<int, DBElement<int, string>, string>(db, 4))
                    Write("{0} ", i);
            }
            else
                WriteLine("  Element with key = 4 has no children.");

            string start_pat = "3";
            WriteLine("\n  Query -> Get all keys starting with pattern \"3\"");
            List<int> result_pat = sendQuery.get_keys_with_pattern<int, DBElement<int, string>, string>(db, start_pat);
            Write("  Keys containing pattern \"3\" in thei keys are: ");
            foreach (int i in result_pat)
                Write("{0} ", i);

            string start_pat2 = "8"; 
            WriteLine("\n\n  Query -> Get all keys starting with pattern \"8\"");
            List<int> result_pat2 = sendQuery.get_keys_with_pattern<int, DBElement<int, string>, string>(db, start_pat2);
            Write("  Keys containing pattern \"8\" in thei keys are: ");
            foreach (int i in result_pat2)
                Write("{0} ", i);
            WriteLine("\n  Since there are no keys starting with \"8\" all the keys in the database are returned.");

            List<string> search_pat = new List<string> { "UnKnown", "Newly" };
            foreach (string pattern_in_metadata in search_pat) {
                WriteLine("\n  Query -> Get all keys containing string - \"{0}\" in their metadata section.", pattern_in_metadata);
                List<int> res1 = sendQuery.get_keys_with_metadata<int, DBElement<int, string>, string>(db, pattern_in_metadata);
                if (res1.Count > 0)
                {
                    Write("  Keys containing string \"{0}\" in their metadata section are: ", pattern_in_metadata);
                    foreach (int i in res1)
                        Write("{0} ", i);
                }
                else
                    WriteLine("  No keys found containing pattern \"{0}\" in their metadata section.", pattern_in_metadata);
            }

            WriteLine("\n\n  Query -> Get all keys written within specified intervals {0} and {1}", tm1, tm2);
            List<int> result1 = sendQuery.get_keys_within_timeInterval<int, DBElement<int, string>, string>(db, tm1, tm2);
            if (result1.Count > 0)
            {
                Write("  Keys written within specified time interval are: ",tm1,tm2);
                foreach (int i in result1)
                    Write("{0} ", i);
            }
            else
                WriteLine("  No keys found written withing specified time interevals tm1 and tm2.");

            //System.Threading.Thread.Sleep(1500);
            WriteLine("\n\n  Query -> Get all keys written after {0}", tm1);
            List<int> result2 = sendQuery.get_keys_within_timeInterval<int, DBElement<int, string>, string>(db, tm1);
            if (result1.Count > 0)
            {
                Write("  Keys written after specified timeinterval {0} are: ", tm1);
                foreach (int i in result2)
                    Write("{0} ", i);
            }
            else
                WriteLine("  No keys found written withing specified time interevals tm1 and tm2.");

            WriteLine();
        }
        void TestR8()
        {
            "Demonstrating Requirement #8".title();
            WriteLine();
        }
        void TestR9()
        {
            "Demonstrating Requirement #9".title();
            WriteLine("\n\n  Going to create a new databse and load the XML file containing package structure present in the SavedXMLFiles folder.");
            DBEngine<int,DBElement<int,string>> packageDb = new DBEngine<int, DBElement<int, string>>();

            string pkg_file = "./../../../../SavedXMLFiles/PackageStructure.xml";
            XDocument pkg_xml = XDocument.Load(pkg_file);
            WriteLine(pkg_xml.ToString());

            Write("\n\n  The database contents after loading the above XML file are: ");
            PersistEngine pkg_pe = new PersistEngine();

            pkg_pe.loadDB<int, DBElement<int, string>, string>(packageDb, pkg_file);
            packageDb.showDB();


            WriteLine();
        }
        static void Main(string[] args)
        {
            TestExec exec = new TestExec();
            "Demonstrating Project#2 Requirements".title('=');
            WriteLine();
            exec.TestR2();
            exec.TestR3();
            exec.TestR4();
            exec.TestR5();
            exec.TestR6();
            exec.TestR7();
            exec.TestR8();
            exec.TestR9();
            Write("\n\n");
            //`  Console.ReadKey();
        }
    }
}
