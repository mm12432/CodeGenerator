using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using code_generator.models;
using code_generator.utils;
using System.Text;

namespace code_generator
{
    class Program
    {
        static string defaultSource = "source";

        static string defaultTarget = "target";

        static string cs = "cs";

        static string ts = "ts";

        static string ns;

        static string csOrNot;

        static string tsOrNot;

        static void CreateDir()
        {
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.MODELS))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.MODELS);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.REPOSITORIES))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.REPOSITORIES);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.REPOSITORIES + '/' + CSharpGenerator.INTERFACES))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.REPOSITORIES + '/' + CSharpGenerator.INTERFACES);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.CONTROLLERS))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.CONTROLLERS);
            }
        }

        static void Interact()
        {
            Console.WriteLine("欢迎使用代码生成工具！");
            while (string.IsNullOrEmpty(ns))
            {
                System.Console.WriteLine("请输入C#代码的命名空间");
                ns = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(csOrNot) || csOrNot.Trim() != "Y" && csOrNot.Trim() != "n")
            {
                System.Console.WriteLine("是否要生成C#代码[Y/n]");
                csOrNot = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(tsOrNot) || tsOrNot.Trim() != "Y" && tsOrNot.Trim() != "n")
            {
                System.Console.WriteLine("是否要生成Typescript代码[Y/n]");
                tsOrNot = Console.ReadLine();
            }
        }
        
        static void Main(string[] args)
        {
            //创建目标目录
            CreateDir();
            //人机交互
            Interact();
            string[] files = Directory.GetFiles(defaultSource);
            foreach (var file in files)
            {
                using (var stream = File.OpenText(file))
                {
                    var jsonText = stream.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<ModelDeclare>(jsonText);

                    string fileNameModel = string.Format("{0}/{1}/{2}/{3}.cs", defaultTarget, cs, CSharpGenerator.MODELS, obj.Name);
                    if (File.Exists(fileNameModel))
                    {
                        File.Delete(fileNameModel);
                    }
                    using (var streamWriter = File.CreateText(fileNameModel))
                    {
                        streamWriter.Write(CSharpGenerator.GenerateModelText(obj, ns));
                    }

                }

            }
        }
    }
}
