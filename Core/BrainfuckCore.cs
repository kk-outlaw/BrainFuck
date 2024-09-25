using System;
using System.Collections.Generic;

namespace Core
{
    public class BrainFuckException : Exception
	{
		public BrainFuckException(string message) : base(message)
		{
		}
	}

	public class Interpreter
	{
		private enum Code : int
		{
			IncrementPtr,
			DecrementPtr,
			IncrementValue,
			DecrementValue,
			Output,
			Input,
			While,
			EndWhile,
		}

		private string[] _words =
		{
			">",
			"<",
			"+",
			"-",
			".",
			",",
			"[",
			"]"
		};

		private const int StackSize = 30000;

		private List<Code> _codes;
		private Dictionary<int, int> _ptrFromTo;
		private Dictionary<int, int> _ptrToFrom;
		private string _text;

		public Interpreter(string text)
		{
			_text = text;
		}

		public Interpreter(string text, string[] words) : this(text)
		{
			if (words.Length < 8)
			{
				throw new BrainFuckException("error: 'words' must include 8 words.");
			}

			_words = words;
		}

		public void Execute()
		{
			try
			{
				Compile();
				Run();
			}
			catch (BrainFuckException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void Compile()
		{
			_codes = new List<Code>();
			_ptrFromTo = new Dictionary<int, int>();
			_ptrToFrom = new Dictionary<int, int>();

			Stack<int> ptrFrom = new Stack<int>();

			while (true)
			{
				int currentIndex = Int32.MaxValue;
				string currentWord = "";
				foreach (var word in _words)
				{
					int index = _text.IndexOf(word);
					if (index >= 0)
					{
						if (index < currentIndex)
						{
							currentIndex = index;
							currentWord = word;
						}
					}
				}

				if (currentIndex == Int32.MaxValue)
				{
					break;
				}
				_text = _text.Substring(currentIndex + currentWord.Length, _text.Length - currentIndex - currentWord.Length);

				for (int i = 0; i < _words.Length; i++)
				{
					if (currentWord == _words[i])
					{
						var code = (Code)i;

						if (code == Code.While)
						{
							ptrFrom.Push(_codes.Count);
						}
						else if (code == Code.EndWhile)
						{
							if (ptrFrom.Count <= 0)
							{
								throw new BrainFuckException(String.Format("compile error: too many '{0}'s.", currentWord));
							}

							int from = ptrFrom.Pop();
							_ptrFromTo.Add(from, _codes.Count);
							_ptrToFrom.Add(_codes.Count, from);
						}

						_codes.Add(code);

						break;
					}
				}
			}

			if (ptrFrom.Count > 0)
			{
				throw new BrainFuckException(String.Format("compile error: too many '{0}'s.", _words[6]));
			}
		}

		private void Run()
		{
			var stack = new int[StackSize];
			int ptrIndex = 0;
			int codeIndex = 0;

			while (codeIndex < _codes.Count)
			{
				switch (_codes[codeIndex])
				{
					case Code.IncrementPtr:
						ptrIndex++;

						if (ptrIndex >= StackSize)
						{
							throw new BrainFuckException("runtime error: stack overflow.");
						}

						break;

					case Code.DecrementPtr:
						ptrIndex--;

						if (ptrIndex < 0)
						{
							throw new BrainFuckException("runtime error: stack underflow.");
						}

						break;

					case Code.IncrementValue:
						stack[ptrIndex]++;
						break;

					case Code.DecrementValue:
						stack[ptrIndex]--;
						break;

					case Code.Output:
						Console.Write((System.Char)stack[ptrIndex]);
						break;

					case Code.Input:
						stack[ptrIndex] = Console.Read();
						break;

					case Code.While:
						if (stack[ptrIndex] == 0)
						{
							codeIndex = _ptrFromTo[codeIndex] + 1;
							continue;
						}
						else
						{
							break;
						}

					case Code.EndWhile:
						if (stack[ptrIndex] != 0)
						{
							codeIndex = _ptrToFrom[codeIndex] + 1;
							continue;
						}
						else
						{
							break;
						}

					default:
						break;
				}

				codeIndex++;
			}
		}

		public string Dump()
		{
			if(_codes == null || _codes.Count <= 0)
			{
				return "";
			}

			var str = "";
			foreach(var code in _codes)
			{
				str += code.ToString() + "\n";
			}

			return str;
		}
	}
}
