namespace jloxcs
{
    class Return : System.Exception
    {
        public readonly object value;

        public Return(object value): base()
        {
            // Java super(null, null, false, false);
            this.value = value;
        }
    }
}
