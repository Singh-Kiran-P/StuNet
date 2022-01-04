using System;
using Server.Api.Config;
using Server.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// https://code-maze.com/migrations-and-seed-data-efcore
// https://www.learnentityframeworkcore.com/configuration/one-to-many-relationship-configuration
// https://henriquesd.medium.com/entity-framework-core-relationships-with-fluent-api-8f741c57b881
// https://dejanstojanovic.net/aspnet/2020/july/seeding-data-with-entity-framework-core-using-migrations

namespace Server.Api.DataBase
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TextChannel> Channels { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        public DbSet<IdentityUserRole<string>> Roles { get; set; }
        public DbSet<CourseSubscription> CourseSubscriptions { get; set; }
        public DbSet<AnswerNotification> AnswerNotifications { get; set; }
        public DbSet<QuestionSubscription> QuestionSubscriptions { get; set; }
        public DbSet<QuestionNotification> QuestionNotifications { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });

            var students = new List<Student>
            {
                new Student() {
                    FieldOfStudyId = 1,
                    Email = "student@student.uhasselt.be",
                    NormalizedEmail = "STUDENT@STUDENT.UHASSELT.BE",
                    UserName = "student@student.uhasselt.be",
                    NormalizedUserName = "STUDENT@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    SecurityStamp = "GSXIEJ7H7DWJTSRP24CU5DWFJV4WNFAI",
                    ConcurrencyStamp = "de4df913-7e5b-4406-b710-ea134f7b4a43",
                    PasswordHash = "AQAAAAEAACcQAAAAEM3NAKXyohZdXCFtacPu/m8XMK+7VbOGSSePxwzsA+RcDlg1m9p/5RWvBSJtrgNrjQ==" //abc123
                },
                new Student() {
                    FieldOfStudyId = 7,
                    Email = "student1@student.uhasselt.be",
                    NormalizedEmail = "STUDENT1@STUDENT.UHASSELT.BE",
                    UserName = "student1@student.uhasselt.be",
                    NormalizedUserName = "STUDENT1@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "310c5066-290f-4735-968f-6a01c4ced67a",
                    SecurityStamp = "DR74ZA3KM3J2CQLDCRNDKYR6T76PMVYA",
                    ConcurrencyStamp = "a5ce3a02-0d48-4e24-8ee6-97cc6d67ea9c",
                    PasswordHash = "AQAAAAEAACcQAAAAEP/I1bTevAWPLRUOYFHsleMGlmTAPiU0z9crCJMaeLfkL1I02Z5Lx5EhNH4fOAWT2Q==" //abc123
                },
                new Student() {
                    FieldOfStudyId = 5,
                    Email = "student2@student.uhasselt.be",
                    NormalizedEmail = "STUDENT2@STUDENT.UHASSELT.BE",
                    UserName = "student2@student.uhasselt.be",
                    NormalizedUserName = "STUDENT2@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "4f2f5684-cec0-4457-ae2a-7cd73e53c8b9",
                    SecurityStamp = "JGGQ2VKBERLLNTL2GOK2UQXVK3UPCUYM",
                    ConcurrencyStamp = "b50c49b7-b8cd-487e-a449-4c7fd2543539",
                    PasswordHash = "AQAAAAEAACcQAAAAEFf7Xdkdes6TByfOYYmu9sN4K1xx3AX0EITa32PAz5urKobJVVy57xcgO3rvK9GhwA==" //abc123
                },
                new Student() {
                    FieldOfStudyId = 10,
                    Email = "student3@student.uhasselt.be",
                    NormalizedEmail = "STUDENT3@STUDENT.UHASSELT.BE",
                    UserName = "student3@student.uhasselt.be",
                    NormalizedUserName = "STUDENT3@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "40baaecf-375d-4854-9f78-7b8bc8e6e102",
                    SecurityStamp = "NZT6UF2IYRYKOXDZZA7H2SODQRGYGG7H",
                    ConcurrencyStamp = "e50df050-357e-462a-ae57-e2d2115fcf6e",
                    PasswordHash = "AQAAAAEAACcQAAAAEJXGzWhvGDBZC18L6K3ixYDQVOORk6ts0LUVqgJlJED/SeI13ePtne7oTjTzTdI/Ag==" //abc123
                },
                new Student() {
                    FieldOfStudyId = 1,
                    Email = "student4@student.uhasselt.be",
                    NormalizedEmail = "STUDENT4@STUDENT.UHASSELT.BE",
                    UserName = "student4@student.uhasselt.be",
                    NormalizedUserName = "STUDENT4@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "ff7a6453-0439-40ec-b30c-e79ce79dc4a6",
                    SecurityStamp = "WGEGLET3G7O5D3NNKG5TA42MRA2DVY7Z",
                    ConcurrencyStamp = "8884b0bc-60ec-4cae-be82-6cde78c4eca4",
                    PasswordHash = "AQAAAAEAACcQAAAAEDi5DCcmyt6OAFh75nVzshU9NEA0Sjd4WUPef2ysxuX41YEuYOMK5Vev91WmjWlhDw==" //abc123
                },
                new Student() {
                    FieldOfStudyId = 4,
                    Email = "student5@student.uhasselt.be",
                    NormalizedEmail = "STUDENT5@STUDENT.UHASSELT.BE",
                    UserName = "student5@student.uhasselt.be",
                    NormalizedUserName = "STUDENT5@STUDENT.UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "f214016b-90c8-41ed-aa3d-4dbab0df8625",
                    SecurityStamp = "3WCB6AQGRJWLQSE5RRTBIMYMEZARAPMT",
                    ConcurrencyStamp = "7dc224fc-69ac-4596-ae55-c04efd844937",
                    PasswordHash = "AQAAAAEAACcQAAAAEFdCioQAxFtgb7ctja6DGSaUIJE6aOOYhcWYdTVPP+ERRjbx3X2ja5yaWc0ZV98Umg==" //abc123
                }
            };

            var profs = new List<Professor>
            {
                new Professor() {
                    Email = "prof@uhasselt.be",
                    NormalizedEmail = "PROF@UHASSELT.BE",
                    UserName = "prof@uhasselt.be",
                    NormalizedUserName = "PROF@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "7d2b412e-7de8-4341-90f4-49b741e83466",
                    SecurityStamp = "TZEZOM5SNMCQIPT4UPQPHWDJZVAZDIQ2",
                    ConcurrencyStamp = "83fe0b78-26c9-4c24-a39c-140ec82493b8",
                    PasswordHash = "AQAAAAEAACcQAAAAEJCOeIX31jPsOvnmQsfwN+7lRjUAAGFJ8ALpjqPTTXjIT9AbdDcr5vJkgK2gsVqK7A==" //abc123
                },
                new Professor() {
                    Email = "prof1@uhasselt.be",
                    NormalizedEmail = "PROF1@UHASSELT.BE",
                    UserName = "prof1@uhasselt.be",
                    NormalizedUserName = "PROF1@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "efdc70a3-7ff1-46a0-8a33-48d83042b259",
                    SecurityStamp = "FO6ETYXBTGSULYOJL362JGNDCW45AY3A",
                    ConcurrencyStamp = "a28fd6d2-f4ca-48ae-b063-5de233e57658",
                    PasswordHash = "AQAAAAEAACcQAAAAELqC8akNOfSKRyC2gXiCwIEimQNxxKWEkFulI6JcTR+dQXwQKJ8kyAon/rqEzsStKg==" //abc123
                },
                new Professor() {
                    Email = "prof2@uhasselt.be",
                    NormalizedEmail = "PROF2@UHASSELT.BE",
                    UserName = "prof2@uhasselt.be",
                    NormalizedUserName = "PROF2@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "eb5b5256-906e-4f97-be88-e47666a857df",
                    SecurityStamp = "GOLUONBWSONCFHJSQ4OBGKP57Q2NUORX",
                    ConcurrencyStamp = "03b440d0-cd49-4512-bca1-c33720747a60",
                    PasswordHash = "AQAAAAEAACcQAAAAEDV3ZynNttjowfQ+/dVaRsxSVVqsmoSOHb5mnHTi7RxOqoJtnS0UiYR83e0QofLOZA==" //abc123
                },
                new Professor() {
                    Email = "prof3@uhasselt.be",
                    NormalizedEmail = "PROF3@UHASSELT.BE",
                    UserName = "prof3@uhasselt.be",
                    NormalizedUserName = "PROF3@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "ff346ff2-e727-4f88-a221-9d9728b7d728",
                    SecurityStamp = "RFH7FN4KSLJKEC2DL7J32U73KJHXZRAS",
                    ConcurrencyStamp = "cbc2cc9f-dae0-435a-8796-58e3dd97002f",
                    PasswordHash = "AQAAAAEAACcQAAAAEIUH779vRE4TVMofmD6SyhnchM4kbqOubtVX9fTiv4mP9/q5HDk8WbP/n7M5v1rg1w==" //abc123
                },
                new Professor() {
                    Email = "prof4@uhasselt.be",
                    NormalizedEmail = "PROF4@UHASSELT.BE",
                    UserName = "prof4@uhasselt.be",
                    NormalizedUserName = "PROF4@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "52afdc0d-0f9a-4b9b-8b66-7bf9b856fdd3",
                    SecurityStamp = "6YTM4AWVOSVXBXERPGGBW7GFMAJ7CQSW",
                    ConcurrencyStamp = "3698a6ea-6991-4a51-b9f0-8ede0ca30961",
                    PasswordHash = "AQAAAAEAACcQAAAAEF8sovgu55Kdi3myaatbhE0gvNjp79a0YU4KMyafbxib3Bpl8E6/D6gpQF2TqPJp2w==" //abc123
                },
                new Professor() {
                    Email = "prof5@uhasselt.be",
                    NormalizedEmail = "PROF5@UHASSELT.BE",
                    UserName = "prof5@uhasselt.be",
                    NormalizedUserName = "PROF5@UHASSELT.BE",
                    PhoneNumber = null,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Id = "8f88e953-f111-4dba-be2e-4fb6efa7dd35",
                    SecurityStamp = "HQ5HJSREDPY6MHUHZRNIFUETSTD6XVNW",
                    ConcurrencyStamp = "74ddbd9a-940b-478c-9f45-67308271c0a3",
                    PasswordHash = "AQAAAAEAACcQAAAAEKc7kBxm3Nfaco7nHVDKfHdZUOQIc0FuTHUeTBqkMgqzWrvjMK/0Oa2pi48t9PRBjA==" //abc123
                }
            };

            var studentRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>() {
                    UserId = students[0].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                },
                new IdentityUserRole<string>() {
                    UserId = students[1].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                },
                new IdentityUserRole<string>() {
                    UserId = students[2].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                },
                new IdentityUserRole<string>() {
                    UserId = students[3].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                },
                new IdentityUserRole<string>() {
                    UserId = students[4].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                },
                new IdentityUserRole<string>() {
                    UserId = students[5].Id,
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                }
            };

            var profRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>() {
                    UserId = profs[0].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityUserRole<string>() {
                    UserId = profs[1].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityUserRole<string>() {
                    UserId = profs[2].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityUserRole<string>() {
                    UserId = profs[3].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityUserRole<string>() {
                    UserId = profs[4].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityUserRole<string>() {
                    UserId = profs[5].Id,
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                }
            };

            modelBuilder.Entity<Professor>().HasData(profs);
            modelBuilder.Entity<Student>().HasData(students);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(profRoles);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(studentRoles);



            var fos = new List<FieldOfStudy>
            {
                new FieldOfStudy() {
                    id = 1,
                    name = "INF",
                    isBachelor = true,
                    fullName = "Bachelor in de Informatica",
                },
                new FieldOfStudy() {
                    id = 2,
                    name = "INF",
                    isBachelor = false,
                    fullName = "Master in de Informatica"
                },
                new FieldOfStudy() {
                    id = 3,
                    name = "BIO",
                    isBachelor = true,
                    fullName = "Bachelor in de Biologie",
                },
                new FieldOfStudy() {
                    id = 4,
                    name = "BIO",
                    isBachelor = false,
                    fullName = "Master in de Biologie"
                },
                new FieldOfStudy() {
                    id = 5,
                    name = "CHEM",
                    isBachelor = true,
                    fullName = "Bachelor in de Chemie",
                },
                new FieldOfStudy() {
                    id = 6,
                    name = "CHEM",
                    isBachelor = false,
                    fullName = "Master in de Chemie"
                },
                new FieldOfStudy() {
                    id = 7,
                    name = "FYS",
                    isBachelor = true,
                    fullName = "Bachelor in de Fysica",
                },
                new FieldOfStudy() {
                    id = 8,
                    name = "FYS",
                    isBachelor = false,
                    fullName = "Master in de Fysica"
                },
                new FieldOfStudy() {
                    id = 9,
                    name = "WIS",
                    isBachelor = true,
                    fullName = "Bachelor in de Wiskunde",
                },
                new FieldOfStudy() {
                    id = 10,
                    name = "WIS",
                    isBachelor = false,
                    fullName = "Master in de Wiskunde"
                }
            };

            modelBuilder.Entity<FieldOfStudy>().HasData(fos);



            var courses = new List<Course>
            {
                new Course() {
                    id = 1,
                    number = "1303",
                    name = "Software Engineering",
                    description = "In dit opleidingsonderdeel maak je kennis met de processen, tools en technieken om complexe, correcte en bruikbare software te bouwen. De verschillende fases van een software engineering process worden bestudeerd. We starten met een basis van requirements engineering. We behandelen diverse procesmodellen voor de ontwikkeling van software, inclusief agiele processen. Technieken zoals test-driven development en refactoring komen aan bod. Na het volgen van dit opleidingsonderdeel, kunnen de studenten (1) principes en kwaliteitsattributen van proces en product uitleggen en nastreven, (2) de fasen van het ontwikkelingsproces, de activiteiten, de resultaten en gerelateerde terminologie uitleggen, (3) een probleem analyseren waarvoor software moet gemaakt worden en dit omzetten in een verzameling gestructureerde vereisten (requirements), (4) UML (Unified Modeling Language) gebruiken voor het maken van een object-georiënteerde analyse en ontwerp van een gesteld probleem, en (5) een software ontwerp omzetten in gestructureerde en onderhoudbare object-georiënteerde code. De studenten verkrijgen ook inzicht in validatie, verificatie en testing, en kunnen de aangeleerde benaderingen toepassen, en verwerven de basisvaardigheden om software design en code gradueel te laten evolueren.",
                    courseEmail = "kiran.singh@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 2,
                    number = "1282",
                    name = "Juridische aspecten van informatica",
                    description = "In dit opleidingsonderdeel worden de juridische aspecten van informatica belicht vanuit een praktische invalshoek. Het opleidingsonderdeel bestaat uit een combinatie van interactieve colleges en oefeningen op basis van casussen.\nEen aantal van de topics die in dit opleidingsonderdeel aan bod komen zijn: intellectuele eigendom, privacybescherming, elektronische contracten, elektronische handel, informaticamisdrijven, telecommunicatierecht, productaansprakelijkheid, consumentenbescherming en netwerkzoeking.",
                    courseEmail = "jochem.lenaers@student.uhasselt.be",
                    profEmail = "prof1@uhasselt.be"
                },
                new Course() {
                    id = 3,
                    number = "2941",
                    name = "Kansrekening en statistiek",
                    description = "Voor het deel kansrekening:\n1. De student is vertrouwd met basisconcepten in de kansrekening.\n2. De student is vertrouwd met kansexperimenten en met het begrip stochastische variabele.\n3. De student kent de basisregels van de kansrekening.\n4. De student kent de basistechnieken van de combinatieleer.\n5. De student kent de belangrijkste discrete en continue verdelingen.\n6. De student is vertrouwd met voorwaardelijke verdelingen (discreet en continu).\n7. De student is vertrouwd met de verwachtingswaarde, het gemiddelde en de variantie, en de moment genererende functie (van discrete en continue verdelingen).\n8. De student kent de wet van de grote aantallen en de centrale limietstelling.\n9. De student kent de basisconcepten van simulaties en Monte Carlo methoden.\n10. De student kent de basisbegrippen van stochastische processen, telprocessen, Markov processen en Markov ketens.\n11. De student kent de basisconcepten van de simulatie van stochastische processen.\n12. De student is vertrouwd met wachtlijnen, single- en multi-server systemen.\n13. De student kent de basisconcepten van de simulatie van wachtlijn-systemen.\n14. De student kan kanstheoretische problemen oplossen en simulaties uitvoeren met een softwarepakket (R)\nVoor het deel statistiek\n1. De student is vertrouwd met basisconcepten in de beschrijvende statistiek.\n2. De student is vertrouwd met de begrippen steekproef en populatie en kent de basis van steekproeftheorie.\n3. De student is vertrouwd met de eigenschappen van schatters voor parameters.\n4. De student kent de basistechnieken van de statistische inferentie: het construeren van betrouwbaarheidsintervallen en het toetsen van hypothesen.\n5. De student kent de basistechnieken van niet-parametrische testen.\n6. De student kan data beschrijven, betrouwbaarheidsintervallen opstellen en hypothesen toetsen met behulp van de statistische software R\n7. De student kan een niet-parametrische test uitvoeren met het statistisch software pakket R",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof2@uhasselt.be"
                },
                new Course() {
                    id = 4,
                    number = "0660",
                    name = "Besturingssystemen",
                    description = "Het besturingssysteem is het eerste, en vaak ook het enige, programma dat steeds loopt als een computer werkt. Het verschaft een omgeving waarin andere programma's beschermd en efficient uitgevoerd kunnen worden. Het is een tussenlaag tussen de computer hardware en gebruikersprogrammatuur, en beheert systeemfaciliteiten zoals hoofdgeheugen, CPU rekentijd, en randapparatuur.\n\nIn dit opleidingsonderdeel maakt de student kennis met de fundamentele concepten van werking en programmering van besturingssystemen. De volgende onderwerpen komen onder meer aan bod: - Structuur van besturingssystemen;- Taken die door besturingssystemen vervuld worden - CPU werkindeling; - Procesmanagement: Gelijktijdige processen: interproces-communicatie, procescoördinatie, deadlockdetectie en -preventie; - Multithreading - Geheugenbeheer en het virtueel geheugen - Bestandsystemen en permanente gegevensopslag - Gegevensinvoer en -uitvoer en efficiënte gegevenstoegang - beveiliging en afscherming.\n\nTijdens de oefeningen wordt aandacht besteed aan verschillende technieken die gerelateerd zijn met de theorie, met nadruk op multithreaded programmering.",
                    courseEmail = "tijl.elens@student.uhasselt.be",
                    profEmail = "prof3@uhasselt.be"
                },
                new Course() {
                    id = 5,
                    number = "4380",
                    name = "Bachelorproef",
                    description = "Zelfstandig kunnen analyseren, aanpakken/implementeren en evalueren van een gegeven probleemstelling, ondersteund door een geschreven werk. Het eindwerk is een uiteenzetting van de (nieuwe) informaticaleerstof die men heeft moeten aanwenden en/of bijleren om tot een voltooiing van de opdracht te komen. De student levert een eindproduct af op basis waarvan bepaald wordt op welke manier de specifieke eindcompetenties van de bachelorproef gehaald werden. Concreet omvat dit eindproduct de volgende elementen:\n\n1. de bachelorproeftekst\n2. een mondelinge (poster)presentatie op academisch niveau\n\nDe vereisten voor de bachelorproef en bovenstaande deelaspecten worden omschreven in een specifieke leidraad die de studenten ter beschikking wordt gesteld.\nBinnen de bachelorproef bestaat eveneens de mogelijkheid om seminaries te organiseren door leden van de vakgroep Informatica; dit neemt dan de vorm aan van lezingen die de verschillende profielen van de Master Informatica kaderen. Indien zij georganiseerd worden zijn studenten verplicht deze seminaries bij te wonen. ",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof4@uhasselt.be"
                },
                new Course() {
                    id = 6,
                    number = "4379",
                    name = "Computernetwerken",
                    description = "Dit opleidingsonderdeel behandelt de basisprincipes van computernetwerken. Onder meer volgende onderwerpen komen aan bod:\n\n- architectuur, ontwerpprincipes, algoritmiek en werking van computernetwerken\n- het hybride OSI-TCP/IP model voor computernetwerken en de principes van gelaagde netwerken, met specifieke focus op de TCP/IP protocolsuite\n- realisatie van praktische implementaties op basis van socket programming (in Python)\n- interpretatie van netwerktraces.\n\nTijdens elk hoorcollege komt een (deel van een) hoofdstuk uit het handboek aan bod, waarbij de docent verwacht dat de studenten dit op voorhand hebben gelezen. Er wordt voornamelijk ingegaan op de moeilijke onderwerpen en op de vragen van de studenten (en de prof). Van de studenten wordt dus zowel een degelijke voorbereiding als actieve participatie verwacht.\nDe theorie wordt verder aan de praktijk getoetst door enerzijds analyses van netwerktraces (die telkens in het volgende hoorcollege worden besproken) en anderzijds programmeeropdrachten waarin zelf netwerksoftware wordt geschreven met socket-gebaseerde communicatie in Python.\nDe practica dienen strikt individueel te worden uitgevoerd, zonder enig overleg met andere studenten of externen.",
                    courseEmail = "melih.demirel@student.uhasselt.be",
                    profEmail = "prof5@uhasselt.be"
                },
                new Course() {
                    id = 7,
                    number = "4377",
                    name = "Theoretische informatica",
                    description = "1. De studenten verwerven de basiskennis van reguliere talen en kunnen deze toepassen.\n2. De studenten verwerven de basiskennis van context-vrije talen en kunnen deze toepassen.\n3. De studenten maken kennis met een toepassing van context-vrije talen: het CYK-parsingalgoritme.\n4. De studenten verwerven de basiskennis van Turing Machines en het begrip beslisbaarheid en kunnen deze toepassen.\n5. De studenten kunnen de verworven kennis over concepten en algoritmen verwerken in een implementatie.",
                    courseEmail = "lander.moors@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 8,
                    number = "1588",
                    name = "Wetenschapsfilosofie",
                    description = "De student verwerft inzicht in de historische evolutie en het eigentijdse functioneren van de wetenschappen in hun maatschappelijke context en is op de hoogte van de diverse kennisbelangen die met wetenschap kunnen worden nagestreefd. Hij/zij kan mee discussiëren over tegenstellingen zoals o.m. deze tussen analytische kennis a priori (wiskunde, logica) versus synthetische kennis a posteriori (ervaringswetenschappen), wetenschappelijke kennis versus alledaagse kennis resp. religieuze en metafysische overtuigingen, reductionisme versus holisme, intrinsieke versus instrumentele waarde. Bovendien slaagt hij/zij er in deze betekenisvol toe te passen op het eigen vakgebied.",
                    courseEmail = "tijl.elens@student.uhasselt.be",
                    profEmail = "prof1@uhasselt.be"
                }
            };

            var topics = new List<Topic>
            {
                new Topic {
                    id = 1,
                    name = "UML",
                    courseId = courses[0].id,
                },
                new Topic {
                    id = 2,
                    name = "Refactoring",
                    courseId = courses[0].id,
                },
                new Topic {
                    id = 3,
                    name = "Examen",
                    courseId = courses[0].id,
                },
                new Topic {
                    id = 4,
                    name = "Privacybescherming",
                    courseId = courses[1].id,
                },
                new Topic {
                    id = 5,
                    name = "Elektronische contracten",
                    courseId = courses[1].id,
                },
                new Topic {
                    id = 6,
                    name = "Netwerkzoeking",
                    courseId = courses[1].id,
                },
                new Topic {
                    id = 7,
                    name = "2D Multiplayer",
                    courseId = courses[5].id,
                },
                new Topic {
                    id = 8,
                    name = "Kanstheoretische",
                    courseId = courses[2].id,
                },
                new Topic {
                    id = 9,
                    name = "Steekproeftheorie",
                    courseId = courses[2].id,
                },
                new Topic {
                    id = 10,
                    name = "Procesmanagement",
                    courseId = courses[3].id,
                },
                new Topic {
                    id = 11,
                    name = "Socket",
                    courseId = courses[5].id,
                },
                new Topic {
                    id = 12,
                    name = "WireShark",
                    courseId = courses[5].id,
                },
                new Topic {
                    id = 13,
                    name = "CYK-parsingalgoritme",
                    courseId = courses[6].id,
                },
                new Topic {
                    id = 14,
                    name = "CYK-parsingalgoritme",
                    courseId = courses[6].id,
                },
                new Topic {
                    id = 15,
                    name = "Ervaringswetenschappen",
                    courseId = courses[7].id,
                }
            };

            var questions = new List<Question>
            {
                new Question {
                    id = 1,
                    userId = students[0].id,
                    courseId = courses[0].id,
                    title = "Agile processen",
                    body = "Wat zijn agile processen?",
                    time = DateTime.UtcNow.AddDays(-1.12)
                },
                new Question {
                    id = 2,
                    userId = students[0].id,
                    courseId = courses[0].id,
                    title = "Studeren Examen",
                    body = "Wat moet er allemaal bestudeerd worden van de kopies voor het examen?",
                    time = DateTime.UtcNow.AddDays(-3.44)
                },
                new Question {
                    id = 3,
                    userId = students[0].id,
                    courseId = courses[0].id,
                    title = "Unit Testing",
                    body = "Wanneer moet men stoppen met unit testing?",
                    time = DateTime.UtcNow.AddDays(-9.891)
                },
                new Question {
                    id = 4,
                    userId = students[0].id,
                    courseId = courses[1].id,
                    title = "Intellectuele eigendom",
                    body = "Wat is intellectuele eigendom?",
                    time = DateTime.UtcNow.AddDays(-57.14)
                },
                new Question {
                    id = 5,
                    userId = students[1].id,
                    courseId = courses[1].id,
                    title = "Studeren Examen",
                    body = "Welke hoofdstukken komen aan bod op het examen?",
                    time = DateTime.UtcNow.AddDays(-99.88)
                },
                new Question {
                    id = 6,
                    userId = students[2].id,
                    courseId = courses[2].id,
                    title = "Monte Carlo",
                    body = "Wat zijn de Monte Carlo methoden?",
                    time = DateTime.UtcNow.AddDays(-0.67)
                },
                new Question {
                    id = 7,
                    userId = students[3].id,
                    courseId = courses[2].id,
                    title = "Steekproeftheorie",
                    body = "Wat is een steekproeftheorie?",
                    time = DateTime.UtcNow.AddDays(-19.23)
                },
                new Question {
                    id = 8,
                    userId = students[3].id,
                    courseId = courses[3].id,
                    title = "Deadlockdetectie",
                    body = "Hoe wordt deadlock precies detect?",
                    time = DateTime.UtcNow.AddDays(-0.02)
                },
                new Question {
                    id = 9,
                    userId = students[4].id,
                    courseId = courses[5].id,
                    title = "Network latency",
                    body = "What is the best way to deal with network latency in a synchronized multiplayer game? Could you advise me on an algorithm or method to use?",
                    time = DateTime.UtcNow.AddDays(-3.4)
                },
                new Question {
                    id = 10,
                    userId = students[4].id,
                    courseId = courses[7].id,
                    title = "Reductionisme, Holisme",
                    body = "Reductionisme versus Holisme?",
                    time = DateTime.UtcNow.AddDays(-58.54)
                }
            };

            modelBuilder.Entity<Question>()
                .HasMany(q => q.topics)
                .WithMany(t => t.questions)
                .UsingEntity("QuestionTopic", typeof(Dictionary<string, object>),
                    r => r.HasOne(typeof(Topic)).WithMany().HasForeignKey("topicId"),
                    l => l.HasOne(typeof(Question)).WithMany().HasForeignKey("questionId"),
                    je => {
                        je.ToTable("questiontopics").HasData(
                            new { questionId = 1, topicId = 1 },
                            new { questionId = 1, topicId = 2 },
                            new { questionId = 1, topicId = 3 },
                            new { questionId = 2, topicId = 3 },
                            new { questionId = 4, topicId = 4 },
                            new { questionId = 4, topicId = 5 },
                            new { questionId = 5, topicId = 4 },
                            new { questionId = 5, topicId = 5 },
                            new { questionId = 5, topicId = 6 },
                            new { questionId = 7, topicId = 7 },
                            new { questionId = 7, topicId = 8 },
                            new { questionId = 8, topicId = 9 },
                            new { questionId = 9, topicId = 15 },
                            new { questionId = 9, topicId = 11 }
                        );
                    }
                );

            var answers = new List<Answer>
            {
                new Answer {
                    id = 1,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    questionId = questions[0].id,
                    title = "Answer 1",
                    body = "answer",
                    time = DateTime.UtcNow.AddHours (-5)
                },
                new Answer {
                    id = 2,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    questionId = questions[1].id,
                    title = "Answer 2",
                    body = "answer",
                    time = DateTime.UtcNow.AddHours (-2),
                    isAccepted = true
                },
                new Answer {
                    id = 3,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    questionId = questions[1].id,
                    title = "Answer 3",
                    body = "answer",
                    time = DateTime.UtcNow.AddDays (-18)
                },
                new Answer {
                    id = 4,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    questionId = questions[2].id,
                    title = "Answer 4",
                    body = "answer",
                    time = DateTime.UtcNow.AddMonths (-11)
                }
            };

            var channels = new List<TextChannel>
            {
                new TextChannel {
                    id = 1,
                    name = "Refactoring oefeningen",
                    courseId = courses[0].id,
                },
                new TextChannel {
                    id = 2,
                    name = "Projectgroep zoeken",
                    courseId = courses[0].id,
                },
                new TextChannel {
                    id = 3,
                    name = "Voorbereiding Examen",
                    courseId = courses[0].id,
                },
                new TextChannel {
                    id = 4,
                    name = "Oefeningen Privacybescherming",
                    courseId = courses[1].id,
                },
                new TextChannel {
                    id = 5,
                    name = "Elektronische contracten",
                    courseId = courses[1].id,
                },
                new TextChannel {
                    id = 6,
                    name = "Netwerkzoeking opdracht",
                    courseId = courses[1].id,
                },
                new TextChannel {
                    id = 7,
                    name = "Oefeningen Kanstheorie",
                    courseId = courses[2].id,
                },
                new TextChannel {
                    id = 8,
                    name = "Steekproeftheorie",
                    courseId = courses[2].id,
                },
                new TextChannel {
                    id = 9,
                    name = "Procesmanagement taak",
                    courseId = courses[3].id,
                },
                new TextChannel {
                    id = 10,
                    name = "Discussie Bachelorproef",
                    courseId = courses[4].id,
                },
                new TextChannel {
                    id = 11,
                    name = "Socket oefeningen",
                    courseId = courses[5].id,
                },
                new TextChannel {
                    id = 12,
                    name = "WireShark opdrachten",
                    courseId = courses[5].id,
                },
                new TextChannel {
                    id = 13,
                    name = "CYK-parsingalgoritme",
                    courseId = courses[6].id,
                },
                new TextChannel {
                    id = 14,
                    name = "Ervaringswetenschappen en meer",
                    courseId = courses[7].id,
                }
            };

            var messages = new List<Message>
            {
                new Message {
                    id = 1,
                    channelId = channels[2].id,
                    userMail = profs[0].Email,
                    body = "Stel hier al je vragen over de voorbereiding van het examen!",
                    time = DateTime.UtcNow.AddDays(-1.123)
                },
                new Message {
                    id = 2,
                    channelId = channels[2].id,
                    userMail = students[0].Email,
                    body = "Er is een examen voor dit vak? Hebben we geen super groot project?",
                    time = DateTime.UtcNow.AddHours(-1.091)
                },
                new Message {
                    id = 3,
                    channelId = channels[2].id,
                    userMail = profs[0].Email,
                    body = "Inderdaad! En het staat op 50% van uw punten :)",
                    time = DateTime.UtcNow.AddDays(-1.0456)
                },
                new Message {
                    id = 4,
                    channelId = channels[2].id,
                    userMail = students[0].Email,
                    body = "ah",
                    time = DateTime.UtcNow.AddHours(-0.952)
                },
                new Message {
                    id = 4,
                    channelId = channels[2].id,
                    userMail = students[4].Email,
                    body = "oh",
                    time = DateTime.UtcNow.AddHours(-0.501)
                }
            };

            modelBuilder.Entity<Topic>().HasData(topics);
            modelBuilder.Entity<Answer>().HasData(answers);
            modelBuilder.Entity<Course>().HasData(courses);
            modelBuilder.Entity<Message>().HasData(messages);
            modelBuilder.Entity<Question>().HasData(questions);
            modelBuilder.Entity<TextChannel>().HasData(channels);
        }
    }
}
