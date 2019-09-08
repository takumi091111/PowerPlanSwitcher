using CommandLine;

namespace PowerPlanSwitcher
{
    class Parser
    {
        public class Options
        {
            [Option('g', "guid", Required = false, HelpText = "変更したい電源プランのGUID")]
            public string Guid { get; set; }

            [Option('p', "plan", Required = false, HelpText = "\"powersave\" or \"balanced\" or \"performance\"")]
            public string Plan { get; set; }
        }
    }
}
