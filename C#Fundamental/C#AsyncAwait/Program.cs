using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
namespace C_Sharp_Ex3_AsyncAwait
{
    class Program: Menu
    {
        static async Task Main(string[] args)
        {
            await Menu.RunMenu();
        }
    }
}
