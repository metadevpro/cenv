using System;
using System.IO;

namespace Metadev.cenv
{
    public class ArgsParser
    {
        public static ArgsOptions Parse(string[] args)
        {
            var options = new ArgsOptions();
            if (args.Length == 0) 
            {
                options.Command = Command.Help;
                options.Valid = true;
                return options;
            }

            var index = 0;
            var position = 0;
            while (index < args.Length)
            {
                var argItem = args[index++];
                if (argItem == "--dry-run")
                {
                    options.DryRun = true;
                }
                else if (argItem == "-o")
                {
                    if (index + 1 <= args.Length)
                    {
                        var nextOption = args[index++];
                        options.OutputFile = nextOption;
                    }
                    else
                    {
                        options.Valid = false;
                        options.ExitCode = 3;
                        Console.WriteLine("-o option should provide a target file.");
                        return options;
                    }
                }
                else if (argItem == "--missing" || argItem == "-m")
                {
                    if (index + 1 <= args.Length)
                    {
                        var nextOption = args[index++];
                        options.ToBeDefinedKeyword = nextOption;
                    }
                    else
                    {
                        options.Valid = false;
                        options.ExitCode = 4;
                        Console.WriteLine("--missing option should provide a missing keyword. Defaults: TBD.");
                        return options;
                    }
                }
                else
                {
                    if (position == 0)
                    {
                        options.Command = GetCommand(args[0]);
                        position++;
                        if (options.Command == Command.None)
                        {
                            options.Valid = false;
                            options.ExitCode = 5;
                            Console.WriteLine("Invalid command specified: '{0}' ", args[0]);
                            Console.WriteLine("For help type:  cenv help");
                            return options;
                        }
                    }
                    else if (position == 1)
                    {
                        options.EnvFile = argItem;
                        position++;
                    }
                    else if (position == 2)
                    {
                        options.TemplateFile = argItem;
                        position++;
                    }
                }
            }
            // Validation
            if (!string.IsNullOrEmpty(options.EnvFile))
            {
                if (!File.Exists(options.EnvFile)) 
                {
                    options.Valid = false;
                    options.ExitCode = 4;
                    Console.WriteLine("File '{0}' not found.", options.EnvFile);
                    return options;
                }
            }
            if (!string.IsNullOrEmpty(options.TemplateFile))
            {
                if (!File.Exists(options.TemplateFile))
                {
                    options.Valid = false;
                    options.ExitCode = 4;
                    Console.WriteLine("File '{0}' not found.", options.TemplateFile);
                    return options;
                }
            }

            options.Valid = true;
            return options;
        }

        public static Command GetCommand(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return Command.None;
            }
            switch (cmd.ToLower())
            {
                case "h":
                case "help":
                    return Command.Help;
                case "a":
                case "apply":
                    return Command.Apply;
                case "c":
                case "check":
                case "v":
                case "verify":
                case "d":
                case "diff":
                    return Command.Check;
                default:
                   return Command.None;
            }
        }
    }
}
