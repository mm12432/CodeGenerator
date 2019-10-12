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
        // static string appName;
        static string[] names;

        static void CreateDirs()
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
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.COMMON))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.COMMON);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.SERVICES))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.SERVICES);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.SERVICES + '/' + CSharpGenerator.INTERFACES))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.SERVICES + '/' + CSharpGenerator.INTERFACES);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.APPCONTEXT))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.APPCONTEXT);
            }
            if (!Directory.Exists(defaultTarget + '/' + cs + '/' + CSharpGenerator.VIEWMODELS))
            {
                Directory.CreateDirectory(defaultTarget + '/' + cs + '/' + CSharpGenerator.VIEWMODELS);
            }
        }

        static void Interact()
        {
            Console.WriteLine("欢迎使用代码生成工具！");
            // while (string.IsNullOrEmpty(appName))
            // {
            //     System.Console.WriteLine("请输入app的名称");
            //     appName = Console.ReadLine();
            // }
            while (string.IsNullOrEmpty(ns))
            {
                System.Console.WriteLine("请输入C#代码的命名空间");
                ns = Console.ReadLine();
            }
            // while (string.IsNullOrEmpty(csOrNot) || csOrNot.Trim() != "Y" && csOrNot.Trim() != "n")
            // {
            //     System.Console.WriteLine("是否要生成C#代码[Y/n]");
            //     csOrNot = Console.ReadLine();
            // }
            // while (string.IsNullOrEmpty(tsOrNot) || tsOrNot.Trim() != "Y" && tsOrNot.Trim() != "n")
            // {
            //     System.Console.WriteLine("是否要生成Typescript代码[Y/n]");
            //     tsOrNot = Console.ReadLine();
            // }
        }

        static void CreateBaseFiles()
        {
            CreateGuidEntityFile();
            CreateBaseRepositoryFile();
            CreateBaseRepositoryInterfaceFlie();
            CreateBaseServiceInterfaceFlie();
            CreateAppContextFile();
            CreateAutoMapperConfigFile();
        }

        static void CreateFile(string fileName, string fileContent)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (var streamWriter = File.CreateText(fileName))
            {
                streamWriter.Write(fileContent);
            }
        }

        static void CreateGuidEntityFile()
        {
            string fileName = string.Format("{0}/{1}/{2}/GuidEntity.cs", defaultTarget, cs, CSharpGenerator.COMMON);
            string fileContent = CSharpGenerator.GenerateGuidEntityText(ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateBaseRepositoryFile()
        {
            string fileName = string.Format("{0}/{1}/{2}/BaseRepository.cs", defaultTarget, cs, CSharpGenerator.REPOSITORIES);
            string fileContent = CSharpGenerator.GenerateBaseRepositoryText(ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateBaseRepositoryInterfaceFlie()
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}/IBaseRepository.cs", defaultTarget, cs, CSharpGenerator.REPOSITORIES, CSharpGenerator.INTERFACES);
            string fileContent = CSharpGenerator.GenerateBaseRepositoryInterfaceText(ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateBaseServiceInterfaceFlie()
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}/IBaseService.cs", defaultTarget, cs, CSharpGenerator.SERVICES, CSharpGenerator.INTERFACES);
            string fileContent = CSharpGenerator.GenerateBaseServiceInterfaceText(ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateAppContextFile()
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}.cs", defaultTarget, cs, CSharpGenerator.APPCONTEXT, CSharpGenerator.APPCONTEXT);
            string fileContent = CSharpGenerator.GenerateContextText(names, ns);
            CreateFile(fileName, fileContent);
        }


        static void CreateAutoMapperConfigFile()
        {
            string fileName = string.Format("{0}/{1}/AutoMapperConfigs.cs", defaultTarget, cs);
            string fileContent = CSharpGenerator.GenerateAutoMapperConfigText(names, ns);
            CreateFile(fileName, fileContent);
        }

        static void GetNames()
        {
            string[] files = Directory.GetFiles(defaultSource);
            names = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                using (var stream = File.OpenText(files[i]))
                {
                    var jsonText = stream.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<ModelDeclare>(jsonText);
                    names[i] = obj.Name;
                }
            }
        }
        static void CreateModelFile(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}.cs", defaultTarget, cs, CSharpGenerator.MODELS, model.Name);
            string fileContent = CSharpGenerator.GenerateModelText(model, ns);
            CreateFile(fileName, fileContent);
        }
        static void CreateViewModelFile(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}.cs", defaultTarget, cs, CSharpGenerator.VIEWMODELS, model.Name);
            string fileContent = CSharpGenerator.GenerateViewModelText(model, ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateRepositoryFile(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}Repository.cs", defaultTarget, cs, CSharpGenerator.REPOSITORIES, model.Name);
            string fileContent = CSharpGenerator.GenerateRepositoryText(model.Name, ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateRepositoryInterfaceFlie(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}/I{4}Repository.cs", defaultTarget, cs, CSharpGenerator.REPOSITORIES, CSharpGenerator.INTERFACES, model.Name);
            string fileContent = CSharpGenerator.GenerateRepositoryInterfaceText(model.Name, ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateServiceFile(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}Service.cs", defaultTarget, cs, CSharpGenerator.SERVICES, model.Name);
            string fileContent = CSharpGenerator.GenerateServiceText(model.Name, ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateServiceInterfaceFlie(ModelDeclare model)
        {
            string fileName = string.Format("{0}/{1}/{2}/{3}/I{4}Service.cs", defaultTarget, cs, CSharpGenerator.SERVICES, CSharpGenerator.INTERFACES, model.Name);
            string fileContent = CSharpGenerator.GenerateServiceInterfaceText(model.Name, ns);
            CreateFile(fileName, fileContent);
        }

        static void CreateBusinessFiles()
        {
            string[] files = Directory.GetFiles(defaultSource);
            foreach (var file in files)
            {
                using (var stream = File.OpenText(file))
                {
                    var jsonText = stream.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<ModelDeclare>(jsonText);
                    CreateModelFile(obj);
                    CreateViewModelFile(obj);
                    CreateRepositoryFile(obj);
                    CreateRepositoryInterfaceFlie(obj);
                    CreateServiceFile(obj);
                    CreateServiceInterfaceFlie(obj);
                }

            }
        }

        static void Main(string[] args)
        {
            //创建目标目录
            CreateDirs();
            //人机交互
            Interact();
            //获得model名字
            GetNames();
            //建立基础文件
            CreateBaseFiles();
            //建立业务文件
            CreateBusinessFiles();
        }
    }
}
