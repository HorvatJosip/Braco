using Braco.Utilities.Extensions;
using System;
using System.Reflection;

namespace Braco.Utilities.Wpf
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetCallingAssembly().FullName);
            Console.WriteLine(args.Join(", "));
        }
    }
}
