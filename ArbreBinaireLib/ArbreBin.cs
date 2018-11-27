using System;
using System.Collections.Generic;

namespace SDD
{
    public partial class ArbreBin<T>
        where T : IComparable<T>
    {
        public T Élément;
        public ArbreBin<T> Gauche;
        public ArbreBin<T> Droite;

        public ArbreBin(
            T élément, 
            ArbreBin<T> gauche = null, 
            ArbreBin<T> droite = null)
        {
            Élément = élément;
            Gauche = gauche;
            Droite = droite;
        }

        public override string ToString()
        {
            if (Droite == null && Gauche == null)
                return "" + Élément;
            else
                return (Gauche?.ToString() ?? "n") 
                + " " + (Droite?.ToString() ?? "n") 
                + " " + Élément + " |";
        }

        public static string
            Format(ArbreBin<T> ab)
        {
            return ab?.ToString() ?? "n";
        }

        public static IEnumerable<T> Parcourir(
            ArbreBin<T> ab, Action<ArbreBin<T>, Action<ArbreBin<T>>> parcours)
        {
            var liste = new List<T>();
            parcours(ab, abin => { liste.Add(abin.Élément); });
            return liste;
        }

    }

    public static class ArbreBinExt
    {
        public static string EnTexte<T>(this ArbreBin<T> ab) where T: IComparable<T>
        {
            return ArbreBin<T>.Format(ab);
        }
    }

}
