namespace iskkonekb.kuvera.model
{
    public enum SysCategory
    {
        initSaldo
    }

    public class Category
    {
        private string _Code;
        private string _Name;

        /*public string Code { get { return _Code; } }
        public string Name { get { return _Name; } }*/
        public Category(string code, string name)
        {
            this._Code = code;
            this._Name = name;
        }
    }
}