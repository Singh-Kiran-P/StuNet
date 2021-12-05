# Feedback: Analysis
## Meeting Information
**Meeting Date/Time:** 8/11/21, ? - ?  
**Meeting Purpose:** Receive feedback for the submitted analysis  
**Meeting Location:** online, at UHasselt  
**Meeting Organizer:** Tom Veuskens  
**Note Taker:** Senn, Tijl  

## Attendees
People who attended:
- Kiran
- Melih
- Senn
- Tijl
- Jochem
- Lander

## Notes
In het **vetgedrukt** staat het origineel, aangevuld met verduidelijking; in het *cursief* staat late interpretatie/schatting van wat er genoteerd was.

### Probleemstelling
- `tijl`: De probleemstelling was geschreven **minder vanuit het probleem, eerder vanuit de oplossing**. **"Wij gaan nu dit doen"** schrijven **is niet de bedoeling.**
- `senn`: **Het probleem is kort, veel oplossing.**
- `senn`: We kunnen **best alles vanuit het probleem zien**.

### Functionele requirements
- `tijl`: **UH-email** aanhalen **is een design constraint; hoeft niet bij Functionele Requirements**.
- `tijl`: *De functionele requirements bevatten* **enkel functies op gebruikers-interactie**.
  - **functies die het systeem doet die niet rechts***treeks...*
- `senn`: **UHasselt-email enkel als constraint** bij **niet functionele requirements**.
- `senn`: *Functionele requirements zijn* **functies die het systeem als blackbox kan uitvoeren** en **die** dus **niet rechtstreeks als gevolg van** *andere* **gebeurtenissen** uitgevoerd worden.

### Niet functionele requirements
- `tijl`: De **attributen zijn** dan wel **meetbaar;** maar **niet zo realistisch**, *en moeten dus nog aangepast worden*.
- `senn`: **Opgelegd te gebruiken technologieen** behoren tot de **Design constraints**.

### Constraints
- `tijl`: De aangehaalde constraints zijn **allemaal extern; emailadres is** ook **extern**.
- `senn`: **Externe vs interne <span style="color: default;">constraints</span> i.p.v. niet functionele, zeg indien <span style="color: default;">deze</span> niet meetbaar is.**

### UC
- `tijl`: **Account aanmaken**: **denk** **goed** na over **wanneer een UC begint/eindigt op registreren of op accepteren klikken**.
- `tijl`: *In het algemeen moet* **elk alternatief moet op succes eindigen**.
- `tijl`: Het volgende is een **design constraint: UC moet email niet meer aannemen** of afwijzen.
- `senn`: Alternatieve scenario moet altijd in succes eindigen.

### Fully Dressed UC
- `tijl`: De **titel** ervan **zou eerder "antwoord bekomen" moeten zijn**, want in het **alternatief scenario stelt** de gebruiker **geen vraag**.
- `tijl`: De **navigatie** zouden we er best uit weglaten en in de plaats **opnemen in precondities**.
- `senn`: "**Antwoord bekomen**" **i.p.v.** "**vraag stellen**".
- `senn`: **Eerste 2 puntjes kunnen in de pre-condities.**
- `senn`: **Answer poperty**.
- `senn`: **validated**.

### UCD
- `tijl`: *...* **moeten dezelfde naam hebben; dezelfde taal**.

### DM
- `tijl`: **answer: accepted?** Geen idee wat er ter sprake werd gebracht...

### Sequentie diagram
- `tijl`: *Er was ook nog een opmerking over naamgeving 'askQuestion' of iets dergelijks, dat blijkbaar niet meer geupdated was in de ingezonden pdf.*

### Contracten
- `tijl`: *De contracten waren goed.*