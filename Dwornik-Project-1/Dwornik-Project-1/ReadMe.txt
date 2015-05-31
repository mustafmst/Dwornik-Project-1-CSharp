 ========================================================================
                            Wymagania Projektu:
========================================================================

////////////////////////////////////////////////////////////////////////

- Punktowe i geometryczne (monochrom + RGB):
	rotacja obrazu z wykorzystaniem interpolacji biliniowej

- Filtracja przestrzenna (monochrom + RGB):
	filtracja 'laplacian' i 'unsharp' dla zadanego alpha (fspecial). 
	Brzeg odbicie symetr.

- Morfologiczne (monochrom + logiczne):
	zamkniêcie elementem linijnym o zadanym nachyleniu i d³ugoœci

- logiczne (tylko obrazy logiczne):
	mapa odleg³oœci dla zadanej metryki (bwdist)

////////////////////////////////////////////////////////////////////////

Realizacja:

-klasa Obraz służy do prostego wczytywania, modyfikowania i zapisywania obrazów wedłóg wymagań projektowych
-klasa Program jest prostym przykładem wykorzystania klasy Obraz 