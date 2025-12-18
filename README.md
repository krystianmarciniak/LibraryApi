# Library API

Proste REST API zrealizowane w ASP.NET Core (.NET 8) z użyciem Entity Framework Core i SQLite.

Projekt implementuje operacje CRUD dla encji Author i Book.
Struktura endpointów i format odpowiedzi są zgodne z dostarczonymi testami automatycznymi (wynik: 3/3).

Uwaga: w trybie deweloperskim baza danych jest resetowana przy starcie aplikacji, aby zapewnić powtarzalne i jednoznaczne wyniki testów automatycznych.

## Widoki aplikacji

### Wyniki testów automatycznych


![Wyniki testów API: 3 / 3p.](./screenshots/Wyniki_testow_1.png)

![Wyniki testów API: 3 / 3p.](./screenshots/Wyniki_testow_2.png)

![Wyniki testów API: 3 / 3p.](./screenshots/Wyniki_testow_3.png)

Aby uruchomić aplikację, należy wykonać w terminalu polecenie:

dotnet run


Testy można uruchomić przez http://localhost:5147/index.html




