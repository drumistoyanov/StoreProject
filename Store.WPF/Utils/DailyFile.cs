using Store.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;

namespace Store.WPF.Utils
{
    public static class DailyFile
    {
        private static readonly StoreDBContext dBContext = new StoreDBContext();
        public static void SaveDailyFile()
        {
            var directoryForFileSaving = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()) + "\\Files\\DailyFiles";
            if (!File.Exists(directoryForFileSaving + "\\" + $"{DateTime.Now.ToShortDateString().Replace('/', '_')}.txt"))
            {
                var lines = dBContext.Sales.ToList();
                lines = lines.Where(x => x.DateCreated.ToShortDateString() == DateTime.Now.ToShortDateString() && x.IsDeleted == false).ToList();
                using StreamWriter file = new StreamWriter(directoryForFileSaving + "\\" + $"{DateTime.Now.ToShortDateString().Replace('/', '_')}.txt");
                file.Write(DateTime.Now);
                file.Write("Оборот:" + $"{lines.Sum(x => x.TotalPrice)}");
                MessageBox.Show("Дневният файл е генериран");
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Вече има дневен файл за днешната дата!.");
            }
        }
    }
}
