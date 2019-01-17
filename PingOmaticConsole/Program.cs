using PingOmaticCore;
using PingOmaticCore.Orchestrator;
using PingOmaticCore.Orchestrator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingOmaticConsole
{
    class Program
    {
        private string FileName { get; set; }
        private bool IsReloadFileOnChange { get; set; }
        private bool NeedsHelp { get; set; }
        Program(string[] args)
        {
            var index = 0;
            while (index < args.Length)
            {
                switch (args[index].ToLower())
                {
                    case "-r":
                        this.IsReloadFileOnChange = true;
                        break;

                    case "--help":
                    case "-h":
                        this.NeedsHelp = true;
                        break;

                    default:
                        this.FileName = args[index];
                        break;
                }
                index++;
            }
        }

        void Run()
        {
            Header();
            if (!ValidateParameters() || NeedsHelp)
                Help();
            else
                Start();
        }

        private void Start()
        {
            var orchestrator = new PingerOrchestrator();
            var configurator = new JSONFileConfiguration(orchestrator, FileName, IsReloadFileOnChange);
            configurator.Configure();

            orchestrator.Start();
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
            orchestrator.Stop();
        }

        private void Help()
        {
            Console.WriteLine("HELP\n----\n");
            Console.WriteLine("PingoMaticConsole [-r] [-h] [--help] <CONFIGURATION_FILE>");
            Console.WriteLine("              -r  Reload on configuration file change");
            Console.WriteLine("    -h or --help  this help");
        }

        private bool ValidateParameters() =>
            this.FileName != null;

        private void Header()
        {
            Console.WriteLine("PingOmatic v. 1.0.0 - luizalbsilva@gmail.com");
            Console.WriteLine("Distributed freely under GPL 3.0");
            Console.WriteLine("\n");
        }

        static void Main(string[] args)
        {
            //new Program(new string[] { "-r", @"C:\Temp\PingOmatic.json" }).Run();
            new Program(new string[] { @"C:\Temp\PingOmatic.json" }).Run();
            //new Program(new string[] { "-r" }).Run();
            //new Program(args).Run();
            Console.ReadKey();
        }
    }

    
}
