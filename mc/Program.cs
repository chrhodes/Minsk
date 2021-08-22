using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using VNC;

namespace Minsk
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Int64 startTicks = Log.APPLICATION_START($"SignalR Startup Delay", Common.LOG_CATEGORY);
            Thread.Sleep(200);
            startTicks = Log.APPLICATION_START($"Enter", Common.LOG_CATEGORY, startTicks);

            // NOTE(crhodes)
            // Console Directive

            bool showTree = true;
            var variables = new Dictionary<VariableSymbol, object>();

            while (true)
            {
                Console.Write("> ");

                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    Log.APPLICATION_START($"Exit - No Input Received", Common.LOG_CATEGORY, startTicks);

                    return;
                }

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    syntaxTree.Root.WriteTo(Console.Out);
                    //PrettyPrint2(syntaxTree.Root);

                    Console.ResetColor();
                }

                if (!result.Diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(diagnostic);

                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);

                        Console.ResetColor();
                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(error);

                        Console.ResetColor();
                        Console.Write(suffix);
                        Console.WriteLine();
                    }

                    Console.ResetColor();
                    Console.WriteLine();
                }

                Log.APPLICATION_START($"Exit", Common.LOG_CATEGORY, startTicks);
            }
        }


    }
}
