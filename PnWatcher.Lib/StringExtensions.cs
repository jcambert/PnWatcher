using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnWatcher.Lib
{
    public enum Sens
    {
        TopBottom,
        BottomTop,
        

    }
    public static class StringExtensions
    {

        public static bool FileInUse(this string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var b= fs.CanWrite;
                }
                return false;
            }
            catch (IOException )
            {
                return true;
            }
        }

        public  static int CountLine(this string lines)
        {
            return lines.Split('\n').Count();
        }

        public static string RemoveLines(this string lines,int max, Sens sens)
        {
            if (lines.CountLine() < max) return lines;
            var _lines = lines.Split('\n');
            if (sens == Sens.TopBottom)
                return string.Join("\n", _lines.Skip(max - _lines.Count()));
            else 
                return string.Join("\n", _lines.Take(max-_lines.Count()));
        }
    }
}
