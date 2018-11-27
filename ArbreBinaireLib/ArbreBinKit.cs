using System;
using System.Collections.Generic;

namespace SDD
{
    public static class ArbreBinKit
    {
        public static ArbreBin<int> 
            NewArbreBinInt(string instructions, params ArbreBin<int>[] arbres)
        {
            return NewArbreBin(instructions, s => int.Parse(s), arbres);
        }

        public static ArbreBin<string> 
            NewArbreBinString(string instructions, params ArbreBin<string> [] arbres)
        {
            return NewArbreBin(instructions, s => s, arbres);
        }

        public static ArbreBin<T>
            NewArbreBin<T>(string instructions, Func<string, T> parse, params ArbreBin<T> [] arbres)
            where T : IComparable<T>
        {
            var nodes = new Stack<ArbreBin<T>>(arbres);
            foreach (var instruction in instructions.Split(' '))
            {
                if (instruction == "|")
                {
                    var parent = nodes.Pop();
                    parent.Droite = nodes.Pop();
                    parent.Gauche = nodes.Pop();
                    nodes.Push(parent);
                }
                else if (instruction.ToLower() == "n")
                {
                    nodes.Push(null);
                }
                else
                {
                    try
                    {
                        nodes.Push(new ArbreBin<T>(parse(instruction)));
                    }
                    catch
                    {
                        throw new Exception($"Instruction non reconnue: {instruction}");

                    }
                }
            }
            if (nodes.Count != 1)
                throw new Exception($"Le pile devrait contenir un seul élément: {nodes.EnTexte()}");
            return nodes.Pop();
        }

        public static readonly string[] iArbres = new string[]
        {
            "n"
            , "8"
            , "3 n 4 |"
            , "n 5 4 |"
            , "-4 2 0 | 18 3 |"
            , "1 4 6 5 | 3 |"
            , "n 8 5 | 12 n 15 | 10 |"
            , "n 3 2 | 7 n 8 | 5 | n 13 12 | 17 n 18 | 15 | 10 |"
            , "1 n 2 | n 3 | n 4 | n 5 |"
            , "n n n n 11 10 | 9 | 8 | 7 |"
            , "1 n 2 | n 3 | n 4 | n 5 | n n n n 11 10 | 9 | 8 | 7 | 6 |"
            , "1 3 2 | n 4 |"
        };

        public static readonly string[] iArbresInvalides = new string[]
        {
            "5 n 4 |"
            , "n 3 4 |"
            , "n 8 5 | 12 n 15 | 1 |"
            , "n 8 5 | 12 n 15 | 20 |"
            , "n 8 5 | 12 n 15 | 6 |"
            , "n 8 5 | 12 n 15 | 13 |"
            , "n 4 5 | 12 n 15 | 10 |"
            , "n 8 5 | 17 n 15 | 10 |"
            , "n 3 2 | 9 n 8 | 5 | n 13 12 | 17 n 18 | 15 | 10 |"
        };

        public static readonly string[] sArbres = new string[]
        {
            "n"
            , "n beta alpha | epsilon n gamma | delta |"
        };

    }

}