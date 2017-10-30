using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Services
{
    public class KilledKittens
    {
        static int killedKitten = 0;

        public static void KillCat()
        {
            killedKitten++;
        }

        public static int ReturnKilledkitten()
        {
            return killedKitten;
        }

        public static void ResetCounter()
        {
            killedKitten = 0;
        }
    }
}
