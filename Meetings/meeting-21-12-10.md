# Feeback Meeting Iteration 1
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
In het **vetgedrukt** staat het origineel, aangevuld met verduidelijking; in het *cursief* staat late interpretatie/schatting van wat er genoteerd was. Alle notities werden gemaakt door `tijl`.

### Zelf-evaluatue Analyse
- Tom: **Ik heb naar jullie analyse gekeken**. **Hoe** denken jullie **het** te hebben **aangepakt na vorige feedback sessie**? Kiran: **We hebben per use case verder gewerkt**... **Github** heeft **automatisch** testing **gedaan**.
- ?: **use case gedaan**. 
- ?: **zorgen dat de belangrijkste features er zijn**

### Zelf-evalutie Groepswerk
- Tijl: **Lander werkt veel aan het project**, maar vindt dat misschien fijn. Het is moeilijk om dat in zijn plaats te zeggen.
- Tijl: **Melih** werkt dan weer iets **minder** dan de rest, maar **hij heeft ook nog vakken uit het eerste**.
- Tom: **Het is wel zo dat sommige dingen afgewerkt zouden moeten zijn**, dus houd hier rekening mee. **Kleine dingetjes die nog niet gedaan zijn** kunnen ophopen. **Als er kritieke dingen waarop iedereen moet wachten** niet gebeuren, kan dat voor problemen zorgen, dus als iemand minder snel dingen lijkt te doen, zou ik hem eerder **minder kritieke dingen** toewijzen.

### Linter
- Tom: Voor **consistentie** kunnen jullie **lintjs gebruiken** en dit **inladen in** de **IDE**; op deze manier wordt voorkomen dat er **ongebruikte variabele** zijn ed. - **alle dingen** die **JETbrains** ook doet.
- Tom: Jullie kunnen zelf de regels hiervoor kiezen. Zo kunnen jullie vastleggen dat **private variabele altijd met een underscore** beginnen, en overal **CamcelCase** gebruikt wordt, wat **kan helpen met consistente stijl te bekomen** die gevraagd wordt in de checklist.
- Tom: Het **kan** ook **geintegreerd worden met continious integration** zodat er een **fout bij het uploaden/inladen** wordt gemeld.
- Tom: Hier moeten jullie **zelf ook een beetje uw weg** **vinden in hoe ver ge erin gaat**.

### Analyse
- Tom: Ik heb het **meer in detail gelezen dan vorige keer, dus deze keer** kan ik **toch dingen van** die in de **vorige** er al in stonden vermelden.

#### Inleiding
- Tom: Iets kleins, er is een **typefout**, namelijk bij **blackboard**.
- Tom: Ik zie dat er geschreven wordt dat **een student** **enkel een vak** kan **zien**, wat ik niet zo goed begrijp. Jochem: **Eerste jaar beleidsinformatica; voor elke aparte richting was er een aparte blackboard instantie**, waardoor de **Discussions van elkaar** **niet zichtbaar** waren. Tom: Ik geloof dat het ondertussen al opgelost is, maar **het is ook maar een kleine notatie**. **Het moet er niet uitgehaald worden** en **als dat er niet meer is zal de oplossing niet plots minder waard worden**.

- Tom: Een pluspunt is de **mails van blackboard heel goed, want ik was er me zelfs niet van bewust dat het ...**
- Tom: Er staat geschreven dat het een **mobiele applicatie** is, maar **waarom mobiel?** **Het is een merkwaardige keuze die** jullie **niet hebben gemotiveerd**, en **het is perfect ok, gewoon tonen dat** jullie **erover nagedacht hebben**.

- Tom: Bij het **vraag verwijderen**...

- Tom: Dat het **eeuwig toegankelijk** is, is misschien niet altijd gewild. Bijvoorbeeld wanneer **een nieuwe prof** **het vak over**neemt, wil hij misschien **vanaf een cleansheet beginnen**. **Misschien eens over nadenken hoe** jullie **dat** zullen **oplossen**. Dit is vervelend **voor de prof**, niet per se voor de student.
- Tom: **Een prof maakt** trouwens **geen blackboard vak**ken **aan; dat doet het onderwijsadministratie en deze geeft de rechten aan de prof**. **Misschien overwegen een nieuwe actor toe te voegen**, want **als het wordt uitgerold zou dat inderdaad zo moeten zijn**. Jullie kunnen echter voorlopig **admin functionaliteiten** **bij de prof houden**, maar dus **als het ooit voor UH zou worden uitgerold dan zal het niet de prof zijn**. De **studiegids wordt door onderwijsadministratie opgezet. Deze weet wie de onderwijsteams zijn**. **In principe gaat het zo op blackboard ook**; **een Course builder geeft ons de rechten** en **zodra we klaar zijn kunnen we die openstellen**. **Alle studenten zitten er** dus **al in in principe**; de **database wordt aangemaakt met data**.
- Kiran: **Moeten we die dingen dan ook nog kunnen aanpassen**? Anders gevraagd: **zouden we nu toch een nieuwe rol aanmaken?** **De code is zo geschreven dat het mogelijk is**. Tom: **Ik zou gewoon een json doen die de hele studiegids exporteert met de hele vakken**, zodat jullie **een datastructurr die al bestaat** gebruiken in de applicatie, maar **uiteindelijk gaat een admin dat doen**. **Het is** alleszins **zeker ok als de vakken uit het niets bestaan**.
- Tom: **Om terug te komen op uw rollen**: **het zou een single sign on zijn**. **Ik denk** zelfs **dat elke applicatie via de single signon gaat**

- Tom: **Als er topics worden aangemaakt worden** ..., maar **wat als er een vraag is die totaal niet in topics past**? Tijl: **Een vraag stellen kan zonder topic - daar moet nog worden over gebrainstormd worden** en dat moet vervolgens gedocumenteerd worden.

#### Systeem attributen

##### Functionele requirements
- Tom: Ik zie meteen dat er **geen extra's** zijn. **Er zijn andere groepen die gewoon alle features hebben beschreven** als **'dit gaan we** zeker **implementeren' want het draagt bij aan de meest belangrijke features**. Kiran: **We hebben er niet over nagedacht**, maar **we hebben nu helemaal onderaan aangehaald** hoe de evolutie is.
- Tom: Er staat '**vragen van lage kwaliteit verwijderen**', maar ik lees dit als **systeem gaat herkennen dat een vraag van lage kwaliteit is**, ondanks dat ik weet dat jullie waarschijnlijk een knop bij elke vraag bedoelen. Dus **op de manier dat het zo vermeld is lijkt het alsof het systeem het doet**, en dat moet worden aangepast.
- Tom: Er staat '**toelaten antwoorden te accepteren**', maar **het leek alsof alleen het onderwijsteam dit kon accepteren**. Elders staat dat het **niet een random student die het een goed antwoord vindt**.
- Tom: **wat** jullie **eventueel wel zouden kunnen doen** is werken met duimen. Tijl: Hier hebben we over nagedacht en gezien dat tegen het idee in gaat van het vragen stellen hebben we dit weggelaten. Tom: **Ik zou momenteel** inderdaad **de duimen weglaten** dan. 
- Tom: **Als ge een automatische footer hebt**, gaan jullie die dan **ook overnemen of een slimmer formatering** gebruiken? Kiran: **De jwt werkt nog niet volledig**. **Stel er is een email open en er komt een mail binnen van inf-stunet met de vraag en body**, dan **bij antwoord moet het antwoord automatisch verwerkt worden en in de applicatie gestoken geworden**. Tom: **Het is toch om in het achterhood te houden dat de footer wordt weggeknipt** als een extra feature.

- Tom: **Requirement 29 is niet duidelijk**. Wat wordt er precies bedoeld? Kiran/Tijl: **Als een email gelekt is, maar niet door de persoon werd gestuurd, dan wordt deze niet verwerkt**.
- Tom: Wat in het geval dat **niemand van de assistenten** **thuis** is en ze deze doorsturen naar iemand van **AR**? Kiran: **Een geforwarde mail zal dan ook niet beantwoord kunnen worden**. Tom: Dit zouden jullie ook eventueel kunnen aanhalen in de lijst van extra's om te tonen dat jullie hierover hebben nagedacht.

##### Niet-functionele requirements
- Tom: Ik zou van '**ongeveer 3**' '**maximum 3**' maken.

##### Constriants
- Tom: De constraint is dus **niet mobiel**, maar **gewoon** een **applicatie**, want het **mocht ook een desktop applicatie zijn**.
- Tom: Bij het **aanmaken van een account** zal dit ook weer **wellicht via single signon** gebeuren, **dus** er zouden **geen aparte accounts** moeten worden aangemaakt bij het uitrollen.

#### UC
- Tom: Let wel op met het jaar, want **een student zit niet altijd gewoon in een jaar**. **Melih zit bijvoorbeeld al over 2 jaren**. **Het is niet altijd even eenduidig**. **Het zoeken gaat overal**, maar **studenten kunnen slechts op een plaats een vraag stellen**, terwijl **bij de meeste studenten het wel voor**komt **dat ze nog een vak meenemen**. Dit **lijkt mij meer essentieel**, en zou dus wel opgelost moeten worden.
- Tom: In het **alternatief** scenario staat dat **als ik inlog met mailadres word ik automatisch herkend**.

- ?: **eventuele belangrijke opmerkingen**... Dit **is gerelateerd aan** de reeds gegeven **opmerking van slechte vragen**, namelijk **hoe gaan we herkennen dat een vraag slecht is**. Opnieuw, **dit gaan waarschijnlijk** ...

- ?: **een mogelijkheid is dat het onderwijsteam getagd kan worden in textchannels**

- Tom: Binnen de functionele requirements zal **niet alles** **belangrijk zijn**. Bijvoorbeeld **een shortcut** in zo'n notificatie **kan** al dan niet voorlopig **ook gewoon een mention zijn**. Deze dingen kunnen best geprioritizeerd worden.
<!-- - de notificatie die we in het achterhoofd hadden was dat -->

- Tom: **Hier weer een** extra feature, ge**link**t aan **stack overflow**: **als** iemand **een zeer gelijkaardige vraag stelt kan** een ander **deze** markeren **als duplicaat**. Het **moet niet gebeuren, maar zijn toffe dingen om in het achterhoofd te houden**.

- Tom: **Hier weer een link**, **bij het filteren op niet beantwoorde vragen** kunnen jullie nadenken over **filters**, want **bij het zoeken zijn** die **er nog niet**. Dit houdt bijvoorbeeld **string matching** in, of zoeken naar of **deze beantwoord** is of **deze nog niet**. Ook **punten of karma sprokkelen** kan een leuke extra zijn.

- Tom: Mochten jullie de **filters niet afkrijgen**, dan **krijgen** jullie er **geen punten** voor **af**, maar **het helpt wel als** jullie vermelden dat jullie **er rekening mee gehouden hebben**, door bijvoorbeeld te schrijven **'deze funct**ionele requirement **is nog niet af'**.

- Tom: Er wordt een notificatie uitgestuurd door **een student die** een **vraag gesteld heeft**, maar ook voor elk antwoord. Ook voor elk bericht? **Als een docent of assistent een notificatie krijgt van elk antwoord, dan is het misschien niet zo ideaal**. Jullie **kunnen vermelden dat dit optioneel kan worden aangepast, een extra**, en in tussen tijd een middenweg hiervoor kiezen, dan **is** het **geen super groot probleem**.

- Tom: Wat wel wat **vreemder** is, is dat **ik** als docent of assistent **een mail adres** zou moeten **meegeven** bij het aanmaken van een vak. **Het is niet duidelijk wat het vak-address is**, dus jullie kunnen best **duidelijk aangeven dat het een forwarder is**.
- Tom: Het **alternatie**f scenario voor het aanmaken van **topics** **bij** het **aanmaken** van een vak, **is niet echt een alternatief**, **want** deze zijn **eerder voor wanneer er iets misgaat**. Nu ik er overnadenk: **in een beperkte use case kan het wel zo staan**, **in een uitgebreide use case zal het eerder als extra scenario zijn**.

#### FDUC
- Tom: Het is me niet precies duidelijk wat die getallen (1 tot 5) zijn in de fully dressed use case. Ik zou dan ook duidelijk aangeven dat het **crossreferences** zijn.

#### UCD
- Tom: Opnieuw, zoals voorheen aangehaald, **staat opeens** vanaf hier veel **in het engels**. **De namen komen** ook **niet meer overeen**, terwijl **continuiteit bij artefacten** **aanwezig moet zijn**.
- Tom: Ook, **wat ik hier uit afleidt is dat het onderwijs team geen vragen van andere vakken zien**, omdat de pijlen dat impliceren. Ik denk dat dat was omdat de **proffen kunnen geen search question doen**. Ten slotte 
- Tom: Ten slotte kunnen jullie optioneel ter verduidelijking **erbij plaatsen** dat enkel een specifieke **student** het **antwoord** kan **accepteren**, of een extra **vraagsteller-actor** gebruiken.

#### DM
- Tom: Er zijn wat **inconsistenties**. Zo is er een **mail adres** en later een **email adres** voor **prof en student**. De **dingen die hier nu staan gedefinieerd moeten ook worden door getrokken naar de contracten**, waar het dan ook zo moet worden genoteerd wegens consistentie. Zo zullen **fouten uiteindelijk door** sluipen tot **in** de **code**.

#### Contracten
- Tom: Dan is er ook nog inconsistente naamgeving. Zo is er **askQuestion** op de ene plaats, **submitQuestion** op de andere.
- ?: Op **basis van wat gaat ge controleren dat een vak bestaat?**
- ?: **Op basis van de contracten gaat ge uiteindelijk uw functies schrijven**
- ?: **als ge een gebruiker zou aanmaken met email**
- ?: **als ge twee verschillende mailadressen hebt, dan gaat het hier niet consistent kunnen zijn**
- ?: **dit zijn attributen uit het domein model**

#### Vooruitgang
- Tom: **Als** jullie **minder bekende dingen hebben gebruikt mag er altijd een voetnoot bij vermeld worden**. Dit kan bij bijvoorbeeld **Moq/Fluent** wat voor ons handig kan zijn geweest als jullie **ernaar toe linken**, want we kennen het niet zo goed.

#### Klassen diagrammen
- Tom: De **diagrammen heb ik niet** echt **in detail bekeken**, maar dit **gebeurt wel op het einde**.
<!-- - Kiran: De **backend-code is niet echt moeilijk**, dus mogen we het houden op 1 voorbeeld, of moeten we dat telkens herhalen? Tom: Het hoeft niet allemaal. -->
- Tom: Wat betreft de frontend-**navigatie**, dat **zijn eerder dingen die ge moet laten blijken uit** *sequentie diadrammen*. De **buttons moeten er niet in**, geloof ik. **Ik ga het navragen en laat jullie waarschijnlijk nog iets weten vandaag**.

### Component diagram
- **Kiran: we hebben dit voor elke repository** *hetzelfde gedaan*, dus **we hebben** **1** component-diagram **gemaakt, maar we hebben er eigenlijk een stuk of 10; is dat ok?**
- Tom: Dat is **zeker ok; ik heb het er gisteren met Jan het erover gehad** en dat was **ook de opmerking die Jan gaf**. **Als het jullie helpt dan is het eigenlijk goed**, want **het moet gewoon leesbaar zijn als ge het** in de analyse **zet voor ons; en het moet vooral jullie helpen**.
- Tom: **Jan** had dan wel de opmerking dat een **Node**s **als 3d balken** worden gerepresenteerd. **Alles in een aparte entity**. Jullie **moeten** dus er **3d balken gebruiken**.
- Tom: En hij vondt **het ook** **bij de haren getrokken dat** jullie **storage ...**, **maar als het jullie helpt, dan is het goed**.

### Enit Relation Diagram
- Tom: Is dit **gewoon een export**?
- ?: **links leggen ertussen**
- Kiran: Dit hebben we **gewoon code gewijs gedaan; op het einde kwam er dit uit**. Er zijn dan wel **een paar van de klassen die anders werken terwijl het in de ORM een object was**. Tom: Het was niet gevraagd maar het erbij zetten kan altijd voordelig zijn, ge moet er toch niet veel voor doen.

### Vragen
#### Contracten & Diagrammen
- Tijl: Wat betreft de contracten, **moeten alle functies uitgeschreven worden?** Kris leek dit te impliceren. Tom: **nee, enkel het voornaamste**.
- Tom: Alles wat anders uitgeschreven wordt in de code, **moet worden geupdatet in het domein model**. Als jullie bijvoorbeeld zouden werken met **de like**, **dan moet het ook worden doorgetrokken in** een **contract**.
- Tijl: Hebben we **voldoende diagrammen?** Tom: Er zit geen *maximum of minimum* op. Tijl: Het kan dus mogelijk zijn dat we exact dezelfde diagrammen op het einde van de 2e iteratie hebben? Tom: Ja.

#### Testing & Documentatie
- Tijl: **moet alle**s een unit test krijgen? Tom: Hiervoor moeten jullie **de gulden middenweg** vinden. **Er worden geen punten afgetrokken als er geen unit testing is** *op sommige plaatsen*. Wijzelf hadden alleszins in ons groepje wel alles unit testing gegeven, en dat op het einde. Doe dit dus meteen op het begin wanneer jullie er zelf voordeel aan hebben.
- Tijl: **moet alles** **documentatie hebben?** Tom: **Bij elke functie** moeten jullie **een pre- en post-conditie zetten**. **Doe het enkel** voor **belangrijke functie**s. **Mijn gevoel zou zijn dat als er documentatie bij algemene** ...
- Tijl: Wat zijn dan geen belangrijke functies? **Alle functies die geen getters/setters en geen constrsuctors zijn**? Tom: Ja, zo zou ge het kunnen zien, ja.

### Afsluitende suggesties
- Tom: **Laat zeker nog weten of er nog problemen zijn**, **ge moogt** ons altijd **een mail sturen**.