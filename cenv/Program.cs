using System;

namespace Metadev.cenv
{
    public class Program
    {
        const string Version = "0.1.0";
        public static int Main(string[] args)
        {
            try
            {
                var options = ArgsParser.Parse(args);
                if (!options.Valid)
                {
                    return options.ExitCode;
                }
                switch (options.Command)
                {
                    case Command.Help:
                        return Help();
                    case Command.Apply:
                        return Apply.Execute(options);
                    case Command.Check:
                        return Check.Execute(options);
                    case Command.None:
                    default:
                        return ErrorCodes.NO_COMMAND_SPECIFIED;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                return ErrorCodes.INTERNAL_EXCEPION;
            }
        }
        static int Help()
        {
            Console.WriteLine("cenv " + Version);
            Console.WriteLine("");
            Console.WriteLine("Create Environments files patching *.config files with env data using TOML files.");
            Console.WriteLine("Usage:     cenv <command> <env.file.toml> <template.config> [options]");
            Console.WriteLine("  Apply env:");
            Console.WriteLine("    cenv apply prod.toml web-template.config -o web.config");
            Console.WriteLine("");
            Console.WriteLine("  Simulate application:");
            Console.WriteLine("    cenv apply prod.toml web-template.config --dry-run -o web.config");
            Console.WriteLine("");
            Console.WriteLine("  Verify/Diff:");
            Console.WriteLine("    cenv check prod.toml web.config");
            Console.WriteLine("");
            Console.WriteLine("  Help:");
            Console.WriteLine("    cenv help");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("   -o <outfile>           Output file");
            Console.WriteLine("");
            Console.WriteLine("   --dry-run              Simulate application.");
            Console.WriteLine("");
            Console.WriteLine("   -m");
            Console.WriteLine("   --missing <keyword>    Defines the value for missing values on check. Default: TBD.");
            return ErrorCodes.NO_ERROR;
        }        
    }
}
