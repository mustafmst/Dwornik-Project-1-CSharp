/**
 * Program napisany przez Pawła Mstowskiego
 * 
 * Projekt na zajecia z Analizy i przetwarzania obrazów
 * 
 * ========================================================================
 *                             Wymagania Projektu:
 * ========================================================================
 * 
 * ////////////////////////////////////////////////////////////////////////
 * 
 * - Punktowe i geometryczne (monochrom + RGB):
 * 	rotacja obrazu z wykorzystaniem interpolacji biliniowej
 * 
 * - Filtracja przestrzenna (monochrom + RGB):
 * 	filtracja 'laplacian' i 'unsharp' dla zadanego alpha (fspecial). 
 * 	Brzeg odbicie symetr.
 * 
 * - Morfologiczne (monochrom + logiczne):
 * 	zamkniêcie elementem linijnym o zadanym nachyleniu i d³ugoœci
 * 
 * - logiczne (tylko obrazy logiczne):
 * 	mapa odleg³oœci dla zadanej metryki (bwdist)
 * 
 * ////////////////////////////////////////////////////////////////////////
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dwornik_Project_1
{
    class Obraz
    {
        
        private Bitmap mainImg;
        private Bitmap lastTrans;
        private string nazwaPliku;
        private int licznikOperacji;
        private string ostatniaOperacja;

        public Obraz()
        {
            licznikOperacji = 0;
        }


        /**
         * Metoda wczytuje obraz oraz inicjuje podstawowe informacje.
         * Przy niepowodzeniu zwraca wartosc 1 a w innych wypadkach 0.
         */
        public int wczytajObraz(string file)
        {
            try
            {
                mainImg = new Bitmap(file);
                nazwaPliku = file;
                lastTrans = mainImg;
                ostatniaOperacja = "none";
            }
            catch (Exception)
            {
                Console.WriteLine("Problem z wczytaniem pliku!!!");
                Console.WriteLine("\n==>>Porada:");
                Console.WriteLine("==>>Prosze podawac pelna sciezke do pliku!!!");
                return 1;
            }
            return 0;
        }

        /**
         * Metoda zapisuje obraz do pliku, ktorego nazwa jest generowana 
         * na podstawie dotychczasowych operacji.
         * Przy niepowodzeniu zwraca wartosc 1 a w innych wypadkach 0.
         */
        public int zapiszObraz()
        {
            try
            {
                lastTrans.Save(nazwaPliku + "_" + ostatniaOperacja + "_no_" + licznikOperacji+".bmp");
            }
            catch (Exception)
            {
                Console.WriteLine("Problem z zapisem pliku!!!");
                return 1;
            }

            return 0;
        }

        /**
         * Przekształcenie z obrazu RGB na odczienie szarości.
         * Zwracany jest obraz wynikowy jako obiekt typu System.Drwaing.Bitmap
         */
        public Bitmap monochrom()
        {
            Bitmap monochrome = mainImg;

            Color tmpPixel;
            Console.WriteLine("Przetwarzanie obrazu z koloru na skale szarosci");
            for (int kz = 0; kz < monochrome.Height; kz++)
            {
                for (int kx = 0; kx < monochrome.Width; kx++)
                {
                    tmpPixel = monochrome.GetPixel(kx, kz);
                    monochrome.SetPixel(kx, kz, Color.FromArgb( (byte)((0.2125 * tmpPixel.R) + (0.7154 * tmpPixel.G) + (0.0721 * tmpPixel.B)),
                                                                (byte)((0.2125 * tmpPixel.R) + (0.7154 * tmpPixel.G) + (0.0721 * tmpPixel.B)),
                                                                (byte)((0.2125 * tmpPixel.R) + (0.7154 * tmpPixel.G) + (0.0721 * tmpPixel.B))    ));
                }
            }

            lastTrans = monochrome;
            ostatniaOperacja = "Monochrom";
            return monochrome;
        }
/*
        public Bitmap rotation(double angle)
        {

        }*/
    }
}
