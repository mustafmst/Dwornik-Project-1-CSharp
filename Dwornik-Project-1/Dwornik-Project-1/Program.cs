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
using System.Text;
using System.Drawing;

namespace Dwornik_Project_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Obraz img = new Obraz();
            /*
             * Sprawdzenie podania argumentów
             */
            if (args.Length == 0)
            {
                Console.WriteLine("Nie podano plikow wejsciowych w argumencie!!!!");
                Console.WriteLine("\n==>>Porada:");
                Console.WriteLine("==>>Prosze podawac pelna sciezke do pliku!!!");
                
                Console.ReadKey();
                return;
            }
            else
            {
                foreach(string file in args) 
                {
                    /*
                     * Sprawdzenie poprawnosci wczytania pliku
                     */
                    if (img.wczytajObraz(file) == 0)
                    {
                        img.monochrom();
                        img.zapiszObraz();
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
