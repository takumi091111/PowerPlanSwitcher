using System;

namespace PowerPlanSwitcher
{
    class PowerPlan
    {
        public static PowerPlan POWERSAVE = new PowerPlan(
            new Guid("a1841308-3541-4fab-bc81-f71556f20b4a"),
            "powersave"
        );

        public static PowerPlan BALANCED = new PowerPlan(
            new Guid("381b4222-f694-41f0-9685-ff5bb260df2e"),
            "balanced"
        );

        public static PowerPlan PERFORMANCE = new PowerPlan(
            new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"),
            "performance"
        );

        /// <summary>
        /// 電源プランのGUID
        /// </summary>
        public Guid Guid { get; }
        /// <summary>
        /// 電源プラン名
        /// </summary>
        public string Name { get; }

        public PowerPlan(Guid guid, string name)
        {
            this.Guid = guid;
            this.Name = name;
        }
    }
}
