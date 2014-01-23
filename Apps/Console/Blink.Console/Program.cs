using System;
using Blink.Shared.Domain.DataModel.Notes;
using Blink.Shared.Engine;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Server;

namespace Blink.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            Sterling.Activate(() => new PlatformAdapter(), () => new MemoryDriver());

            System.Console.WriteLine("Sterling activated...");

            while (System.Console.ReadKey().Key != ConsoleKey.Escape)
            {
            }

            Sterling.Deactivate();
        }
    }
}
