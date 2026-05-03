using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Student_Data
{

    public class CommonColors()
    {

        public static Color[] BackColors = new Color[]
{
            (Color)ColorConverter.ConvertFromString("#e9e0fd"  ), // Violet
            (Color)ColorConverter.ConvertFromString("#ffd6e7") , // Red 
            (Color)ColorConverter.ConvertFromString("#ccffd0") , // Green
            (Color)ColorConverter.ConvertFromString("#c0f5ef") , // Blue Green
            (Color)ColorConverter.ConvertFromString("#c2ecfc") , // Blue
            (Color)ColorConverter.ConvertFromString("#ffe1d6") , // Orange
            (Color)ColorConverter.ConvertFromString("#ffd6da") , // Maroon

};


        public static Color[] ForeColors = new Color[]
        {
            (Color)ColorConverter.ConvertFromString("#6950d4"  ), // Violet
            (Color)ColorConverter.ConvertFromString("#e2026b") , // Red
      (Color)ColorConverter.ConvertFromString("#058c50") , // Green
         (Color)ColorConverter.ConvertFromString("#046d68") , // Blue Green
    (Color)ColorConverter.ConvertFromString("#0282b8") , // Blue
        (Color)ColorConverter.ConvertFromString("#cd4b26") , // Orange
          (Color)ColorConverter.ConvertFromString("#96062f") , // Maroon

        };

        public static (Color, Color) GetRandomColor()
        {
            Random RD = new Random();

            int Index = RD.Next(6);

            return (ForeColors[Index], BackColors[Index]);

        }
    }

}
