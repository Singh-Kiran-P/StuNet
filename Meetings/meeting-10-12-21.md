# Meeting Minutes
## Meeting Information
**Meeting Date/Time:** 10/12, 10:03 - 11:39  
**Meeting Purpose:** Get feedback  
**Meeting Location:** Online  
**Meeting Organizer:** Tom  
**Note Taker:** Tijl  

## Attendees
People who attended:
- Kiran
- Jochem
- Tijl
- Tom
- Lander (9:25)

## Notes
In het **vetgedrukt** staat het origineel, aangevuld met verduidelijking; in het *cursief* staat late interpretatie/schatting van wat er genoteerd was.

### Zelf-evaluatue Analyse
- **Ik heb naar jullie analyse gekeken**
- **Hoe hebben we het aangepakt na vorige feedback sessie**
- **We hebben per use case verder gewerkt,**
- **github automatisch gedaan**
- **use case gedaan**
- **zorgen dat de belangrijkste features er zijn**

### Zelf-evalutie Groepswerk
- **Lander werkt veel aan het project**
- **Melih minder; hij heeft ook nog vakken uit het eerste**
- **Het is wel zo dat sommige dingen afgewerkt zouden moeten zijn**
- **Klein dingetjes die nog niet gedaan zijn kunnen ophopen**

- **Als er kritieke dingen waarop iedereen moet wachten**
- **geen minder kritieke dingen aan hem**

### Linter
- **consistente lintjs gebruiken; inladen in IDE; geen ongebruikte variabele... alle dingen van JETbrains**
- **private variabele altijd met een underscore; CamcelCase**
- **kan helpen met consistente stijl te bekomen**
- **kan geintegreerd worden met continious integration; fout bij het uploaden/inladen**
- **zelf ook een beetje uw weg in vinden in hoe ver ge erin gaat**

### Analyse
- meer in detail gelezen dan vorige keer, dus deze keer toch dingen van de vorige keer

#### Inleiding
- **typefout blackboard**
- **een student kan enkel een vak zien**
- **Eerste jaar beleidsinformatica; voor elke aparte richting was er een aparte blackboard instantie**
- **Discussions van elkaar waren niet zichtbaar**
- **Het is ook maar een kleine notatie**
- **Het moet er niet uitgehaald worden; als dat er niet meer is zal de oplossing niet plots minder waard worden**

- **Mails van blackboard heel goed, want ik was er me zelfs niet van bewust dat het ...**

- **Mobiele applicatie? Waarom mobiel?**
- **Het is een merkwaardige keuze die we niet hebben gemotiveerd**
- **Het is perfect ok, gewoon tonen dat we erover nagedacht hebben**

- **Eeuwig toegankelijk**
- **Een nieuwe prof neemt het vak over**
- **Een prof wil vanaf een cleansheet beginnen**
- **Misschien eens over nadenken hoe we dat oplossen**
- **Voor de prof**
- **Nu gaan we ervan uit dat het altijd eeuwig toegankelijk moet zijn**

- **Vraag verwijderen**
- **Een prof maakt geen blackboard vak aan; dat doet het onderwijsadministratie en deze geeft de rechten aan de prof.**
- **Misschien overwegen een nieuwe actor toe te voegen**

- **Als het wordt uitgerold zou dat inderdaad zo moeten zijn**
- **Admin functionaliteiten zowel bij de prof houden**
- **Als het ooit voor UH zou worden uitgerold dan zal het niet de prof zijn**
- **Studiegids worden door onderwijsadministratie opgezet. Deze weet wie de onderwijsteams zijn.**

- **In principe gaat het zo op blackboard ook**
- **Een Course builder geeft ons de rechten**
- **Zodra we klaar zijn kunnen we die openstellen; alle studenten zitten er al in in principe**

- **Database wordt aangemaakt met data**
- **Moeten we die dingen dan ook nog kunnen aanpassen**
- **Zouden we nu toch een nieuwe rol aanmaken?**
- **Ik zou gewoon een json doen die de hele studiegids exporteert met de hele vakken**
- **Een datastructurr die al bestaat**
- **Uiteindelijk gaat een admin dat doen**
- **Uw vak structuur**
- **Het is zeker ok als de vakken uit het niets bestaan**

- **De code is zo geschreven dat het mogelijk is**

- **Om terug te komen op uw rollen**
- **Het zou een single sign on zijn**
- **Ik denk dat elke applicatie gaat via de single signon gaat**

- **Als er topics worden aangemaakt worden**
- **Wat als er een vraag is die totaal niet in topics past**
- **Een vraag stellen kan zonder topic - daar moet nog worden over gebrainstormd worden**

#### Systeem attributen

##### Functionele requirements
- **geen extra's**
- **er zijn andere groepen die gewoon alle features hebben beschreven**
- **'dit gaan we implementeren' want het draagt bij aan de meest belangrijke features**
- **We hebben er niet over nagedacht**
- **We hebben nu helemaal onderaan aangehaald**
- **vragen van lage kwaliteit verwijderen**
- **systeem gaat herkennen dat een vraag van lage kwaliteit is**
- **op de manier dat het zo vermeld is lijkt het alsof het systeem het doet**
- **toelaten antwoorden te accepteren**
- **het leek alsof alleen het onderwijsteam dit kon accepteren**
- **niet een random student die het een goed antwoord vindt**
- **wat we eventueel wel zouden kunnen doen**
- **ik zou momenteel de duimen weglaten**
- **als ge een automatische footer hebt, ook overnemen of een slimmer formatering overnemen?**

- **de jwt werkt nog niet volledig**
- **stel er is een email open en er komt een mail binnen van inf stunet met de vraag en body**
- **bij antwoord moet het antwoord automatisch verwerkt worden en in de applicatie gestoken geworden**
- **het is toch om in het achterfood te houden dat de footer wordt weggeknipt**

- **requirement 29 is niet duidelijk**
- **als een email gelekt is, maar niet door de persoon werd gestuurd, dan wordt deze niet verwerkt**
- **niemand van de assistentn is thuis van AR**
- **een geforwarde mail zal dan ook niet beantwoord kunnen worden**

##### Niet-functionele requirements
- **ongeveer 3 -> maximum 3**

##### Constriants
- **niet mobiel, gewoon applicatie**
- **mocht ook een desktop applicatie zijn**

- **aanmaken van een account**
- **wellicht via single signon**
- **dus geen aparte accounts voor aanmaken**

#### UC
- **Een student zit niet altijd gewoon in een jaar**
- **Melih zit bijvoorbeeld over 2 jaren**
- **Het is niet altijd even eenduidig**
- **Het zoeken gaat overal**
- **Studenten kunnen slechts op een plaats een vraag stellen**
- **Bij de meeste studenten komt het wel voor dat ze nog een vak meenemen**
- **Lijkt mij meer essentieel**
- **Alternatief: als ik inlog met mailadres word ik automatisch herkend**

- **eventuele belangrijke opmerkingen**
- **is gerelateerd aan opmerking van slechte vragen**
- **hoe gaan we herkennen dat een vraag slecht is**
- **dit gaan waarschijnlijk **
- **een mogelijkheid is dat het onderwijsteam getagd kan worden in textchannels**

- **niet alles zal belangrijk zijn**
- **een shortcut kan ook gewoon een mention zijn**
<!-- - de notificatie die we in het achterhoofd hadden was dat -->

- **Hier weer een link met stack overflow**
- **als ge een zeer gelijkaardige vraag stelt kan iemand die deze markeert als duplicaat**
- **moet niet gebeuren, maar zijn toffe dingen om in het achterhoofd te houden**

- **hier weer een link**
- **bij het filteren op niet beantwoorde vragen**
- **filters bij het zoeken zijn er nog niet**
- **string matching; deze beantwoord, deze nog niet**
- **punten of karma sprokkelen**

- **filters niet afkrijgen**
- **we krijgen geen punten af**
- **het helpt wel als ze er rekening mee gehouden hebben**
- **'deze funct is nog niet af'**

- **een student die de vraag gesteld heeft...**
- **notificatie: als een docent/ass een not krijgt van elk antwoord, dan is het misschien niet zo ideaal**
- **we kunnen vermelden dat dit optioneel kan worden aangepast, een extra**
- **is geen super groot probleem**

- **vreemder: ik moet een mail adres meegeven?**
- **het is niet duidelijk wat het vak address is**
- **duidelijk aangeven dat het een forwarder is**
- **alternatieve topics: bij aanmaken, is niet echt een alternatief - want dat is eerder voor wanneer er iets misgaat**
- **in een beperkte use case kan het wel zo staan**
- **in een uitgebreide use case zal het eerder als extra scenario zijn**

### Uitgebreide UC
- **crossreferences**

### UCD
- **staat opeens in het engels**
- **de namen komen niet meer overeen**
- **continuiteit bij artefacten moeten aanwezig zijn**
- **wat ik hier uit afleidt is dat het onderwijs team geen vragen van andere vakken zien**
- **proffen kunnen geen search question doen**
- **student: antwoord accepteren**
- **vraag steller actor of erbij plaatsen**

### DM
- **inconsistenties: mail adres - email adres (prof, student)**
- **dingen die hier nu staan gedefinieerd moeten ook worden door getrokken naar de contracten**
- **fouten sluipen uiteindelijk door in code**

### 
- **askQuestion / submitQuestion**
- **basis van wat gaat ge controleren dat een vak bestaat?**
- **op basis van de contracten gaat ge uiteindelijk uw functies schrijven**
- **als ge een gebruiker zou aanmaken met email**
- **als ge twee verschillende mailadressen hebt, dan gaat het hier niet consistent kunenn zijn**
- **dit zijn attributen uit het domein model**

### 8 
- **als we minder bekende dingen hebben gebruikt mag er altijd een voetnoot bij vermeld worden**
- **Moq/Fluent -> ernaar toe linken**

- **klassendiagrammen heb ik niet in detail bekeken; gebeurt wel op het einde**
- **backend code is niet echt moeilijk**
- **navigatie zijn eerder dingen die ge moet laten blijken uit systeem navigatie**
- **buttons moeten er niet in**
- **ik ga het navragen en laat jullie waarschijnlijk nog iets weten vandaag**

### Component diagram
- **K: we hebben dit voor elke repository**
- **K: we hebben er 1 gemaakt, maar we hebben er eigenlijk een stuk of 10; is dat ok?**
- **zeker ok; ik heb het er gisteren met Jan het erover gehad**
- **ook de opmerking die Jan gaf**
- **als het jullie helpt dan is het eigenlijk goed**
- **het moet gewoon leesbaar zijn als ge het erin zit voor ons; en het moet vooral jullie helpen**
- **Jan: Node als 3d balken**
- **alles in een aparte entity**
- **we moeten 3d balken gebruiken**
- **het is bij de haren getrokken dat we storage ...**
- **maar als het jullie helpt, dan is het goed**

### Enit R
- **gewoon een export**
- **links leggen ertussen**
- **gewoon code gewijs gedaan; op het einde kwam er dit uit**
- **een paar van de klassen die anders werken terwijl het in de ORM een object was**


- **moet worden geupdate in het domein model**
- **de like? dan moet het ook worden doorgetrokken in contract**


- **voldoende diagrammen?**

### Conctracten
- **moeten alle functies uitgeschreven worden?**
- **nee, enkel het voornaamste**

### Testing
- **``moeten alle dingen getest worden?**
- **vind de gulden middenweg**
- **er worden geen punten afgetrokken als er geen ut is**

- **``moet alles een documentatie hebben?**
- **bij elke functie een pre/post zetten**
- **doe het enkel op belangrijke functie**
- **mijn gevoel zou zijn dat als er documentatie bij algemene **
- **alle functies die geen getters/setters en constrs zijn**

### Afsluitende suggesties
- **laat zeker nog weten of er nog problemen zijn**
- **ge moogt een mail sturen**