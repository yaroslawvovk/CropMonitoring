using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Helpers
{
    static class InitialProvinces
    {
        public static Dictionary<int, string> GetProvinces()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "Черкаси");
            dic.Add(2, "Чернігів");
            dic.Add(3, "Чернівці");
            dic.Add(4, "Крим");
            dic.Add(5, "Дніпропетровськ");
            dic.Add(6, "Донецк");
            dic.Add(7, "Івано-Франківськ");
            dic.Add(8, "Харків");
            dic.Add(9, "Херсон");
            dic.Add(10, "Хмельницький");
            dic.Add(11, "Київ");
            dic.Add(12, "Київ місто");
            dic.Add(13, "Кіровоград");
            dic.Add(14, "Луганськ");
            dic.Add(15, "Львів");
            dic.Add(16, "Миколаїв");
            dic.Add(17, "Одеса");
            dic.Add(18, "Полтава");
            dic.Add(19, "Рівне");
            dic.Add(20, "Севастополь");
            dic.Add(21, "Суми");
            dic.Add(22, "Тернопіль");
            dic.Add(23, "Прикарпаття");
            dic.Add(24, "Вінниця");
            dic.Add(25, "Волинь");
            dic.Add(26, "Запоріжжя");
            dic.Add(27, "Житомир");
            return dic;
        }
    }
}


