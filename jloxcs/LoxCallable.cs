using System.Collections.Generic;

namespace jloxcs
{
    interface LoxCallable
    {
        int arity();
        object call(Interpreter interpreter, List<object> arguments);
    }
}
