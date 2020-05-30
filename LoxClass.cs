using System.Collections.Generic;

namespace jloxcs
{
    class LoxClass : LoxCallable
    {
        public readonly string name;
        public readonly LoxClass superclass;
        private readonly Dictionary<string, LoxFunction> methods;

        public LoxClass(string name, LoxClass superclass, Dictionary<string, LoxFunction> methods)
        {
            this.superclass = superclass;
            this.name = name;            
            this.methods = methods;
        }

        public LoxFunction findMethod(string name)
        {
            if (methods.ContainsKey(name))
            {
                return methods[name];
            }

            if(superclass != null)
            {
                return superclass.findMethod(name);
            }

            return null;
        }

        public string toString()
        {
            return name;
        }

        public object call(Interpreter interpreter, List<object> arguments)
        {
            LoxInstance instance = new LoxInstance(this);
            LoxFunction initializer = findMethod("init");
            if (initializer != null)
            {
                initializer.bind(instance).call(interpreter, arguments);
            }

            return instance;
        }

        public int arity()
        {
            LoxFunction initializer = findMethod("init");
            if (initializer == null)
                return 0;

            return initializer.arity();
        }
    }
}
