# Plan: Prepare sprint 2
## Meeting Information
**Meeting Date/Time:** 20/11/21, 9:10 - 10:08<br>
**Meeting Purpose:** Sprint 2 voorbereiden<br>
**Meeting Location:** Online<br>
**Meeting Organizer:** Tijl / Senn / Kiran<br>
**Note Taker:** Tijl<br>

## Attendees
People who attended:
- Jochem
- Kiran
- Senn
- Melih om 9:30
- Tijl

## Agenda Items

Item | Description
---- | ----
Agenda Item Evaluatie sprint 1 | • Tot waar is elke use case geraakt?<br>• Was de hoeveelheid werk in de tijd te veel, te weinig?<br>• <br>• <br>• 
Agenda Item Planning sprint 2 | • Wat willen we gedaan krijgen tegen het volgende sprint-einde?<br>• Wie doet wat?<br>• Wanneer eindigt de volgende sprint?<br>• <br>• <br>• 

## Discussion Items
Item | Who | Notes |
---- | ---- | ---- |
UC vragen stellen | Jochem | • topics linken naar een course, bij een vraag stellen navigeren naar een vraag<br>• nu is het hardcoded met een course, maar de navigatie naar het vragen zou leuk zijn; gebeurt al, maar is nep<br>• ook linken: many to many relationship<br>• het is heel narrow afgemaakt; er moet nog wat gebeuren met integratie
UC account aanmaken | Kiran | • ge kunt aanmelden<br>• ge kunt registreren, als ge prof zijt krijgt ge automatische rollen a.d.h.v. email<br>• auths van de route zijn ook rollen afhankelijk<br>• we hebben jwt gebruikt met webtokens<br>• front end moet nog gebeuren; werkt met react local storage async<br>• er is wel een probleem bij het beschermen van een pagina; dit moet nog in de front-end gebeuren<br>• frontend cookies bijhouden; gegevens bijhouden; tokens in frontend bijhouden
UC account aanmaken | Senn | • als ge inlogt als een prof, krijgt ge bepaalde pagina's<br>• de pagina's gaan niet toegankelijk zijn voor de andere gebruikers
UC course aanmaken | Tijl | • Voorlopig alleen de frontend<br>• Je kan course toevoegen<br>• Bewerkingen op de server moet nog gebeuren en data naar de server sturen moet ook nog gebeuren<br>• Frontend kan nog enkele aanpassingen krijgen maar server implementeren is prioritair<br>• course object gaat een collection/list van
Evaluatie sprint 1 | Jochem | • We hebben de tijd besteedt dan we gemiddeld gaan
wie doet wat wanneer | Jochem | • tegen het einde van de iteratie willen we:<br>• gebruikers kunnen vragen stellen<br>• liefst al een mail uitsturen (notificatie)


## Action Items
| Done? | Item | Responsible | Due Date |
| ---- | ---- | ---- | ---- |
| | Unit testing | Jochem | woensdag 24/11/21 |
| | Login register frontend, jwt in frontend | Senn, Jochem | zaterdag 27/11/21 |
| | Mail notificaties om wachtwoord te bevestigen | Senn | zaterdag 27/11/21 |
| | Integratie bestaande front end: navigatie tussen bestaande pagina's | Lander | maandag 22/11/21 (avond) |
| | Create course backend code schrijven, linken met front end | Melih, Tijl | maandag 22/11/21 (tegen avond) |
| | vak opzoeken | Kiran | zaterdag 27/11/21 |
| | Front end van een vak | Tijl, Lander | donderdag 25/11/21 |
| | Uploaden van files in filesystem (structuur) | Melih, Kiran | zaterdag 27/11/21 |

## Other Notes & Information
Dtos: Kiran
- email, passwoord, fieldofstudy (optional): als alle required geen errors
- automapper: heel dto mappen op het model
- bv: identity user
- kijkt gewoon of de namen bestaan, en zoniet dan worden ze null
- is het object dat ge van de front end zou kunnen binnenkrijgen, of ernaar versturen