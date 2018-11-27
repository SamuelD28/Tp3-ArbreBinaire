using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.ConsoleColor;
using static SDD.ArbreBin<int>;

namespace SDD
{
    public static partial class MenuGénéral
    {
        const int Offset = 60;

        static readonly Dictionary<string, ArbreBin<int>> Arbres 
            = new Dictionary<string, ArbreBin<int>>();

        public static void ResetArbres()
        {
            Arbres.Clear();
            foreach(var iArbre in ArbreBinKit.iArbres)
            {
                Arbres[iArbre] = ArbreBinKit.NewArbreBinInt(iArbre);
            }
        }

        public static void Main()
        {
            SetWindowSize(Math.Min(LargestWindowWidth, 140), LargestWindowHeight-5);
            ResetArbres();
            Show();
        }

        static void Show()
        {
            // ConsoleMenu.ExitChar = 'Z';
            ConsoleMenu.Show
            (
                "Arbre Binaire", Yellow, Black, Red, MenuItems.ToArray()
            );
        }

        static readonly List<MenuItem> MenuItems = new List<MenuItem>
        {
            MenuItem.Rien

            , new MenuItem("Reset",
                () => {
                    ResetArbres();
                    WriteLine("Arbres remis à zéro!");
                })

            , MenuItem.Spacer
            , new MenuItem(" Arbre binaire quelconque")

            , new MenuItem(nameof(EstVide) + ", " + nameof(EstFeuille) + ", etc.",
                () => {
                    Section(nameof(EstVide));
                    Calculer(nameof(EstVide), EstVide);
                    
                    Section(nameof(EstFeuille));
                    Calculer(nameof(EstFeuille), EstFeuille);

                    Section(nameof(EstComplet));
                    Calculer(nameof(EstComplet), EstComplet);

                    Section(nameof(EstIncomplet));
                    Calculer(nameof(EstIncomplet), EstIncomplet);
                })

            , new MenuItem(nameof(Hauteur) + ", " + nameof(Taille) + ", etc.",
                () => {

                    Section(nameof(Taille));
                    Calculer(nameof(Taille), Taille);

                    Section(nameof(Hauteur));
                    Calculer(nameof(Hauteur), Hauteur);
                    
                    Section(nameof(HauteurOptimale));
                    Calculer(nameof(HauteurOptimale), HauteurOptimale);

                    Section(nameof(HauteurMoyenne));
                    Calculer(nameof(HauteurMoyenne), HauteurMoyenne, h => h.ToString("N1"));

                    Section(nameof(EstBalancé));
                    Calculer(nameof(EstBalancé), EstBalancé);

                    Section(nameof(Largeur));
                    Calculer(nameof(Largeur), Largeur);

                })

            , new MenuItem(nameof(ParcourirEnOrdre),
                () => {
                    Section("Récursif");
                    Calculer(
                        nameof(ParcourirEnOrdre), 
                        ab => Parcourir(ab, ParcourirEnOrdre),
                        en => en.EnTexte().PadRight(Offset));
                    
                    Section("Itératif");
                    Calculer(
                        nameof(ParcourirEnOrdreItératif),
                        ab => Parcourir(ab, ParcourirEnOrdreItératif),
                        en => en.EnTexte().PadRight(Offset));
                })

            , new MenuItem(nameof(ParcourirPréOrdre),
                () => {
                    Section("Récursif");
                    Calculer(
                        nameof(ParcourirPréOrdre),
                        ab => Parcourir(ab, ParcourirPréOrdre),
                        en => en.EnTexte().PadRight(Offset));
                    Section("Itératif");
                    Calculer(
                        nameof(ParcourirPréOrdreItératif),
                        ab => Parcourir(ab, ParcourirPréOrdreItératif),
                        en => en.EnTexte().PadRight(Offset));
                })

            , new MenuItem(nameof(ParcourirPostOrdre),
                () => {
                    Section("Récursif");
                    Calculer(
                        nameof(ParcourirPostOrdre),
                        ab => Parcourir(ab, ParcourirPostOrdre),
                        en => en.EnTexte().PadRight(Offset));
                    Section("Itératif");
                    Calculer(
                        nameof(ParcourirPostOrdreItératif),
                        ab => Parcourir(ab, ParcourirPostOrdreItératif),
                        en => en.EnTexte().PadRight(Offset));
                })

            , new MenuItem(nameof(ParcourirEnOrdreInverse),
                () => Calculer(nameof(ParcourirEnOrdreInverse), ab => Parcourir(ab, ParcourirEnOrdreInverse),
                en => en.EnTexte().PadRight(Offset)))

            , new MenuItem("Parcourir & Modifier",
                () => {
                    Section(nameof(Ajouter1));
                    Calculer(
                        nameof(Ajouter1),
                        ab => { Ajouter1(ab); return ab; },
                        en => en.EnTexte().PadRight(Offset));

                    Section(nameof(Doubler));
                    Calculer(
                        nameof(Doubler),
                        ab => { Doubler(ab); return ab; },
                        en => en.EnTexte().PadRight(Offset));

                })

            , new MenuItem(nameof(ParcourirEnLargeur),
                () => Calculer(nameof(ParcourirEnLargeur), ab => Parcourir(ab, ParcourirEnLargeur),
                en => en.EnTexte().PadRight(Offset)))

            , new MenuItem(nameof(Énumérer),
                () => {
                    Section(nameof(Énumérer));
                    Calculer(nameof(Énumérer), Énumérer, en => en.EnTexte().PadRight(Offset));

                    Section("Nombre de feuilles");
                    Calculer(nameof(NbFeuilles), NbFeuilles);

                    Section("Nombre de tiges");
                    Calculer(nameof(NbTiges), NbTiges);

                    Section("Nombre d'embranchements");
                    Calculer(nameof(NbEmbranchements), NbEmbranchements);

                    Section("Equation F + T + E = N");
                    Calculer(nameof(ÉquationFTEN), ÉquationFTEN);

                })

            , new MenuItem(nameof(Cloner),
                () =>  {
                    Section("Récursif");
                    Calculer(nameof(Cloner), Cloner,
                    ab => "\n" + ab.EnTexte().PadLeft(Offset));
                    
                    Section("En parcourant");
                    Calculer(nameof(ClonerEnParcourant), ClonerEnParcourant,
                    ab => "\n" + ab.EnTexte().PadLeft(Offset));
                })

            , MenuItem.Spacer
            , new MenuItem(" Arbre binaire de recherche")

            , new MenuItem(nameof(Min) + " & " + nameof(Max),
                () => {
                    Section(nameof(Min));
                    Calculer(nameof(Min), ab => EstVide(ab) ? "aucun" : ""+Min(ab));
                    Section(nameof(Max));
                    Calculer(nameof(Max), ab => EstVide(ab) ? "aucun" : ""+Max(ab));
                })

            , new MenuItem(nameof(EstValide),
                () => {
                    Section("Valide");
                    Calculer(nameof(EstValide), EstValide);
                    Section("Invalide");
                    Calculer(nameof(EstValide), EstValide, null, ArbreBinKit.iArbresInvalides);
                })

            , new MenuItem(nameof(Contient) + " & " + nameof(Chemin),
                () => {
                    if(int.TryParse(MenuIO.Lire("Nombre à chercher: "), out int n))
                    {
                        Section(nameof(Contient));
                        Calculer(nameof(Contient), ab => Contient(ab, n));
                        Section(nameof(Chemin));
                        Calculer(nameof(Chemin), ab => Chemin(ab, n), s => s ?? "null");
                    }
                    else
                    {
                        WriteLine("Nombre invalide");
                    }
                })

            , new MenuItem(nameof(Ajouter) + " un (au choix)",
                () => {
                    if(int.TryParse(MenuIO.Lire("Nombre à ajouter: "), out int n))
                    {
                        Calculer(nameof(Ajouter), ab => {
                            if(Ajouter(ref ab, n))
                                return ab;
                            else
                                return new ArbreBin<int>(0);
                        }, ab => ab.EnTexte().PadRight(Offset));
                    }
                    else
                    {
                        WriteLine("Nombre invalide");
                    }
                })

            , new MenuItem(nameof(Ajouter) + " plusieurs en ordre",
                () => {
                    if(int.TryParse(MenuIO.Lire("Quantité de nombres à ajouter: "), out int n) && n >= 0)
                    {
                        MenuIO.RapporterCalcul($"Ajout de {n} nombres en ordre", () => {
                            ArbreBin<int> ab = null;
                            Ajouter(ref ab, Enumerable.Range(1, n));
                            return ab;
                        }, 1, AfficherAvecStats);
                        WriteLine();
                        MenuIO.RapporterCalcul($"Ajout de {n} nombres en ordre inverse", () => {
                            ArbreBin<int> ab = null;
                            Ajouter(ref ab, Enumerable.Range(1, n).Reverse());
                            return ab;
                        }, 1, AfficherAvecStats);
                    }
                    else
                    {
                        WriteLine("Nombre invalide");
                    }
                })

            , new MenuItem(nameof(Ajouter) + " plusieurs choisis",
                () => {
                    try 
                    {
                        var nombres = MenuIO
                            .Lire("Nombres à ajouter (séparés par des espaces)")
                            .Split(' ')
                            .Select(n => int.Parse(n));

                        int compte = -1;

                        MenuIO.RapporterCalcul($"Ajout dans l'ordre", () => {
                            ArbreBin<int> ab = null;
                            compte = Ajouter(ref ab, nombres);
                            return ab;
                        }, 1, ab => AfficherAvecStats(ab) + $"\n    {compte}/{nombres.Count()} ajoutés");

                    }
                    catch
                    {
                        WriteLine("Nombres invalides");
                    }
                })

            , new MenuItem(nameof(Ajouter) + " plusieurs en désordre",
                () => {
                    if(int.TryParse(MenuIO.Lire("Quantité de nombres à ajouter: "), out int n) && n >= 0)
                    {
                        MenuIO.RapporterCalcul($"Ajout de {n} nombres en désordre", () => {
                            ArbreBin<int> ab = null;
                            Ajouter(ref ab, Enumerable.Range(1, n), new Random(1));
                            return ab;
                        }, 1, AfficherAvecStats);
                    }
                    else
                    {
                        WriteLine("Nombre invalide");
                    }
                })

            , new MenuItem(nameof(Vider),
                () => Calculer(nameof(Vider), ab => Vider(ref ab) && EstVide(ab)))

            , new MenuItem(nameof(Enlever) + " un (au choix)",
                () => {
                    if(int.TryParse(MenuIO.Lire("Nombre à enlever: "), out int n))
                    {
                        Calculer(nameof(Enlever), ab => {
                            if(Enlever(ref ab, n))
                                return ab;
                            else
                                return new ArbreBin<int>(0);
                        }, ab => ab.EnTexte().PadRight(Offset));
                    }
                    else
                    {
                        WriteLine("Nombre invalide");
                    }
                })

            , new MenuItem(nameof(Ajouter) + " et " + nameof(Enlever),
                () => {
                    try
                    {
                        var nombres = MenuIO
                            .Lire("Nombres à ajouter (séparés par des espaces)")
                            .Split(' ')
                            .Select(n => int.Parse(n));

                        WriteLine();
                        var nombre = int.Parse(MenuIO.Lire("Nombres à enlever"));

                        ArbreBin<int> ab = null;

                        WriteLine();
                        MenuIO.RapporterCalcul($"Ajout dans l'ordre {string.Join(" ", nombres)}", () => {
                            Ajouter(ref ab, nombres);
                            return ab;
                        }, 1, abin => AfficherAvecStats(abin));

                        WriteLine();
                        MenuIO.RapporterCalcul($"Suppression de {nombre}", () => {
                            Enlever(ref ab, nombre);
                            return ab;
                        }, 1, abin => AfficherAvecStats(abin));

                    }
                    catch
                    {
                        WriteLine("Nombres invalides");
                    }
                })

        };

        static string AfficherAvecStats(ArbreBin<int> ab)
            => $"\n    {ab.EnTexte()}"
             + $"\n    {nameof(Hauteur)}: {Hauteur(ab)}"
             + $"\n    {nameof(HauteurOptimale)}: {HauteurOptimale(ab)}"
             + $"\n    {nameof(HauteurMoyenne)}: {HauteurMoyenne(ab):N2}"
             + $"\n    {nameof(EstBalancé)}: {EstBalancé(ab)}"
             + $"\n    {nameof(ÉquationFTEN)}: {ÉquationFTEN(ab)}"
             ;

        static void Calculer<T>(
            string fnName, 
            Func<ArbreBin<int>, T> fn, 
            Func<T, string> format = null, 
            IEnumerable<string> arbres = null,
            bool update = false)
        {
            arbres = arbres ?? ArbreBinKit.iArbres;
            foreach (var iArbre in arbres)
            {
                if( !Arbres.TryGetValue(iArbre, out var ab) ) 
                    ab = ArbreBinKit.NewArbreBinInt(iArbre);
                MenuIO.RapporterCalcul(iArbre.PadLeft(Offset), () => fn(ab), 1, format);
                if (update && Arbres.ContainsKey(iArbre))
                    // NB l'update ne marche pas pour les arbres vides...
                    Arbres[iArbre] = ab;
            }
            if (!update) ResetArbres();
        }

        static void Section(string titre)
        {
            WriteLine();
            WriteLine(titre.PadLeft(Offset));
            WriteLine(new String('=', titre.Length).PadLeft(Offset));
            WriteLine();
        }

        static partial void Ajouter1(ArbreBin<int> ab);
        static partial void Doubler(ArbreBin<int> ab);

    }
}
