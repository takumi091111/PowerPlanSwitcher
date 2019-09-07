using System;
using CommandLine;
using System.Linq;

namespace PowerPlanSwitcher
{
    class Client
    {
        string[] args;
        bool isImmediate;

        public Client(string[] args)
        {
            this.args = args;
            this.isImmediate = (0 < args.Length);
        }

        public class Options
        {
            [Option('g', "guid", Required = false, HelpText = "変更したい電源プランのGUID")]
            public string Guid { get; set; }
        }

        public void Run()
        {
            if (this.isImmediate)
            {
                ImmediateMode();
                return;
            }
            InteractiveMode();
        }

        private void ImmediateMode()
        {
            PowerPlan[] availablePlans = PowerPlan.GetPowerPlans();

            Parser.Default.ParseArguments<Options>(this.args)
                .WithParsed(o =>
                {
                    bool isValidGuid = Guid.TryParse(o.Guid, out Guid guid);
                    if (!isValidGuid) return;

                    PowerPlan[] searchResult = availablePlans.Where(p => p.Guid == guid).ToArray();
                    bool planExists = 0 < searchResult.Length;
                    if (!planExists) return;

                    PowerPlan.PowerSetActiveScheme(guid);
                });
        }

        private void InteractiveMode()
        {
            PowerPlan[] availablePlans = PowerPlan.GetPowerPlans();
            PowerPlan currentPlan = PowerPlan.GetCurrentPowerPlan();

            Console.WriteLine($"現在の電源プラン: {currentPlan.Name} ({currentPlan.Guid})\n");

            availablePlans
                .Select((p, i) => $"{i + 1}: {availablePlans[i].Name} ({availablePlans[i].Guid})")
                .ToList()
                .ForEach(Console.WriteLine);

            Console.Write("\n変更したいプランの番号: ");

            if (!int.TryParse(Console.ReadLine(), out int result)) return;
            if (result <= 0 && availablePlans.Length < result) return;

            PowerPlan selectedPlan = availablePlans[result - 1];
            PowerPlan.PowerSetActiveScheme(selectedPlan.Guid);

            Console.WriteLine($"\n{selectedPlan.Name}へ変更しました。");
        }
    }
}
