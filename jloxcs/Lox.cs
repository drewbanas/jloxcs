using System.IO;
using System.Collections.Generic;

namespace jloxcs
{
    class Lox
    {
        private static readonly Interpreter interpreter = new Interpreter();
        static bool hadError = false;
        static bool hadRuntimeError = false;

        static void Main(string[] args)
        {

            if (args.Length > 1)
            {
                System.Console.WriteLine("Usage: lox [script]");
                System.Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
            }
            else
            {
                runPrompt();
            }
        }

        private static void runFile(string path)
        {
            if (!File.Exists(path))
                throw new IOException();

            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
            run(System.Text.Encoding.Default.GetString(bytes, 0, bytes.Length)); 

            // Indicate error in the exit code
            if (hadError) System.Environment.Exit(65);
            if (hadRuntimeError) System.Environment.Exit(70);
        }

        private static void runPrompt()
        {
            for (;;)
            {
                System.Console.Write("> ");
                run(System.Console.ReadLine());
                hadError = false;
            }
        }

        private static void run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();
            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.parse();

            // Stop if there was a syntax error
            if (hadError)
                return;

            Resolver resolver = new Resolver(interpreter);
            resolver.resolve(statements);

            // Stop if there was a resolution error
            if (hadError)
                return;

            interpreter.interpret(statements);
        }

        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            System.Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

        public static void error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        public static void runtimeError(RuntimeError error)
        {
            System.Console.WriteLine(error.Message + "\n[line " + error.token.line + "]");
            hadRuntimeError = true;
        }
    }
}
