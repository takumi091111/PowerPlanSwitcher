using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace PowerPlanSwitcher
{
    class PowerPlanManager
    {
        private const string PATTERN = @"(?<Guid>(-?[a-z\d])+)\s+\((?<Name>.+)\)";

        /// <summary>
        /// 電源プランの配列を返します。
        /// </summary>
        public static PowerPlan[] GetPowerPlans()
        {
            string output = GetRedirect(@"C:\\Windows\\system32\\powercfg.exe", "/LIST");

            MatchCollection matches = new Regex(PATTERN).Matches(output);
            IEnumerable<PowerPlan> plans = matches.Cast<Match>().Select(m =>
            {
                string guid = m.Groups["Guid"].Value;
                string name = m.Groups["Name"].Value;
                return new PowerPlan(new Guid(guid), name);
            });

            return plans.ToArray();
        }

        /// <summary>
        /// 現在の電源プランを返します。
        /// </summary>
        public static PowerPlan GetCurrentPowerPlan()
        {
            string output = GetRedirect(@"C:\\Windows\\system32\\powercfg.exe", "/GETACTIVESCHEME");

            Match m = new Regex(PATTERN).Match(output);
            string guid = m.Groups["Guid"].Value;
            string name = m.Groups["Name"].Value;
            PowerPlan plan = new PowerPlan(new Guid(guid), name);

            return plan;
        }

        /// <summary>
        /// 電源プランを適用します。
        /// </summary>
        /// <param name="guid">電源プランのGUID</param>
        public static void PowerSetActiveScheme(Guid guid)
        {
            PowerProf.PowerSetActiveScheme(IntPtr.Zero, ref guid);
        }

        /// <summary>
        /// 外部プログラムの標準出力を返します
        /// </summary>
        /// <param name="execFilePath">実行ファイルのパス</param>
        private static string GetRedirect(string execFilePath, string option = null)
        {
            // プロセス起動時の設定情報
            ProcessStartInfo psInfo = new ProcessStartInfo();

            // 実行するファイル
            psInfo.FileName = execFilePath;
            // 引数を設定
            if (option != null)
            {
                psInfo.Arguments = option;
            }

            // コンソールウィンドウを開かない
            psInfo.CreateNoWindow = true;
            // シェル機能を使用しない
            psInfo.UseShellExecute = false;
            // 標準出力をリダイレクト(返す)
            psInfo.RedirectStandardOutput = true;

            // 実行開始
            Process p = Process.Start(psInfo);
            // 標準出力の読み取り
            string output = p.StandardOutput.ReadToEnd();

            // 改行コードの修正
            output = output.Replace("\\r\\r\\n", "\\n");

            return output;
        }
    }
}
