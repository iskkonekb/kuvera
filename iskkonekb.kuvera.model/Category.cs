using iskkonekb.kuvera.core;
namespace iskkonekb.kuvera.model
{
    public class Category : ICategory
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