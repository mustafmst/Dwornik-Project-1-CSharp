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

namespace ImageTool
{
    /// <summary>
    /// Klasa do przechowywania i przetwarzania obrazu
    /// </summary>
    public class Obraz
    {
        #region zmienne

        private Bitmap mainImg;
        private Bitmap lastTrans;
        private string nazwaPliku;
        private int licznikOperacji;
        private string ostatniaOperacja;

        #endregion
        //======================================================================================
        #region wlasciwosci

        /// <summary>
        /// pobieranie glownego obrazu
        /// </summary>
        public Bitmap Main
        {
            public get
            {
                return mainImg;
            }
        }

        /// <summary>
        /// pobieranie obrazu po transformacji
        /// </summary>
        public Bitmap Trans
        {
            public get
            {
                return lastTrans;
            }
        }

        #endregion
        //======================================================================================
        #region metody

        public Obraz()
        {
            licznikOperacji = 0;
        }

        public Obraz(Bitmap nowy, string name)
        {
            mainImg = new Bitmap(nowy);
            nazwaPliku = name;
            lastTrans = mainImg;
            ostatniaOperacja = "none";
            licznikOperacji = 0;
        }


        /// <summary>
        /// Metoda wczytuje obraz oraz inicjuje podstawowe informacje.
        /// Przy niepowodzeniu zwraca wartosc 1 a w innych wypadkach 0.
        /// </summary>
        /// <param name="file">nazwa pliku</param>
        /// <returns>
        /// 0 - wczytano plik
        /// 1 - blad odczytu
        /// </returns>
        public int wczytajObraz(string file)
        {
            try
            {
                mainImg = new Bitmap(file);
                nazwaPliku = file;
                lastTrans = mainImg;
                ostatniaOperacja = "none";
                licznikOperacji = 0;
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

        /// <summary> 
        /// Metoda zapisuje obraz do pliku, ktorego nazwa jest generowana 
        /// na podstawie dotychczasowych operacji.
        /// Przy niepowodzeniu zwraca wartosc 1 a w innych wypadkach 0.
        /// </summary>
        /// <returns>
        /// 0 - zapisano plik
        /// 1 - blad zapisu
        /// </returns>
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

        /// <summary>
        /// Przekształcenie z obrazu RGB na odczienie szarości.
        /// </summary>
        /// <returns>obraz wynikowy</returns>
        public Bitmap monochrom()
        {
            Bitmap monochrome = new Bitmap(mainImg);

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


        /// <summary>
        /// Obraca obraz korzystajac z interpolacji biliniowej
        /// </summary>
        /// <param name="angle">kat obrotu w stopniach</param>
        /// <returns>obraz wynikowy</returns>
        public Bitmap rotation(double angle)
        {
            Console.WriteLine("Rotacja o kat: " + angle+" stopni");
            Bitmap newImage = new Bitmap(mainImg);
            int ny, nx, iFloorX, iFloorY, iCeilingX, iCeilingY;
            double dDis, dAng, dTx, dTy, deltaX, deltaY;
            Color newPixel, topL, topR, botL, botR, top, bot;
            string angleC = angle.ToString();
            angle = (angle + 90) * Math.PI / 180;

            double percent = (mainImg.Height + mainImg.Width) / 10;
            int percentCount = 0;

            for (int ky = 0; ky < mainImg.Height; ky++)
            {
                for (int kx = 0; kx < mainImg.Width; kx++)
                {
                    if ((ky + kx) > percentCount * percent)
                    {
                        percentCount++;
                        Console.Write(" | ");
                    }
                    newImage.SetPixel(kx, ky, Color.Black);

                    
                    nx = kx - mainImg.Width / 2;
                    ny = mainImg.Height / 2 - ky;


                    dDis = Math.Sqrt(ny * ny + nx * nx);
                    dAng = 0.0;


                    if (nx == 0)
                    {
                        if (ny == 0)
                        {
                            newImage.SetPixel(kx, ky, mainImg.GetPixel(kx, ky));
                            continue;
                        }
                        else
                        {
                            dAng = Math.Atan2((double)nx, (double)ny);
                        }
                    }
                    else
                    {
                        dAng = Math.Atan2((double)nx, (double)ny);
                    }


                    dAng -= angle;

                    dTx = dDis * Math.Cos(dAng);
                    dTy = dDis * Math.Sin(dAng);

                    dTx = dTx + (double)(mainImg.Width / 2);
                    dTy = dTy + (double)(mainImg.Height / 2);

                    iFloorX = (int)(Math.Floor(dTx));
                    iFloorY = (int)(Math.Floor(dTy));
                    iCeilingX = (int)(Math.Ceiling(dTx));
                    iCeilingY = (int)(Math.Ceiling(dTy));

                    if (iFloorX < 0 || iCeilingX < 0 || iFloorX >= mainImg.Width || iCeilingX >= mainImg.Width || iFloorY < 0 || iCeilingY < 0 || iFloorY >= mainImg.Height || iCeilingY >= mainImg.Height)
                    {
                        continue;
                    }

                    deltaX = dTx - (double)iFloorX;
                    deltaY = dTy - (double)iFloorY;

                    topL = mainImg.GetPixel(iFloorX, iFloorY);
                    topR = mainImg.GetPixel(iCeilingX, iFloorY);
                    botL = mainImg.GetPixel(iFloorX, iCeilingY);
                    botR = mainImg.GetPixel(iCeilingX, iCeilingY);

                    // interpolacja liniowa pomiêdzy górnymi pixelami
                    top = Color.FromArgb(   (byte)((1 - deltaX) * topL.R + deltaX * topR.R),
                                            (byte)((1 - deltaX) * topL.G + deltaX * topR.G),
                                            (byte)((1 - deltaX) * topL.B + deltaX * topR.B));

                    // interpolacja liniowa miêdzy dolnymi pixelami
                    bot = Color.FromArgb(   (byte)((1 - deltaX) * botL.R + deltaX * botR.R),
                                            (byte)((1 - deltaX) * botL.G + deltaX * botR.G),
                                            (byte)((1 - deltaX) * botL.B + deltaX * botR.B));
                    
                    // interpolacja liniiowa miêdzy górnym a dolnym wynikiem
                    newPixel = Color.FromArgb(  (byte)((1 - deltaY) * top.R + deltaY * bot.R),
                                                (byte)((1 - deltaY) * top.G + deltaY * bot.G),
                                                (byte)((1 - deltaY) * top.B + deltaY * bot.B));
                    

                    //ustawienie koloru nowego pixela
                    newImage.SetPixel(kx, ky, newPixel);

                }
            }

            Console.WriteLine("Koniec!!!");
            lastTrans = newImage;
            ostatniaOperacja = "Rotacja_bilinowa_o_" + angleC + "_stopni";
            return newImage;

        }

        /// <summary>
        /// filtruje obraz maska laplasjan
        /// </summary>
        /// <param name="alpha">parametr tworzenia maski</param>
        /// <returns>obraz wynikowy</returns>
        public Bitmap laplacian(double alpha)
        {

            Console.WriteLine("Filtracja laplasjan maska z parametrem alpha: " + alpha);
            Bitmap newImage = new Bitmap(mainImg);
            double[,] mask = new double[3,3];

            mask[0,0] = mask[0,2] = mask[2,0] = mask[2,2] = (1 / (alpha + 1)) * alpha;
            mask[0,1] = mask[1,0] = mask[1,2] = mask[2,1] = (1/(alpha+1))*(1-alpha);
            mask[1,1] = (1/(alpha+1))*-4;

            double R, G, B;
            Color tmp;

            double percent = (mainImg.Height + mainImg.Width) / 10;
            int percentCount = 0;

            for(int kz=1;kz<newImage.Height-1;kz++)
            {
                for(int kx=1;kx<newImage.Width-1;kx++)
                {

                    if ((kz + kx) > percentCount * percent)
                    {
                        percentCount++;
                        Console.Write(" | ");
                    }

                    R = 0; G = 0; B = 0;
                    for(int mz=0,z=kz-1;mz<3;mz++,z++)
                    {
                        for(int mx=0,x=kx-1;mx<3;mx++,x++)
                        {
                            tmp = mainImg.GetPixel(x,z);
                            R += mask[mz,mx] * tmp.R;
                            G += mask[mz,mx] * tmp.G;
                            B += mask[mz,mx] * tmp.B;
                        }
                    }
                    newImage.SetPixel( kx,kz, Color.FromArgb((byte)R,(byte)G,(byte)B));
                }
            }

            Console.WriteLine("Koniec!!!");
            lastTrans = newImage;
            ostatniaOperacja = "Filtracja_laplacian_" + alpha + "_alpha";
            return newImage;
        }

        /// <summary>
        /// filtruje obraz maska laplasjan
        /// </summary>
        /// <param name="alpha">parametr tworzenia maski</param>
        /// <returns>obraz wynikowy</returns>
        public Bitmap unsharp(double alpha)
        {

            Console.WriteLine("Filtracja unsharp maska z parametrem alpha: " + alpha);

            Bitmap newImage = new Bitmap(mainImg);
            double[,] mask = new double[3, 3];

            mask[0, 0] = mask[0, 2] = mask[2, 0] = mask[2, 2] = (1 / (alpha + 1)) * (-alpha);
            mask[0, 1] = mask[1, 0] = mask[1, 2] = mask[2, 1] = (1 / (alpha + 1)) * (alpha-1);
            mask[1, 1] = (1 / (alpha + 1)) * (alpha+5);

            double R, G, B;
            Color tmp;

            double percent = (mainImg.Height + mainImg.Width) / 10;
            int percentCount = 0;

            for (int kz = 1; kz < newImage.Height - 1; kz++)
            {
                for (int kx = 1; kx < newImage.Width - 1; kx++)
                {

                    if ((kz + kx) > percentCount * percent)
                    {
                        percentCount++;
                        Console.Write(" | ");
                    }

                    R = 0; G = 0; B = 0;
                    for (int mz = 0, z = kz - 1; mz < 3; mz++, z++)
                    {
                        for (int mx = 0, x = kx - 1; mx < 3; mx++, x++)
                        {
                            tmp = mainImg.GetPixel(x, z);
                            R += mask[mz, mx] * tmp.R;
                            G += mask[mz, mx] * tmp.G;
                            B += mask[mz, mx] * tmp.B;
                        }
                    }
                    newImage.SetPixel(kx, kz, Color.FromArgb((byte)R, (byte)G, (byte)B));
                }
            }

            Console.WriteLine("Koniec!!!");
            lastTrans = newImage;
            ostatniaOperacja = "Filtracja_unsharp_" + alpha + "_alpha";
            return newImage;
        }

        #endregion
    }
}
