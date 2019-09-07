using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PowerPlanSwitcher
{
    class PowerProf
    {
        [DllImport("powrprof.dll",
            EntryPoint = "PowerSetActiveScheme",
            CharSet = CharSet.Auto,
            SetLastError = true)]
        public static extern uint PowerSetActiveScheme(IntPtr UserRootPowerKey,
            ref Guid SchemeGuid);
    }
}
