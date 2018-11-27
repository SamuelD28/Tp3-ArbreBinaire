using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;
using System.Linq;
using static SDD.ArbreBin<int>;

namespace SDD
{
    [TestClass]
    public class TesterArbreBin
    {
        static readonly bool[] AllTrue 
            = Enumerable.Repeat(true, ArbreBinKit.iArbres.Length).ToArray();
        
        static readonly IEnumerable<ArbreBin<int>> iArbres
            = ArbreBinKit.iArbres.Select(a => ArbreBinKit.NewArbreBinInt(a));

        static void AreEqualSeq<T>(IEnumerable<T> attendus, IEnumerable<T> recus)
        {
            AreEqual(attendus.Count(), recus.Count());
            for(int i = 0; i < attendus.Count(); i++)
            {
                var attendu = attendus.ElementAt(i);
                var recu = recus.ElementAt(i);
                AreEqual(attendu, recu);
            }
        }

        static void Tester<T>(
            string fnName, 
            Func<ArbreBin<int>, T> fn, 
            T[] attendus,
            ArbreBin<int> [] _arbres = null)
        {
            var arbres = _arbres ?? iArbres.ToArray();
            for (int i = 0; i < attendus.Length; i++)
            {
                AreEqual(attendus[i], fn(arbres[i]),
                    $"\n\n{fnName}( {ArbreBinKit.iArbres[i]} ) = {attendus[i]}, pas {fn(arbres[i])}");
            }
        }

        [TestMethod]
        public void _00_EstVide()
        {
            Tester(nameof(EstVide), EstVide, 
                new[]{ true, false, false, false, false, false, false, false, false });
        }

        [TestMethod]
        public void _01_EstFeuille()
        {
            Tester(nameof(EstFeuille), EstFeuille, 
                new[] { false, true, false, false, false, false, false, false, false });
        }

        [TestMethod]
        public void _02_EstComplet()
        {
            Tester(nameof(EstComplet), EstComplet,
                new[] { false, false, false, false, true, true, true, true, false, false, true });
        }

        [TestMethod]
        public void _03_EstIncomplet()
        {
            Tester(nameof(EstIncomplet), EstIncomplet,
                new[] { false, true, true, true, false, false, false, false, true, true, false });
        }

        [TestMethod]
        public void _10_Hauteur()
        {
            Tester(nameof(Hauteur), Hauteur,
                new[] { 0, 1, 2, 2, 3, 3, 3, 4, 5, 5, 6 });
        }

        [TestMethod]
        public void _11_Taille()
        {
            Tester(nameof(Taille), Taille,
                new[] { 0, 1, 2, 2, 5, 5, 5, 11, 5, 5, 11 });
        }

        [TestMethod]
        public void _12_HauteurOptimale()
        {
            Tester(nameof(HauteurOptimale), HauteurOptimale,
                new[] { 0, 1, 2, 2, 3, 3, 3, 4, 3, 3, 4 });
        }

        [TestMethod]
        public void _14_Min()
        {
            ThrowsException<ArgumentNullException>(() =>Min(null));
            Tester(nameof(Min), Min,
                new[] { 8, 3, 4, -4, 1, 5, 2, 1, 7, 1 }, iArbres.Skip(1).ToArray());
        }

        [TestMethod]
        public void _15_Max()
        {
            ThrowsException<ArgumentNullException>(() => Max(null));
            Tester(nameof(Max), Max,
                new[] { 8, 4, 5, 18, 6, 15, 18, 5, 11, 11 }, iArbres.Skip(1).ToArray());
        }

        [TestMethod]
        public void _20_ParcourirEnOrdre()
        {
            Tester(
                nameof(ParcourirEnOrdre), 
                ab => Parcourir(ab, ParcourirEnOrdre).EstStrictementOrdonné(),
                AllTrue);
        }

        [TestMethod]
        public void _24a_ParcourirEnOrdreItératif()
        {
            Tester(
                nameof(ParcourirEnOrdreItératif),
                ab => Parcourir(ab, ParcourirEnOrdreItératif).EstStrictementOrdonné(),
                AllTrue);
        }

        [TestMethod]
        public void _21_ParcourirEnOrdreInverse()
        {
            Tester(
                nameof(ParcourirEnOrdreInverse),
                ab => Parcourir(ab, ParcourirEnOrdreInverse).Reverse().EstStrictementOrdonné(),
                AllTrue);
        }

        private void TesterParcourirPostOrdre(
            Action<ArbreBin<int>, Action<ArbreBin<int>>> parcourir)
        {
            Tester(
                nameof(parcourir),
                ab => Parcourir(ab, parcourir).EnTexte(),
                ArbreBinKit.iArbres.Select(
                    ab => string.Join(" ", ab.Split(' ').Where(s => s != "n" && s != "|"))
                ).ToArray());
        }

        [TestMethod]
        public void _22_ParcourirPostOrdre()
        {
            TesterParcourirPostOrdre(ParcourirPostOrdre);
        }

        [TestMethod]
        public void _24b_ParcourirPostOrdreItératif()
        {
            TesterParcourirPostOrdre(ParcourirPostOrdreItératif);
        }

        private void TesterParcourirPréOrdre(
            Action<ArbreBin<int>, Action<ArbreBin<int>>> parcourir)
        {
            AreEqualSeq(new[] { ""
                , "8"
                , "4 3"
                , "4 5"
                , "3 0 -4 2 18"
                , "3 1 5 4 6"
                , "10 5 8 15 12"
                , "10 5 2 3 8 7 15 12 13 18 17"
                , "5 4 3 2 1"
                , "7 8 9 10 11"
                , "6 5 4 3 2 1 7 8 9 10 11"
                , "4 2 1 3"
            }, iArbres.Select(ab => Parcourir(ab, parcourir).EnTexte()));
        }

        [TestMethod]
        public void _23_ParcourirPréOrdre()
        {
            TesterParcourirPréOrdre(ParcourirPréOrdre);
        }

        [TestMethod]
        public void _24c_ParcourirPréOrdreItératif()
        {
            TesterParcourirPréOrdre(ParcourirPréOrdreItératif);
        }

        [TestMethod]
        public void _24d_ParcourirEnLargeur()
        {
            AreEqualSeq(new[] { ""
                , "8"
                , "4 3"
                , "4 5"
                , "3 0 18 -4 2"
                , "3 1 5 4 6"
                , "10 5 15 8 12"
                , "10 5 15 2 8 12 18 3 7 13 17"
                , "5 4 3 2 1"
                , "7 8 9 10 11"
                , "6 5 7 4 8 3 9 2 10 1 11"
                , "4 2 1 3"
            }, iArbres.Select(ab => Parcourir(ab, ParcourirEnLargeur).EnTexte()));
        }

        [TestMethod]
        public void _31_Énumérer()
        {
            Tester(nameof(Énumérer), ab => Énumérer(ab).EstStrictementOrdonné(), AllTrue);
        }

        [TestMethod]
        public void _32_ÉnumérerNoeuds()
        {
            Tester(
                nameof(ÉnumérerNoeuds), 
                ab => ÉnumérerNoeuds(ab).Select(noeud => noeud.Élément).EstStrictementOrdonné(), 
                AllTrue);
        }

        private void TesterCloner(Func<ArbreBin<int>, ArbreBin<int>> cloner)
        {
            foreach (var iArbre in ArbreBinKit.iArbres)
            {
                var ab = ArbreBinKit.NewArbreBinInt(iArbre);
                var clone = cloner(ab);
                AreEqual(ab.EnTexte(), clone.EnTexte(),
                    "La copie n'est pas exacte");
                IsTrue(ÉnumérerNoeuds(ab).Zip(ÉnumérerNoeuds(clone), (a, b) => a != b).All(x => x)
                    , "La copie n'est pas profonde");
            }
        }

        [TestMethod]
        public void _30_Cloner()
        {
            TesterCloner(Cloner);
        }

        [TestMethod]
        public void _47x_ClonerEnParcourant()
        {
            TesterCloner(ClonerEnParcourant);
        }

        [TestMethod]
        public void _33_NbFeuilles()
        {
            Tester(nameof(NbFeuilles), NbFeuilles,
                new[] { 0, 1, 1, 1, 3, 3, 2, 4, 1, 1, 2 });
        }

        [TestMethod]
        public void _34_NbTiges()
        {
            Tester(nameof(NbTiges), NbTiges,
                new[] { 0, 0, 1, 1, 0, 0, 2, 4, 4, 4, 8 });
        }

        [TestMethod]
        public void _35_NbEmbranchement()
        {
            Tester(nameof(NbEmbranchements), NbEmbranchements,
                new[] { 0, 0, 0, 0, 2, 2, 1, 3, 0, 0, 1 });
        }

        [TestMethod]
        public void _36_ÉquationFTEN()
        {
            Tester(nameof(ÉquationFTEN), ÉquationFTEN,
                new[] {
                     "0f + 0t + 0e = 0n"
                    , "1f + 0t + 0e = 1n"
                    , "1f + 1t + 0e = 2n"
                    , "1f + 1t + 0e = 2n"
                    , "3f + 0t + 2e = 5n"
                    , "3f + 0t + 2e = 5n"
                    , "2f + 2t + 1e = 5n"
                    , "4f + 4t + 3e = 11n"
                    , "1f + 4t + 0e = 5n"
                    , "1f + 4t + 0e = 5n"
                    , "2f + 8t + 1e = 11n"
                });
        }

        [TestMethod]
        public void _37_HauteurMoyenne()
        {
            Tester(nameof(HauteurMoyenne), ab => Math.Round(HauteurMoyenne(ab),1),
                new[] { 0.0, 1.0, 1.5, 1.5, 2.2, 2.2, 2.2, 3.0, 3.0, 3.0, 3.7 });
        }

        [TestMethod]
        public void _38x_EstBalancé()
        {
            Tester(nameof(EstBalancé), EstBalancé,
                new[] { true, true, true, true, true, true, true, true, false, false, false, false });
        }

        [TestMethod]
        public void _39x_Largeur()
        {
            Tester(nameof(Largeur), ab => Largeur(ab).ToString(),
                new[] { "[0, 0]", "[1, 1]", "[1, 1]", "[1, 1]", "[2, 2]", "[2, 2]", "[2, 2]", "[3, 4]", "[1, 1]", "[1, 1]", "[2, 2]" });
        }

        [TestMethod]
        public void _40_EstValide()
        {
            // Arbres valides
            Tester(nameof(EstValide), EstValide, AllTrue);
            
            // Arbres invalides
            foreach (var iArbreInvalide in ArbreBinKit.iArbresInvalides)
            {
                AreEqual(false, EstValide(ArbreBinKit.NewArbreBinInt(iArbreInvalide)), iArbreInvalide);
            }
        }

        [TestMethod]
        public void _41_Contient()
        {
            Tester(nameof(Contient), ab => Contient(ab, 2),
                new[] { false, false, false, false, true, false, false, true, true, false, true });
            Tester(nameof(Contient), ab => Contient(ab, 8),
                new[] { false, true, false, false, false, false, true, true, false, true, true });
        }

        [TestMethod]
        public void _42_Chemin()
        {
            Tester(nameof(Chemin), ab => Chemin(ab, 2),
                new[] { null, null, null, null, "GD*", null, null, "GG*", "GGG*", null, "GGGG*" });
            Tester(nameof(Chemin), ab => Chemin(ab, 8),
                new[] { null, "*", null, null, null, null, "GD*", "GD*", null, "D*", "DD*" });
        }

        [TestMethod]
        public void _45a_AjouterUn()
        {
            ArbreBin<int> ab = null;
            AreEqual(true, Ajouter(ref ab, 5));
            AreEqual("5", ab.EnTexte());

            AreEqual(false, Ajouter(ref ab, 5));
            AreEqual("5", ab.EnTexte());

            Ajouter(ref ab, 2);
            AreEqual("2 n 5 |", ab.EnTexte());

            Ajouter(ref ab, 9);
            AreEqual("2 9 5 |", ab.EnTexte());

            Ajouter(ref ab, 7);
            AreEqual("2 7 n 9 | 5 |", ab.EnTexte());

            AreEqual(true, Ajouter(ref ab, 4));
            AreEqual(false, Ajouter(ref ab, 4));
            AreEqual(false, Ajouter(ref ab, 5));
            AreEqual(false, Ajouter(ref ab, 9));
            AreEqual("n 4 2 | 7 n 9 | 5 |", ab.EnTexte());
        }

        [TestMethod]
        public void _45b_Ajouter2()
        {
            var arbres = iArbres.Skip(1).ToArray();
            var ajoutsRéussis 
                = new[] { true, true, true, false, true, true, false, false, true, false };
            Tester(nameof(Ajouter)+"2", ab => Ajouter(ref ab, 2), ajoutsRéussis, arbres);
            var arbresAprès = new[] {
                "2 n 8 |"
                , "2 n 3 | n 4 |"
                , "2 5 4 |"
                , "-4 2 0 | 18 3 |"
                , "n 2 1 | 4 6 5 | 3 |"
                , "2 8 5 | 12 n 15 | 10 |"
                , "n 3 2 | 7 n 8 | 5 | n 13 12 | 17 n 18 | 15 | 10 |"
                , "1 n 2 | n 3 | n 4 | n 5 |"
                , "2 n n n 11 10 | 9 | 8 | 7 |"
                , "1 n 2 | n 3 | n 4 | n 5 | n n n n 11 10 | 9 | 8 | 7 | 6 |"
                , "1 3 2 | n 4 |"
            };
            AreEqualSeq(arbresAprès, arbres.Select(ab => ab.EnTexte()));
        }

        [TestMethod]
        public void _45c_Ajouter9()
        {
            var arbres = iArbres.Skip(1).ToArray();
            var ajoutsRéussis
                = new[] { true, true, true, true, true, true, true, true, false, false, true };
            Tester(nameof(Ajouter)+"9", ab => Ajouter(ref ab, 9), ajoutsRéussis, arbres);
            var arbresAprès = new[] {
                "n 9 8 |"
                , "3 9 4 |"
                , "n n 9 5 | 4 |"
                , "-4 2 0 | 9 n 18 | 3 |"
                , "1 4 n 9 6 | 5 | 3 |"
                , "n n 9 8 | 5 | 12 n 15 | 10 |"
                , "n 3 2 | 7 9 8 | 5 | n 13 12 | 17 n 18 | 15 | 10 |"
                , "1 n 2 | n 3 | n 4 | 9 5 |"
                , "n n n n 11 10 | 9 | 8 | 7 |"
                , "1 n 2 | n 3 | n 4 | n 5 | n n n n 11 10 | 9 | 8 | 7 | 6 |"
                , "1 3 2 | 9 4 |"
            };
            AreEqualSeq(arbresAprès, arbres.Select(ab => ab.EnTexte()));
        }

        [TestMethod]
        public void _46a_AjouterPlusieurs()
        {
            ArbreBin<int> ab = null;
            AreEqual(5, Ajouter(ref ab, new [] {5, 5, 2, 2, 9, 9, 7, 7, 4, 4, 5}));
            AreEqual("n 4 2 | 7 n 9 | 5 |", ab.EnTexte());

            ab = null;
            AreEqual(5, Ajouter(ref ab, new[] { 1, 2, 3, 4, 5 }));
            AreEqual("n n n n 5 4 | 3 | 2 | 1 |", ab.EnTexte());

            ab = null;
            AreEqual(5, Ajouter(ref ab, new[] { 5, 4, 3, 2, 1 }));
            AreEqual("1 n 2 | n 3 | n 4 | n 5 |", ab.EnTexte());

        }

        [TestMethod]
        public void _46b_AjouterPlusieursRandom()
        {
            ArbreBin<int> ab = null;
            AreEqual(10, Ajouter(ref ab, Enumerable.Range(1, 10), new Random(1)));
            AreEqual("1 3 n 4 | 2 | 6 5 | 8 10 9 | 7 |", ab.EnTexte());
        }

        [TestMethod]
        public void _50_Vider()
        {
            ArbreBin<int> ab = null;
            AreEqual(false, Vider(ref ab));
            IsNull(ab);
            foreach(var arbre in iArbres.Skip(1))
            {
                var a = arbre;
                AreEqual(true, Vider(ref a), arbre.EnTexte());
                IsNull(a, a.EnTexte());
            }
        }

        [TestMethod]
        public void _51a_Enlever_Vide()
        {
            ArbreBin<int> ab = null;
            AreEqual(false, Enlever(ref ab, 1));
            IsNull(ab);
        }

        [TestMethod]
        public void _51b_Enlever_Unique()
        {
            var ab = ArbreBinKit.NewArbreBinInt("5");
            AreEqual(false, Enlever(ref ab, 1));
            IsNotNull(ab);
            AreEqual(true, Enlever(ref ab, 5));
            IsNull(ab);
        }

        [TestMethod]
        public void _51c_Enlever_Feuille()
        {
            var ab = ArbreBinKit.NewArbreBinInt("1 3 2 | 5 7 6 | 4 |");
            AreEqual(false, Enlever(ref ab, 0));
            AreEqual(true, Enlever(ref ab, 1));
            AreEqual("n 3 2 | 5 7 6 | 4 |", ab.EnTexte());
            AreEqual(false, Enlever(ref ab, 1));
            AreEqual(true, Enlever(ref ab, 7));
            AreEqual("n 3 2 | 5 n 6 | 4 |", ab.EnTexte());
            AreEqual(true, Enlever(ref ab, 5));
            AreEqual("n 3 2 | 6 4 |", ab.EnTexte());
            AreEqual(true, Enlever(ref ab, 6));
            AreEqual("n 3 2 | n 4 |", ab.EnTexte());
            AreEqual(true, Enlever(ref ab, 3));
            AreEqual("2 n 4 |", ab.EnTexte());
            AreEqual(true, Enlever(ref ab, 2));
            AreEqual("4", ab.EnTexte());

        }

        [TestMethod]
        public void _51d_Enlever_BrancheSimple()
        {
            var ab = ArbreBinKit.NewArbreBinInt("1 n 2 | n 3 | n n 7 6 | 5 | 4 |");
            AreEqual(true, Enlever(ref ab, 3));
            AreEqual("1 n 2 | n n 7 6 | 5 | 4 |", ab.EnTexte());
            AreEqual(true, Enlever(ref ab, 5));
            AreEqual("1 n 2 | n 7 6 | 4 |", ab.EnTexte());

        }

        [TestMethod]
        public void _51e_Enlever_BiBrancheSimple()
        {
            // Nombre pair on fait monter à gauche
            var ab = ArbreBinKit.NewArbreBinInt("1 5 2 |");
            AreEqual(true, Enlever(ref ab, 2));
            AreEqual("n 5 1 |", ab.EnTexte());
            
            // Nombre impair on fait monter à droite
            ab = ArbreBinKit.NewArbreBinInt("1 5 3 |");
            AreEqual(true, Enlever(ref ab, 3));
            AreEqual("1 n 5 |", ab.EnTexte());
            
            // Nombre pair on fait monter à gauche
            ab = ArbreBinKit.NewArbreBinInt("1 n 2 | n 3 | n n 8 7 | 6 | 4 |");
            AreEqual(true, Enlever(ref ab, 4));
            AreEqual("1 n 2 | n n 8 7 | 6 | 3 |", ab.EnTexte());
            
            // Nombre impair on fait monter à droite
            ab = ArbreBinKit.NewArbreBinInt("1 n 2 | n 3 | n n 8 7 | 6 | 5 |");
            AreEqual(true, Enlever(ref ab, 5));
            AreEqual("1 n 2 | n 3 | n 8 7 | 6 |", ab.EnTexte());

        }

        [TestMethod]
        public void _51f_Enlever_BiBrancheComplexe()
        {
            var ab = ArbreBinKit.NewArbreBinInt("1 3 2 | 6 8 7 | 4 |");
            AreEqual(true, Enlever(ref ab, 4));
            AreEqual("1 n 2 | 6 8 7 | 3 |", ab.EnTexte());
            ab = ArbreBinKit.NewArbreBinInt("0 2 n 3 | 1 | 6 8 7 | 4 |");
            AreEqual(true, Enlever(ref ab, 4));
            AreEqual("0 2 1 | 6 8 7 | 3 |", ab.EnTexte());
            ab = ArbreBinKit.NewArbreBinInt("1 3 2 | 6 8 7 | 5 |");
            AreEqual(true, Enlever(ref ab, 5));
            AreEqual("1 3 2 | n 8 7 | 6 |", ab.EnTexte());
            ab = ArbreBinKit.NewArbreBinInt("1 3 2 | n 7 6 | 9 8 | 5 |");
            AreEqual(true, Enlever(ref ab, 5));
            AreEqual("1 3 2 | 7 9 8 | 6 |", ab.EnTexte());
        }

        [TestMethod]
        public void _52a_Enlever_Plusieurs_Feuilles()
        {
            var ab = ArbreBinKit.NewArbreBinInt("1 3 2 | 6 8 7 | 4 |");
            AreEqual(5, Enlever(ref ab, new[] { 1, 3, 2, 6, 8 }));
            AreEqual(0, Enlever(ref ab, new[] { 1, 3, 2, 6, 8 }));
            AreEqual("n 7 4 |", ab.EnTexte());
        }

        [TestMethod]
        public void _52b_Enlever_Plusieurs_Complexe()
        {
            var ab = ArbreBinKit.NewArbreBinInt("1 3 2 | 6 8 7 | 4 |");
            AreEqual(2, Enlever(ref ab, new[] { 4, 3 }));
            AreEqual(0, Enlever(ref ab, new[] { 4, 3 }));
            AreEqual("1 n 2 | n 8 7 | 6 |", ab.EnTexte());
        }

        [TestMethod]
        public void _60x_Balancer()
        {
            ArbreBin<int> ab = null;
            IsTrue(EstBalancé(ab), "Car un arbre vide est considéré balancé");
            IsFalse(Balancer(ref ab), "Car on a pas besoin de le balancer");
            
            Ajouter(ref ab, Enumerable.Range(1, 15));
            IsFalse(EstBalancé(ab), "Car il est linéaire");
            IsTrue(Balancer(ref ab), "Car on peut le balancer");
            IsTrue(EstBalancé(ab), "Car il est maintenant balancé");
            IsFalse(Balancer(ref ab), "Car on a pas besoin de le rebalancé");
            AreEqual("8f + 0t + 7e = 15n", ÉquationFTEN(ab));
            
            Vider(ref ab);
            Ajouter(ref ab, Enumerable.Range(1, 16));
            IsTrue(Balancer(ref ab), "Car on peut le balancer");
            AreEqual("8f + 1t + 7e = 16n", ÉquationFTEN(ab));
        }

        [TestMethod]
        public void _61x_NouvelArbreBalancé()
        {
            ArbreBin<int> ab = NouvelArbreBalancé(new int[0]);
            AreEqual("n", ab.EnTexte());

            ab = NouvelArbreBalancé(Enumerable.Range(1, 15).ToArray());
            AreEqual("8f + 0t + 7e = 15n", ÉquationFTEN(ab));

            ab = NouvelArbreBalancé(Enumerable.Range(1, 16).ToArray());
            AreEqual("8f + 1t + 7e = 16n", ÉquationFTEN(ab));

            ThrowsException<ArgumentException>(() => NouvelArbreBalancé(new[] { 2, 2 }));
            ThrowsException<ArgumentException>(() => NouvelArbreBalancé(new[] { 2, 1 }));
        }

    }
}
