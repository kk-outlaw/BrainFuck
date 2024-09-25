using System.IO;
using Core;

namespace Brainfuck
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

			var interpreter = new Interpreter(cmd);
			interpreter.Execute();
		}
	}
}
