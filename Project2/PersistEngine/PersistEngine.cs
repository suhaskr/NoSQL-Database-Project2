﻿///////////////////////////////////////////////////////////////
// PersistEngine.cs - define methods for saving and reading  //
//                    database contents from XML             //
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
 *  string generate_file_name() - Generate file name for the XML file to be stored in the SavedXMLFiles folder.
 * 
 *  string persistDB(DBEngine<Key, Value> db) - Save all the contents in the database to a single XML file, return path to file.
 *
 *  persistDBelement(DBElement<Key, Data> elem) - Used by Convert each DBElement to an XML Element.
 *
 *  loadDB(DBEngine<Key, Value> db, string file_name) - Parse data from given XML file and load to database. 
 *
 *
 * Package Operations:
 * --------------------
 *  This package implements the methods that are used 
 *  to save the database contents to an XML file and
 *  to parse data from XML file and load it to database.
 *   
 */
/*
 * Maintenance:
 * ------------
 * Required Files: PersistEngine.cs, DBElement.cs, DBEngine.cs
 *                 Display.cs, UtilityExtensions.cs, DBExtensions.cs
 *
 * Build Process:  devenv ProjectTwo.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History
 * --------------------
 * ver 1.0 : 10 Oct 2015
 *  - first release
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
    public class PersistEngine
    {
        //-------< Generate file name for the XML file to be stored. >---------------------------
        public string generate_file_name()
        {
            StringBuilder file_name = new StringBuilder();
            file_name.Append("./../../../../SavedXMLFiles/");
            file_name.Append("SavedXML_");
            file_name.Append(DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_"));
            file_name.Append(".xml");
            return file_name.ToString();
        }

        //-------< Save all the contents in the database to a single XML file. >---------------------------
        public string persistDB<Key, Value, Data>(DBEngine<Key, Value> db)
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XComment comment = new XComment("This is a No-SQL Database.");
            xml.Add(comment);
            XElement noSqlDb = new XElement("NoSqlDb");
            XElement keyType = new XElement("KeyType", typeof(Key));
            XElement payloadType = new XElement("PayloadType", typeof(Data));
            xml.Add(noSqlDb);
            noSqlDb.Add(keyType);
            noSqlDb.Add(payloadType);

            //-------< Save the Key and DBElement in a single XML Element called Key-Value-Pair >---------
            foreach (Key k in db.Keys())
            {
                XElement pair = new XElement("Key-Value-Pair");
                XElement key = new XElement("Key", k);
                pair.Add(key);
                Value val1;
                db.getValue(k, out val1);

                DBElement<Key, Data> elem = val1 as DBElement<Key, Data>;
                XElement dbelem = persistDBelement<Key, Data>(elem);

                pair.Add(dbelem);
                noSqlDb.Add(pair);
            }
            string file_name = generate_file_name();
            xml.Save(file_name);
            return file_name;

        }

        //-------< Save name,description, children list and payload into a single XML Element. >----------
        public XElement persistDBelement<Key, Data>(DBElement<Key, Data> elem)
        {
            XElement element = new XElement("Element");
            XElement name = new XElement("Name", elem.name);
            XElement descr = new XElement("Descr", elem.descr);
            element.Add(name);
            element.Add(descr);
            if (elem.children.Count() > 0)
            {
                XElement children = new XElement("Children");
                foreach (Key key in elem.children)
                {
                    XElement k = new XElement("Key", key);
                    children.Add(k);
                }
                element.Add(children);
            }
            XElement time = new XElement("timestamp", elem.timeStamp);
            element.Add(time);
            if (elem.payload != null)
            {
                XElement payload = new XElement("payload", elem.payload);
                element.Add(payload);
            }
            return element;
        }

        //-------< Read the XML File from the path provided and load it to the database.  >--------
        public void loadDB<Key, Value, Data>(DBEngine<Key, Value> db, string file_name)
        {
            try
            {
                
                XDocument xml = XDocument.Load(file_name);
                XElement root = xml.Element("NoSqlDb");

                if (String.Compare(root.Element("KeyType").Value, typeof(Key).ToString()) == 0)
                {
                    if (String.Compare(root.Element("PayloadType").Value, typeof(Data).ToString()) == 0)
                    {
                        //Read each element at a time and add it to the database.
                        foreach (var kv_pair in root.Elements("Key-Value-Pair"))
                        {
                            try
                            {
                                XElement xml_dbElement = kv_pair.Element("Element");
                                Key k = (Key)Convert.ChangeType(kv_pair.Element("Key").Value, typeof(Key));

                                DBElement<Key, Data> elem = new DBElement<Key, Data>();
                                elem.name = xml_dbElement.Element("Name").Value;
                                elem.descr = xml_dbElement.Element("Descr").Value;
                                elem.timeStamp = DateTime.Parse(xml_dbElement.Element("timestamp").Value);
                                if (xml_dbElement.Element("payload") != null)
                                    elem.payload = (Data)Convert.ChangeType(xml_dbElement.Element("payload").Value, typeof(Data));

                                bool children_present = false;
                                List<Key> child_list = new List<Key>();
                                if (xml_dbElement.Element("Children") != null)
                                {
                                    children_present = true;
                                    foreach (var child_key in xml_dbElement.Element("Children").Elements("Key"))
                                    {
                                        child_list.Add((Key)Convert.ChangeType(child_key.Value, typeof(Key)));
                                    }

                                }
                                if (children_present)
                                    elem.children = child_list;

                                Value val = (Value)Convert.ChangeType(elem, typeof(Value));
                                db.insert(k, val);
                            }

                            catch (NullReferenceException)
                            {
                                WriteLine("\n  XML elements of a particular DBElement is not in the expected format. A particular tag could not be found.");
                                WriteLine("  Cannot add this DBElement to database. Skipping to parsing of the next DBElement.");
                            }
                            catch (System.Xml.XmlException)
                            {
                                WriteLine("\n  An error occured while reading the XML File. Skipping to parse the next DB Element.");
                            }
                        }
                    }
                    else
                        WriteLine("The payload type of database is not same as the payload type of elements in XML file.");
                }
                else
                    WriteLine("The Key-type of database is different from the Key-type present in XML file. Cannot Load database.");
            }
            catch (System.IO.FileNotFoundException)
            {
                WriteLine("\n  XML File not found. Could not load it to database.");
            }
            catch (NullReferenceException ex)
            {
                WriteLine("\n  XML file is not in the expected format. A particular tag could not be found.");
                WriteLine("  Cannot load the XML file to database. ex = {0}", ex);
            }
            catch (XmlException)
            {
                WriteLine("\n  An error occured while readin the XML File. Could not load the data base contents.");
            }

        }
    }


#if (TEST_PERSISTENGINE)
    class Test_PersistEngine
    {
        static void Main(string[] args)
        {
            "Testing DBExtensions Package".title('=');
            WriteLine();

            DBElement<int, string> elem1 = new DBElement<int, string>("Element-7", "Description of Element-7");
            elem1.payload = "Payload of element-7.";
            elem1.children.AddRange(new List<int> { 8, 9 });
            DBElement<int, string> elem2 = new DBElement<int, string>("Element-8", "Description of Element-8");
            //elem2.payload = "Payload of element-8.";
            DBElement<int, string> elem3 = new DBElement<int, string>("Element-9", "Description of Element-9");
            elem3.payload = "Payload of element-3.";

            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            db.insert(7, elem1);
            db.insert(8, elem2);
            db.insert(9, elem3);

            Write("  Created a new dataBase  with following contents:");
            db.showDB();
            WriteLine("\n\n  Now going to persist the database contents to an XML file.");
            PersistEngine pe = new PersistEngine();
            string file_name = pe.persistDB<int, DBElement<int, string>, string>(db);
            WriteLine("  Database contents are saved as - {0}", file_name);
            WriteLine("\n  Going to remove all DB elements and load the xml file saved above.");
            db.remove(7);
            db.remove(8);
            db.remove(9);
            WriteLine("  DB contents before calling load-DB");
            db.showDB();
            WriteLine("  Now send command to load database from the XML file.");
            pe.loadDB<int, DBElement<int, string>, string>(db, file_name);
            Write("  DB contents after calling load-DB ");
            db.showDB();
        }
    }
#endif
}
