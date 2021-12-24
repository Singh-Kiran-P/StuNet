# Plan Sprint 3
## Meeting Information
**Meeting Date/Time:** 24/12/2021, 9:15 - 9:58  
**Meeting Purpose:** Prepare sprint 3  
**Meeting Location:** Online  
**Meeting Organizer:** Senn  
**Note Taker:** Tijl  

## Attendees
People who attended:
- Senn
- Kiran
- Jochem
- Tijl

## Agenda Items

Item | Description
---- | ----
Evaluation sprint 2 | • Wie heeft wat klaar gekregen?<br>• Voldoende tijd?<br>• <br>• <br>• 
Prepare sprint 3 | • Wie gaat wat doen?<br>• <br>• <br>• <br>• 

## Discussion Items
Item | Who | Notes |
---- | ---- | ---- |
Vraag stellen en beantwoorden | Senn | <br>• Lander had nog wat dingen nodig van de backend en heeft dus wat moeten aanpassen (1d)<br>• Er was ook nog geen link tussen de prof en de course, dus hebben we daar ook aan gewerkt.<br>• Mails uitsturen course<br>• Daarvoor hebben we Kiran zijn template gebruikt<br>• Template in orde gebracht, route toegevoegd, alles werkt<br>• Bij het stellen van een vraag wordt er naar het emailadres een mail verzonden met vraag, titel<br>• We zijn gisteren begonnen aan het ontvangen (geholpen door Kiran)<br>• We kregen het eerst niet opgestart met de backend, maar het lukt nu op een aparte thread<br>• De mail moet enkel nog geparsed worden, zodat het correct in de database wordt gestopt, en correct getoond wordt in de frontend. |
Abonnees | Tijl | <br>• zaterdag en maandag aan gewerkt, moeilijkheden met hoe het weer te geven in de database. Gekozen voor aparte notificatie tabel tegenover het bijhouden van lijsten. Bij samenwerken met Jochen was er wat code duplicatie, maar alles werkt. Puntje is volledig afgewerkt. |
SignalR | Kiran | <br>• We hebben gezocht hoe dat werkte allemaal, blijkbaar niet zo moeilijk, ik heb Kiran gedaan<br>• Jochem heeft dan de frontend afgehandeld, en toen werkte het<br>• Het werkt allemaal |
SignalR | Jochem | • Notifee was veel makkelijker dan we hadden ingeschat |
Substring matching | Senn | • Het zoeken naar een course ook toegevoegd in de backend, samen met Lander |
Afwerken account | Jochem | • Was slechts 20 minuutjes werk<br>• Alleen de gekke code in de frontend weet ik niet hoe weg te werken<br>• Er is geen enkele variabele naam, het datatype is gek - ik heb geen idee hoe het werkt |
Vakemail | Senn | • Vak emails zijn nog niet aan te passen via EditCourse |
Description bij vak aanmaken | Senn | • Hebben ik en Lander gedaan |
Antwoord accepteren | Jochem | • Is ook gebeurd |
Tests | Jochem | • Heb ik gisteren al gedaan<br>• Het is wel heel veel werk om aan de user te komen |
Tests | Senn | • Lander had ook een geschreven voor de email bij het aanmaken van de course<br>• HttpContext nabootsen is niet gemakkelijk<br>• Deze dingen kunnen we nog wel in orde brengen |
Refactoring | Tijl | • Ik heb nog een helft die getest moet worden alvorens gemerged
Volgende meeting | Tijl | • Vrijdag 31/12, weer om 9u

## Planning iteratie 2
### TODO list
#### Extra-ish
- account aanmaken
  - 2 jaren (jaar verwijderen/negeren)
- vak aanmaken
  - description
- vak opzoeken
  - enkel de filters en substring matching
  - vanuit home pagina navigeren naar genavigeerde vakken
  - vanuit home pagina interessante vakken bekijken
- vraag opzoeken
  - voorlopig nog terug te vinden in een lijst
  - filteren op topics
- vak beheren
  - gebruiker op admin rechten controleren
  - routes beschermen

#### requirement-ish
Use cases met moeilijkheidsgraad van 1 tot 5.

- vraag stellen
  - 1: e-mail uitsturen naar vakmail/proffen
- vraag beantwoorden
  - 4: via e-mail beantwoorden
- antwoord accepteren
  - 2: accepteer knop voor proffen en de vraagsteller
- abonneren op een vak & vraag
  - 3: abonnees toevoegen aan database
  - 1: geabonneerden tonen in home
  - 4: notificaties uitsturen voor geaboneerde vakken
    - textchannels
    - vragen
- bericht sturen in textchannel
  - 3: berichten krijgen -> notificatie
<!-- - vak aanmaken
  - assistenten toevoegen -->
- *home pagina aanmaken* (zie vak opzoeken)
  - 2: mogelijks interessante

### Verdeling sprints

#### Sprint B1
- **bericht sturen in textchannel**
- **filesystem**

#### Sprint B2
- **vraag stellen**
- **afwerking: account aanmaken**
- vraag beantwoorden
- **abonneren op een vak & vraag**
- **bericht sturen in textchannel: berichten krijgen -> notificatie**
- **afwerking: filteren op topics + substring**
- **antwoord accepteren**
- **description toevoegen aan een vak**

#### Sprint B3
- analyse
- diagrammen
- alle routes protecten
- afwerking: home
  - vanuit home pagina navigeren naar genavigeerde vakken
  - vanuit home pagina interessante vakken bekijken

#### Sprint B4
- analyse
- Deployment: kijken naar hoe de code gedeployed moet worden
- presentatie voorbereiden

## Action Items
| Done? | Item | Responsible | Due Date |
| ---- | ---- | ---- | ---- |
| | Analyse Feedback verwerken | Tijl | 31/12/21 |
| | Bestaande diagrammen updaten | Lander & Melih | 31/12/21 |
| | Diagrammen (SD voor emails, ...) aanmaken | Lander & Melih | 31/12/21 |
| | Routes beschermen | Kiran | 31/12/21 |
| | Voor elke controller een unit test | Jochem | 31/12/21 |
| | Afhandelen ontvangen mails | Senn & Kiran | 31/12/21 |
| | Afwerken Home screen | Lander | 31/12/21 |
| | Aanpassen vak mail | Lander | 31/12/21 |

## Other Notes & Information
N/A