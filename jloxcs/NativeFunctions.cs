using System.Collections.Generic;

namespace jloxcs
{
    /*
     * Csharp had some problems defining the clockFunction
     * directly in Interpreter.globals.define's arugments
     */
    class NativeFunctions // can also just be a namespace?
    {
        public class clockFunction : LoxCallable
        {
            public int arity()
            {
                return 0;
            }
            public object call(Interpreter interprter, List<object> arguments)
            {
                return (double)System.Environment.TickCount / 1000.0;
            }
            public string toString()
            {
                return "<native fn>";
            }
        }
    }
}
