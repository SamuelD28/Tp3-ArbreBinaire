using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static SDD.AssertExt;
using static SDD.KitDeMots;

namespace SDD
{
    [TestClass]
    public class TesterDictionnaireArbreBin : TesterDictionnaire
    {
        protected override IDictionnaire NewDico()
        {
            return new DictionnaireHashSet();
        }
    }

    public abstract partial class TesterDictionnaire
    {

        [TestMethod]
        public void _80_InfoÉtat()
        {
            var dico = NewDico(MotsProches);
            var info = dico.InfoÉtat;
            IsTrue(
                info.StartsWith("Capacité")
                || info.StartsWith("Rien")
                || info.StartsWith("Hauteur")
            );
        }
    }

    }
