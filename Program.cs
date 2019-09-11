using System;

namespace code_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("欢迎使用代码生成工具！");
            System.Console.WriteLine("请输入C#代码的命名空间");
            var ns = Console.ReadLine();
            while (string.IsNullOrEmpty(ns))
            {
                System.Console.WriteLine("请输入C#代码的命名空间");
                ns = Console.ReadLine();
            }
            System.Console.WriteLine("是否要生成C#代码[Y/n]");
            var cs = Console.ReadLine();
            while (cs.Trim() != "Y" && cs.Trim() != "n")
            {
                System.Console.WriteLine("是否要生成C#代码[Y/n]");
                cs = Console.ReadLine();
            }
            System.Console.WriteLine("是否要生成Typescript代码[Y/n]");
            var ts = Console.ReadLine();
            while (ts.Trim() != "Y" && ts.Trim() != "n")
            {
                System.Console.WriteLine("是否要生成Typescript代码[Y/n]");
                ts = Console.ReadLine();
            }
        }
    }
}
