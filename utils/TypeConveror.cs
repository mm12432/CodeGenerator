namespace code_generator.utils
{
    public class TypeConveror
    {
        public static string ToCSharpType(string type)
        {
            return type;
        }

        public static string ToTypeScriptType(string type)
        {
            switch (type)
            {
                case "string":
                    return "string";
                case "double":
                case "int":
                    return "Number";
                case "Datetime":
                    return "Date | string | undefined";
                default:
                    return "string";
            }
        }
    }
}