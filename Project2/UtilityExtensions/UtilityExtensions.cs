/////////////////////////////////////////////////////////////////////
// UtilityExtensions.cs - define methods to simplify project code  //
// Ver 1.1                                                         //
// Application: Demonstration for CSE681-SMA, Project#2            //
// Language:    C#, ver 6.0, Visual Studio 2015                    //
// Platform:    Dell Inspiron 15, Core-i5, Windows 10              //
// Author:      Suhas Kamasetty Ramesh                             //
//              MS Computer Engineering, Syracuse University       //
//              (315) 278-3888 skamaset@syr.edu                    //
// Source:      Jim Fawcett, CST 4-187, Syracuse University        //
//              (315) 443-3948, jfawcett@twcny.rr.com              //
/////////////////////////////////////////////////////////////////////
/*
 * Public Interface:
 * -----------------
 *
 * title() - method used to display a title 
 *             (Line below will contain multiple the given character or '-')
 *
 * Package Operations:
 * -------------------
 * This package implements utility extensions that are not specific
 * to a single package.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: UtilityExtensions.cs
 *
 * Build Process:  devenv ProjectTwo.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 : 11 Oct 15
 * - changed name space and added comments and public interface
 *  
 * ver 1.0 : 13 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ProjectTwo
{
  public static class UtilityExtensions
  {
    public static void title(this string aString, char underline = '-')
    {
      Console.Write("\n  {0}", aString);
      Console.Write("\n {0}", new string(underline, aString.Length + 2));
    }
  }


#if(TEST_UTILITYEXTENSIONS)
    public class TestUtilityExtensions
  {
    static void Main(string[] args)
    {
      "Testing UtilityExtensions.title".title();
            "Testing for the second time".title();
            
      Write("\n\n");
    }
  }
#endif
}
