namespace Metadev.cenv
{
    public enum Command {
        None = 0,
        Help,
        Apply,
        Check
    }
    public class ArgsOptions
    {
        public string EnvFile;
        public string TemplateFile;
        public string OutputFile;
        public bool DryRun;
        public Command Command;
        public bool Valid;
        public int ExitCode;
        public string ToBeDefinedKeyword;

        public ArgsOptions()
        {
            ToBeDefinedKeyword = "TBD";
        }
    }
}
