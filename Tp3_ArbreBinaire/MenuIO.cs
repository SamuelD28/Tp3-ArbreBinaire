using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;

namespace SDD
{
    public static class MenuIO
    {
        public static void RapporterBrut<T> (
            string propriété, 
            T valeur, 
            string eol = "\n", 
            ConsoleColor? color = null, 
            int offset = 0, 
            Func<T, string> format = null)
        {
            ForegroundColor = Cyan;
            if(!String.IsNullOrWhiteSpace(propriété))
                Write("{0," + offset + "} : ", propriété);
            else 
                Write("  ");
            ForegroundColor = color ?? Magenta;
            format = format ?? (v => v.ToString());
            Write(format(valeur).PadRight(10));
            ResetColor();
            Write(eol);
        }

        public static void RapporterCalcul<T> (
            string propriété, 
            Func<T> calcul, 
            int benchmarks = 1,
            Func<T, string> format = null)
        {
            try {
                T valeur = default(T);
                long totalTicks = 0;
                int loops = Math.Max(benchmarks, 1);
                for (int i = 0; i < loops; i++)
                {
                    var watch = Stopwatch.StartNew();
                    valeur = calcul();
                    watch.Stop();
                    totalTicks += watch.ElapsedTicks;
                }
                
                RapporterBrut(propriété, valeur, eol: benchmarks > 0 ? "" : "\n", format:format);
                
                if (benchmarks < 1) return;

                if (valeur?.ToString() != "") Write("  ");
                ForegroundColor = DarkYellow;
                var elapsed = 1.0 * totalTicks / benchmarks / Stopwatch.Frequency;
                if (elapsed >= 60)
                    WriteLine("({0:N1} min)", elapsed / 60);
                else if (elapsed >= 1)
                    WriteLine("({0:N1} s)", elapsed);
                else if (elapsed >= 0.001)
                    WriteLine("({0:N1} ms)", elapsed * 1000);
                else if (elapsed >= 0.000_001)
                    WriteLine("({0:N1} us)", elapsed * 1_000_000);
                else 
                    WriteLine("({0:N1} ns)", elapsed * 1_000_000_000);
                ResetColor();
            }
            catch(Exception ex)
            {
                RapporterBrut(propriété, ex.Message, color:Red);
            }
        }

        public static void RapporterAction (
            string description, 
            Action action, 
            int benchmarks = 1 )
        => RapporterCalcul(description, () => { action(); return ""; }, benchmarks);

        public static void 
            RapporterPlusieursCalculs<T, U> (
                IEnumerable<T> items, 
                Func<T, string> description, 
                Func<T, U> calcul, 
                int benchmarks = 1,
                Func<U, string> format = null)
        {
            foreach (var item in items)
            {
                RapporterCalcul(description(item), () => calcul(item), benchmarks, format);
            }
        }

        public static void 
            RapporterPlusieursActions<T> (
                IEnumerable<T> items, 
                Func<T, string> description, 
                Action<T> action, 
                int benchmarks = 1)
        {
            RapporterPlusieursCalculs(items, description, item => { action(item); return ""; }, benchmarks);
        }

        public static string Lire(string propriété)
        {
            var lecture = "";
            while (lecture == "")
            {
                BackgroundColor = White;
                ForegroundColor = Black;
                Write("> " + propriété + " ? ");
                ResetColor();
                Write(" ");
                ForegroundColor = White;
                lecture = ReadLine().Trim();
                ResetColor();
            }
            return lecture;
        }
    }
}
