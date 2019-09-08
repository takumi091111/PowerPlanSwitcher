using CommandLine;
using System;
using System.Linq;

namespace PowerPlanSwitcher
{
    class Client
    {
        string[] args;

        public Client(string[] args)
        {
            this.args = args;
        }

        public void Run()
        {
            if (0 < args.Length)
            {
                ImmediateMode();
                return;
            }

            InteractiveMode();
        }

        private void ImmediateMode()
        {
            PowerPlan[] availablePlans = PowerPlanManager.GetPowerPlans();

            CommandLine.Parser.Default.ParseArguments<Parser.Options>(args)
                .WithParsed(o =>
                {
                    if (o.Guid != null && o.Plan != null) return;

                    if (o.Guid != null)
                    {
                        bool isValidGuid = Guid.TryParse(o.Guid, out Guid guid);
                        if (!isValidGuid) return;

                        PowerPlan[] searchResult = availablePlans.Where(p => p.Guid == guid).ToArray();
                        bool planExists = 0 < searchResult.Length;
                        if (!planExists) return;

                        PowerPlanManager.PowerSetActiveScheme(guid);
                        return;
                    }

                    if (o.Plan != null)
                    {
                        PowerPlan plan;

                        switch (o.Plan)
                        {
                            case "powersave":
                                plan = PowerPlan.POWERSAVE;
                                break;
                            case "balanced":
                                plan = PowerPlan.BALANCED;
                                break;
                            case "performance":
                                plan = PowerPlan.PERFORMANCE;
                                break;
                            default:
                                return;
                        }

                        PowerPlanManager.PowerSetActiveScheme(plan.Guid);
                        return;
                    }
                });
        }

        private void InteractiveMode()
        {
            PowerPlan[] availablePlans = PowerPlanManager.GetPowerPlans();
            PowerPlan currentPlan = PowerPlanManager.GetCurrentPowerPlan();

            Console.WriteLine($"現在の電源プラン: {currentPlan.Name} ({currentPlan.Guid})\n");

            availablePlans
                .Select((p, i) => $"{i + 1}: {availablePlans[i].Name} ({availablePlans[i].Guid})")
                .ToList()
                .ForEach(Console.WriteLine);

            Console.Write("\n変更したいプランの番号: ");

            if (!int.TryParse(Console.ReadLine(), out int result)) return;
            if (result <= 0 && availablePlans.Length < result) return;

            PowerPlan selectedPlan = availablePlans[result - 1];
            PowerPlanManager.PowerSetActiveScheme(selectedPlan.Guid);

            Console.WriteLine($"\n{selectedPlan.Name}へ変更しました。");
        }
    }
}
