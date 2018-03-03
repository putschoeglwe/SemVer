namespace SemVer
{
    public abstract class Ident {}
    public class Numeric : Ident
    {
        public Numeric(ulong n)
        {
            Number = n;
        }

        public ulong Number { get; }
    }

    public class AlphaNumeric : Ident
    {
        public AlphaNumeric(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}