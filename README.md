# Harjoitustyö "viestintäsovellus"

Tämä projekti on viestinvälityssovelluksen taustapalvelu, joka on kehitetty C# ja ASP.NET Corella. Sovellus mahdollistaa käyttäjien lähettää sekä julkisia että yksityisiä viestejä, jotka ovat joko kaikkien tai vain vastaanottajien nähtävissä.

Pääominaisuudet:
- Julkiset viestit ovat kaikkien käyttäjien nähtävissä.
- Yksityiset viestit näkyvät vain lähettäjälle ja vastaanottajalle.
- Käyttäjät voivat rekisteröityä, kirjautua sisään, päivittää ja poistaa omia profiilejaan.
- Käyttäjät voivat muokata ja poistaa vain omia viestejään.
- Sovellus käyttää JWT (JSON Web Token) autentikointiin ja valtuutukseen.

## Projektin rakenne

Projekti on jaettu kansioihin, joissa jokaisessa on tietty osa sovellusta. Tämä parantaa sovelluksen modulaarisuutta, luettavuutta ja testattavuutta. Tässä selitys jokaiselle kansiolle:

### 1. Models

Sisältää tietomallit `User` ja `Message`, jotka määrittelevät sovelluksessa käytettävän tiedon rakenteen:
- `User`: käyttäjä, jolla on attribuutit kuten `UserName`, `Password`, `Email`, `FirstName`, `LastName`, `JoinDate` ja `LastLoginDate`.
- `Message`: viesti, jolla on attribuutit `Title`, `Body`, `Sender`, `Recipient` ja `PrevMessage`. Sisältää linkit viestiketjun aiempiin viesteihin.
- `MessageServiceContext.cs`: tietokannan konteksti, joka liittää sovelluksen `Users` ja `Messages`-tauluihin Entity Frameworkin kautta.

### 2. Controllers

Sisältää `MessagesController.cs` ja `UsersController.cs`. Nämä kontrollerit käsittelevät asiakkaan lähettämät HTTP-pyynnöt ja ohjaavat ne oikeisiin palveluihin:
- `MessagesController.cs` hallitsee viestien lähettämistä ja vastaanottamista. Toteuttaa metodit viestien luomiseen, muokkaamiseen, poistamiseen sekä julkisten ja yksityisten viestien hakemiseen.
- `UsersController.cs` vastaava käyttäjiin liittyvistä toiminnoista: uuden käyttäjän luominen, käyttäjätietojen haku, muokkaus ja käyttäjätilin poistaminen.

### 3. Services

Palvelut sisältävät liiketoimintalogiikan, joka toteuttaa tiedonkäsittelytoiminnot:
- `MessageService.cs` määrittelee viestitoiminnot, kuten viestien luomisen, poistamisen, päivittämisen sekä julkisten ja yksityisten viestien haun.
- `UserService.cs` toteuttaa käyttäjätietoihin liittyvät toiminnot, kuten käyttäjätilin luomisen, käyttäjätietojen hakemisen, päivittämisen ja poistamisen.
- `UserAuthenticationService.cs` hallitsee käyttäjän todennusta, kuten salasanan tarkistusta, tilitietojen luontia ja oikeuksien tarkistusta viestien hallintaan.

### 4.  Repositories

Repositorit hallitsevat tietokantakyselyt ja tiedon saatavuuden:
- `MessageRepository.cs` toteuttaa viestitiedon hallinnan, mukaan lukien viestien lisäys, haku ja poisto.
- `UserRepository.cs` hallitsee käyttäjätietoa, mukaan lukien käyttäjätilin luonti, muokkaus ja poisto.

### 5. Middleware

Middleware (väliohjelmistot) toteuttavat esimerkiksi autentikoinnin ja valtuutuksen:
- `ApiKeyMiddleware.cs` tarkistaa API-avaimen saatavuuden ja oikeellisuuden, joka vaaditaan tiettyihin sovelluksen rajapintoihin.
- `BasicAuthenticationHandler.cs` käsittelee perusautentikoinnin käyttäen Authorization-otsaketta käyttäjänimen ja salasanan tarkistamiseksi.
- `UserAuthenticationService.cs` luo käyttäjän tilitiedot käyttämällä suolaa ja salasanan tiivistämistä sekä tarkistaa käyttäjän oikeudet hallita viestejä.

### 6. Migrations

Sisältää tietokannan migraatiot `Users` ja `Messages`-taulujen luomiseksi. Nämä migraatiot luovat taulut automaattisesti sekä määrittävät suhteet ja indeksit taulujen välillä.

## API-rajapinnat ja niiden käyttö

### Keskeiset rajapinnat

1. Viestien hallinta

- GET /api/Messages: Hakee kaikki julkiset viestit.
- GET /api/Messages/Received/{username}: Hakee käyttäjän yksityiset viestit.
- POST /api/Messages: Luo uuden viestin.
- PUT /api/Messages/{id}: Päivittää olemassa olevan viestin.
- DELETE /api/Messages/{id}: Poistaa käyttäjän viestin.

2. Käyttäjien hallinta

- GET /api/Users: Hakee kaikkien käyttäjien listan.
- POST /api/Users: Luo uuden käyttäjän.
- PUT /api/Users/{username}: Päivittää käyttäjän tietoja.
- DELETE /api/Users/{username}: Poistaa käyttäjätilin.
  
### Esimerkki käyttäjätilin luomisesta ja sovelluksen käytöstä

Kun käyttäjä haluaa luoda käyttäjätilin sovelluksessa, hän lähettää ensin POST-pyynnön osoitteeseen `/api/Users` sisältäen käyttäjätiedot. `UserService` tarkistaa käyttäjänimen saatavuuden ja varmistaa sen ainutlaatuisuuden, minkä jälkeen se tiivistää salasanan ennen tietojen tallentamista tietokantaan.

Tilitunnuksen luomisen jälkeen käyttäjä voi kirjautua sovellukseen sisään päästäkseen suojattuihin toimintoihin, kuten yksityisviestin lähettämiseen. Kirjautuminen tapahtuu `BasicAuthenticationHandlerin` avulla, joka tarkistaa käyttäjän oikeudet ja identiteetin.

Kirjautuneena käyttäjä voi lähettää viestejä POST-pyynnöllä osoitteeseen `/api/Messages`, määrittäen viestille vastaanottajan, jos kyseessä on yksityisviesti, tai jättää vastaanottajakentän tyhjäksi, jolloin viesti on julkinen. `MessageService` luo uuden viestin, ja jos viesti on vastaus aiempaan viestiin, se tallentaa myös viittauksen kyseiseen viestiin luoden näin viestiketjun.

Viestien selaaminen onnistuu GET-pyynnöillä: käyttäjä voi hakea omat yksityiset viestinsä tai katsoa kaikki julkiset viestit. `MessageRepository` suodattaa viestit automaattisesti pyynnön tyypin perusteella, tarjoten näin käyttäjälle halutun viestisisällön.

### Viestin lähetysprosessin logiikka

Kun käyttäjä lähettää viestin, sovellus käsittelee sen seuraavassa järjestyksessä: aluksi `MessagesController` vastaanottaa asiakkaan pyynnön ja välittää viestitiedot eteenpäin `MessageService`-palvelulle. Tämä palvelu luo uuden viestiolion ja toimittaa sen `MessageRepository`-tietovarastolle, joka tallentaa viestin tietokantaan. Jos viesti on vastaus aiempaan viestiin, `MessageService` tallentaa linkin kyseiseen edelliseen viestiin, jolloin syntyy viestiketju. Näin sovelluksen eri toiminnot on jaettu selkeästi osiin SOLID-periaatteiden mukaisesti, mikä parantaa koodin testattavuutta ja luettavuutta.

## Turvallisuus

Turvallisuusratkaisujen osalta sovellus tallentaa salasanat suojatussa muodossa hyödyntäen suolaa ja tiivistämistä, mikä estää salasanan suoran lukemisen tietokannasta. Osaan sovelluksen rajapinnoista pääsy on suojattu API-avaimella, mikä rajoittaa luvattoman käytön mahdollisuuksia. Lisäksi autentikoinnissa käytetään sekä perusautentikointia että JWT-tunnuksia (JSON Web Token), joiden avulla hallitaan resurssien käyttöoikeuksia ja varmistetaan käyttäjän oikeudet eri toimintoihin.
