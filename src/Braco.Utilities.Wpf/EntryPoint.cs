using System;
using System.Reflection;

namespace Braco.Utilities.Wpf
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetCallingAssembly().FullName);
            Console.WriteLine(string.Join(", ", args));
        }
    }
}
