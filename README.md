# Projektuppgift - DT191G - Webbutveckling med .NET

## Uppgiftsbeskrivning
Denna uppgift avser projektuppgiften i kursen DT191G och innefattar en databasdriven webbapplikation, skapad med ASP.NET Core, Entity Framework och Identity. Webbplatsen är skapad för en fiktiv frisörsalong som heter "LUX Locks". På webbplatsen kan kunder boka tid, läsa och skriva recensioner samt se vilka frisörer och behandlingar som erbjuds. Administrativ personal har möjlighet att hantera bokningar, tjänster, personalinformation och konto.

## Demonstration av webbplats
Applikationen är inte publicerad, men det finns en videodemonstration som visar hur applikationen fungerar. Se [här](https://vimeo.com/1067817969?share=copy)

## Funktionalitet
För kunder:
- Boka tid
- Läsa och skriva recensioner
- Se frisörer
- Se behandlingar

För personal:
- Registrera konto och logga in (Identity-baserad autentisering)
- Hantera bokningar (radera, redigera, se kommande bokningar)
- Hantera recensioner (radera, redigera och se recensioner)
- Hantera personal (se, lägga till, uppdatera personalinformation och radera personal)
- Hantera företagets olika tjänster (se, lägga till, redigera och ta bort behandlingar)

## Teknisk information
### Databashantering
Applikationen använder Entity Framework Core för databasanslutning, där migrationer används för att skapa och uppdatera databasschemat. SQLite används som databas.

Följande tabeller hanteras i databasen:
- AspNetUsers (användarkonton, Identity)
- Appointments (bokningar)
- Reviews (recensioner)
- Stylists (personal)
- Treatments (behandlingar)

### Struktur
Projektet är uppbyggt enligt MVC (Model-View-Controller)-mönstret och innehåller följande komponenter:
- Models: definierar databasmodellerna
- Controllers: hanterar logik och kommunikation mellan databasen och användargränssnittet
- Views: Innehåller Razor-sidor som renderar användargränssnittet

## Models
Applikationen innehåller fyra huvudmodeller:
- AppointmentModel: Innehåller information om kundbokningar
- ReviewModel: Sparar kundrecensioner kopplade till specifika frisörer och tjänster
- StylistModel: Innehåller information om personal
- TreatmentModel: Representerar de olika behandlingar som erbjuds på salongen

## Controllers
Applikationen innehåller följande Controllers som har scaffholdats fram med genererad kod baserad på modellerna:
- AppointmentController
- ReviewController
- StylistController
- TreatmentController

## Views
Projektet innehåller Razor Views för att visa data. Scaffholding har använts för att generera CRUD-gränssnitt för respektive modell. Exempel på vyer:
- Home/Index - startsidan
- Appointments/Create - skapar en ny bokning
- Reviews/Index - visar recensioner
- Stylist/Details - visar detaljer om personal
- Treatment/Delete - hanterar borttagning av behandling

## Skapad av:
- Julie Andersson
- Webbutvecklingsprogrammet på Mittuniversitetet i Sundsvall
- Projektuppgift - kurs DT191G - Webbutveckling med .NET