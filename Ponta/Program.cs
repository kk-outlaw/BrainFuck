using System.IO;
using Core;

namespace Ponta
{
    class Program
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
				"んほー！！", //>
				"んほーー！！", //<
				"んほーーー！！", //+
				"んほーーーー！！", //-
				"んほーーーーー！！", //.
				"んほーーーーーー！！", //,
				"んほーーーーーーー！！", //[
				"んほーーーーーーーー！！" //]
			};

			var interpreter = new Interpreter(cmd, words);
			interpreter.Execute();
		}
	}
}
