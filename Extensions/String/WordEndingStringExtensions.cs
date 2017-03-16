using System.Collections.Generic;

namespace Extensions.String
{
    public class WordEndingStringExtensions
    {
        /// <summary>
        /// Окончания глаголов
        /// </summary>
        static string[] VerbEndings = new string[]
        {
            //1 спряжение
            "у", "ю", //1 лицо
            "ешь", //2 лицо
            "ет", //3 лицо
            "ем", //1 лицо множ
            "ете", //2 лицо множ
            "ут", "ют", //3 лицо множ

            //2 спряжение
            //"у", "ю", //1 лицо совпадают
            "ишь", //2 лицо
            "ит", //3 лицо
            "им", //1 лицо множ
            "ите", //2 лицо множ
            "ат", "ят", //3 лицо множ
        };

        static string[] AdjectiveEndings = new string[]
        {
            
            //мужской род  //женский род  //средний род  //множественное число

            "ый", "ий",    "ая",  "яя",   "ое", "ее",     "ые", "ие", //именительный падеж

            "ого", "его",  "ой",  "ей",   /*ого, его"*/   "ых", "их", //родительный падеж

            "ому", "ему", /*"ой", "ей"*/  /*ому, ему*/    "ым", /*им*/  //дательный падеж

            /*ого его */   "ую",  "юю",   /*ое, ее*/     /*"ых", "их"*/ //родительный падеж

            /* ым им */    /*ой, ей*/    /*ого, его */   "ыми", "ими", //творительный падеж

            "ом", /*ем*/    /*ой,  ей*/   /*ом, ем*/    /*ых, их */ //падежный падеж

        };

        public static IEnumerable<string> AllWordEndings
        {
            get
            {
                List<string> result = new List<string>();

                result.AddRange(VerbEndings);
                result.AddRange(AdjectiveEndings);

                return result;
            }
        }
    }
}
