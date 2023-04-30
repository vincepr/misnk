// See https://aka.ms/new-console-template for more information

using Minsk.CodeAnalysis;

namespace Minsk
{
    // 1 + 2 * 3
    // gets parsed into a treel like:
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("#showTree : to enable parse-tree information");
            Console.WriteLine("#cls : clear the Terminal");
            bool showTree = false;
            while (true)
            {
                // get input
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                // pseudo commands for custom flags
                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "ENABLED showing parse-tree." : "disabled showing parse-tree");
                    continue;
                } else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                // create tree
                var syntaxTree = SyntaxTree.Parse(line);

                // optional showing of parse-tree
                if (showTree)
                {
                Console.ForegroundColor = ConsoleColor.Yellow;
                PrettyPrint(syntaxTree.Root);
                Console.ResetColor();
                }

                // print diagnostics OR result
                if (syntaxTree.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var d in syntaxTree.Diagnostics)
                        Console.WriteLine(d);
                    Console.ResetColor();
                }
                else
                {
                    var eval = new Evaluator(syntaxTree.Root);
                    var result = eval.Evaluate();
                    Console.WriteLine(result.ToString());
                }
            }
        }

        // format node-tree like:
        //   ├─PlusToken
        //   └─NumberExpression
        //     └─NumberToken 2
        static void PrettyPrint( SyntaxNode node, string indent="", bool isLast=true)
        {
            Console.Write(indent);
            Console.Write(isLast ? "└─" : "├─");
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }
            Console.WriteLine() ;
            indent += isLast ? "  " : "│ ";
            var lastChild = node.GetChildren().LastOrDefault(); 
            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child==lastChild);
        }
    }
}



