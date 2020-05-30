using System.Collections.Generic;

namespace jloxcs
{
    class LoxFunction : LoxCallable
    {
        private readonly Stmt.Function declaration;
        private readonly Environment closure;
        private readonly bool isInitializer;

        public LoxFunction(Stmt.Function declaration, Environment closure, bool isInitializer)
        {
            this.isInitializer = isInitializer;
            this.closure = closure;
            this.declaration = declaration;
        }

        public LoxFunction bind(LoxInstance instance)
        {
            Environment environment = new Environment(closure);
            environment.define("this", instance);
            return new LoxFunction(declaration, environment, isInitializer);
        }

        public string toString()
        {
            return "<fn" + declaration.name.lexeme + ">";
        }

        public int arity()
        {
            return declaration.params_.Count;
        }

        public object call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(closure);// Environment(interpreter.globals);

            for (int i = 0; i < declaration.params_.Count; i++)
            {
                environment.define(declaration.params_[i].lexeme, arguments[i]);
            }

            try
            {
                interpreter.executeBlock(declaration.body, environment);
            }
            catch (Return returnValue)
            {
                if (isInitializer)
                    return closure.getAt(0, "this");

                return returnValue.value;
            }

            if (isInitializer)
                return closure.getAt(0, "this");

            return null;
        }

    }
}
