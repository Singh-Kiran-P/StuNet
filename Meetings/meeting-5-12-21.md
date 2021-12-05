# Finish iteration 1: Prepare video, analysis
## Meeting Information
**Meeting Date/Time:** 5/12/21, 9:01 - 9:10, 9:30-9:50,  
**Meeting Purpose:** Sprint 3 bespreken  
**Meeting Location:** online  
**Meeting Organizer:** /  
**Note Taker:** Tijl Elens  

## Attendees
People who attended:
- Kiran
- Melih
- Senn 
- Tijl
- Jochem
- Lander

## Notes

## Agenda Items

Item | Description
---- | ----
Voorbereiden einde iteratie 1 | • Video<br>• Evolutie<br>• Resultaat<br>• Diagrams

## Discussion Items
Item | Who | Notes |
---- | ---- | ---- |
Video student | Senn | • Registreren eerst laten zien met eigen UH-mail<br>• Dan proberen in te loggen<br>• Dan email bevestigen (in spam?)<br>• Nog eens proberen inloggen<br>• TODO: Jwt vermelden<br>• Dan bij het zoeken naar een course<br>• Een vraag stellen openen<br>• Vraag schrijven<br>• 2 antwoorden zelf op vraag formuleren<br>• Op antwoord klicken (en pagina bekijken)
Video prof | Tijl | • Aanhalen dat de prof-functionaliteit getoond wordt<br>• Aanmaken course
Video prof | Senn | • Openen, laten zien dat de gebruiker nog is aangemeld<br>
Video prof | Senn | • Gebruik van de professor een default account: prof@uhasselt.be met wachtwoord: abc123• Aanmelden met prof-account<br>• Navigeren naar create course-knop<br>• 
Video prof | Melih | • Uitleggen wat er allemaal is van topic, name en description mogelijk is.
Video prof | Senn | • In search course<br>• Stellen van de vraag kan overbodig zijn<br>• Als tijd wel, anders weglaten
Video prof | Tijl | • Edit course, voeg een topic toe, selecteer alle topics, verwijder een topic<br>• Houd een topic in en pas de naam aan


## Action Items
| Done? | Item | Responsible | Due Date |
| ---- | ---- | ---- | ---- |
| n | Gebruik refactoring / unit tests | Jochem & Tijl | 5/12 12:00 |
| n | Component diagrammen uitzoeken | Tijl | 5/12 12:00 |
| n | Checklist met afgewerkte (functionele)requirements en use cases | Kiran | 5/12 12:00 |
| n | Screenshots (account aanmaken, create course) | Lander & | 5/12 12:00 |
| | Video indienen | Melih | 5/12 14:00
| | Latex afhalen, code zippen en insturen |  | 5/12 20:00
<!-- | n | Feedback van klant? | Tijl | 5/12 | -->
<!-- | n | Deployment diagram | | -->
<!-- | n | Course Description | Tijl & Melih | ? | -->
<!-- | n | Navigeren naar vak | | | -->
## Other Notes & Information
Volgorde video
1. Registreer eerst laten zien met eigen UH-mail
1. Probeer in te loggen
1. E-mail bevestigen (in spam?)
1. Open de applicatie opnieuw en probeer nogmaals in te loggen
1. Vermelden van de jwt token
    1. Er gebeurt authenticatie & role-based authorisatie
    1. De gebruiker krijgt bij het inloggen een token terug
    1. Deze token wordt dan bij elke request meegeven
    1. Het gebruik van de token maakt het mogelijk dat de server stateless is, zorgt ervoor dat de server uitbreidbaar is
1. Dan de zoeken-naar-een-course-pagina opendoen en een course opzoeken
1. Open 'ask question' en stel een vraag
1. Geef erna nog twee antwoorden zelf op de net gestelde vraag
1. Open vervolgens een van de antwoorden <!-- 1. Open, laten zien dat de gebruiker nog is aangemeld --> 
1. Log uit, en leg terwijl uit dat het eigenlijk niet nodig is om te switchen (want veel is voorlopig voor iedereen toegankelijk)
1. Log in als professor met een default account: prof@uhasselt.be met wachtwoord: abc123
1. Navigeer opnieuw naar de create course-knop in het search-course
1. Maak een nieuwe course aan
1. Leg uit wat er allemaal is naam, number, beschrijving (ontbreekt)
1. Leg ook uit over topics, wat hun doel is
1. Navigeer naar Edit-course
1. Voeg een topic toe
1. Selecteer alle topics, deselecteer alle behalve 1, verwijder dan die topic (waarna de selectie zichzelf updatet)
1. Houd een topic in en pas de naam aan
1. Keer terug naar de course en leg uit dat er nog geen updates gebeuren in de view.
1. Stel een nieuwe vraag voor indien er nog tijd voor is om snel te laten zien dat er aan de course een vraag wordt toegevoegd
1. Tonen in de course dat topics werden toegevoegd (pagina herladen.)
1. Terugkeren naar search course
