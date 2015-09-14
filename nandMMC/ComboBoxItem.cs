namespace nandMMC
{
    internal class ComboBoxItem
    {
        private readonly string _name;

        public ComboBoxItem(string name, string value)
        {
            _name = name;
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return _name;
        }
    }
}