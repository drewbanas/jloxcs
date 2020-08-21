using System.Collections.Generic;

namespace jloxcs
{
    class LoxInstance
    {
        private LoxClass klass;
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public LoxInstance(LoxClass klass)
        {
            this.klass = klass;
        }

        public object get(Token name)
        {
            if (fields.ContainsKey(name.lexeme))
            {
                return fields[name.lexeme];
            }

            LoxFunction method = klass.findMethod(name.lexeme);
            if (method != null)
                return method.bind(this);// method;

            throw new RuntimeError(name, "Undefined property '" + name.lexeme + "'.");
        }

        public void set(Token name, object value)
        {
            if (fields.ContainsKey(name.lexeme))
                fields[name.lexeme] = value;
            else
                fields.Add(name.lexeme, value); // fields.put(name.lexeme, value); Java replaces existing
        }

        public override string ToString()
        {
            return klass.name + " instance";
        }
    }
}