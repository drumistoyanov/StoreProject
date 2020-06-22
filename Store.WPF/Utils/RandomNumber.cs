using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.WPF.Utils
{
    public static class RandomNumber
    {
        private static string GenerateRandomNumber()
        {
            var rng = new Random();
            string date = DateTime.Now.ToString("ddMMyy");
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //0123456789
            int index = rng.Next(letters.Length);
            string randomNumberSequence = rng.Next(100, 999).ToString();
            string randomLetterAndNumberSequence = letters[index] + randomNumberSequence;
            string result = "BB" + date + randomLetterAndNumberSequence;
            return result;
        }
        public static string BillNumber(StoreDBContext dBContext)
        {
            var number = "";
            do
            {
                number = GenerateRandomNumber();
            } while (dBContext.Bills.Select(b => b.Number).Contains(number));
            return number;
        }
    }
}
