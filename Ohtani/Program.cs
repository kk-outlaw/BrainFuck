using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Ohtani
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1 || (args[0] == "-i" && args.Length < 2))
			{
				return;
			}

			string cmd = "";

			if (args[0] != "-i")
			{
				cmd = args[0];
			}
			else
			{
				cmd = File.ReadAllText(args[1]);
			}

			var words = new string[] {
                "大谷翔平", //>
				"翔谷大平", //<
				"大平翔谷", //+
				"大翔谷平", //-
				"平谷大翔", //.
				"谷翔大平", //,
				"翔大谷平", //[
				"谷大平翔"  //]
			};

			var interpreter = new Interpreter(cmd, words);
			interpreter.Execute();
		}
	}
}
