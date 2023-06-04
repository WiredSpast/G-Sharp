namespace GSharp.Misc
{
    public class StringifyAble
    {
        public interface IStringifyAble
        {
            string Stringify();
            void ConstructFromString(string str);
        }
    }
}
