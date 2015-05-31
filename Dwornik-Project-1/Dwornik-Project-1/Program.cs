using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Dwornik_Project_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Obraz img = new Obraz();

            if (args.Length == 0)
            {
                Console.WriteLine("Nie podano plikow wejsciowych w argumencie!!!!");
                Console.ReadKey();
                return;
            }
            else
            {
                foreach(string file in args) 
                {
                    img.wczytajObraz(file);
                    img.monochrom();
                    img.zapiszObraz();
                }
            }
            Console.ReadKey();
        }
    }
}
