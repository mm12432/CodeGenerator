using System.Text;
using code_generator.models;

namespace code_generator.utils
{
    public class CSharpGenerator
    {
        public static string MODELS = "Models";
        public static string REPOSITORIES = "Repositories";
        public static string REPOSITORY = "Repository";
        public static string INTERFACES = "Interfaces";
        public static string CONTROLLERS = "Controllers";
        public static string GenerateModelText(ModelDeclare model, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("namespace {0}.{1}", namespaceText, MODELS));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public class {0}", model.Name));
            sb.AppendLine("    {");
            foreach (var prop in model.ModelProperties)
            {
                sb.AppendLine(string.Format("        public {0} {1} {{ get; set; }}", TypeConveror.ToCSharpType(prop.ValueType), prop.Name));
            }
            //sb.AppendLine(TypeConveror.ToCSharpType())
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
