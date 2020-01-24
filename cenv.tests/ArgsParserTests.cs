using Xunit;
using Metadev.cenv;

namespace cenv.tests
{
    public class ArgsParserTests
    {
        [Fact()]
        public void Should_Parse_Help_As_Default()
        {
            var res = ArgsParser.Parse(new string[] { });
            Assert.True(res.Valid);
            Assert.Equal(Command.Help, res.Command);
        }
        [Fact()]
        public void Should_Parse_Help()
        {
            var res = ArgsParser.Parse(new string[] { "help" });
            Assert.True(res.Valid);
            Assert.Equal(Command.Help, res.Command);
        }
        [Fact()]
        public void Should_Parse_Help_2()
        {
            var res = ArgsParser.Parse(new string[] { "h" });
            Assert.True(res.Valid);
            Assert.Equal(Command.Help, res.Command);
        }
        [Fact()]
        public void Should_Parse_Apply()
        {
            var res = ArgsParser.Parse(new string[] { "apply", "test-data/prod.toml", "test-data/template.config", "-o", "test-data/output.config", "--dry-run" });
            Assert.True(res.Valid);
            Assert.Equal(Command.Apply, res.Command);
            Assert.Equal("test-data/prod.toml", res.EnvFile);
            Assert.Equal("test-data/template.config", res.TemplateFile);
            Assert.Equal("test-data/output.config", res.OutputFile);
            Assert.True(res.DryRun);
        }
        [Fact()]
        public void Should_Parse_Check()
        {
            var res = ArgsParser.Parse(new string[] { "check", "test-data/prod.toml", "test-data/output.config" });
            Assert.True(res.Valid);
            Assert.Equal(Command.Check, res.Command);
            Assert.Equal("test-data/prod.toml", res.EnvFile);
            Assert.Equal("test-data/output.config", res.TemplateFile);
            Assert.Null(res.OutputFile);
            Assert.False(res.DryRun);
        }
        [Fact()]
        public void Should_Parse_Error()
        {
            var res = ArgsParser.Parse(new string[] { "unknown"});
            Assert.False(res.Valid);
        }
    }
}
