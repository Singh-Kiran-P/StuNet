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
            CreateFieldOfStudy(modelBuilder);
            CreateUsers(modelBuilder);
            CreateCourse(modelBuilder);
        }

        private void CreateUsers(ModelBuilder modelBuilder)
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
                    SecurityStamp = "GSXIEJ7H7DWJTSRP24CU5DWFJV4WNFAI",
                    Id = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    PasswordHash = "AQAAAAEAACcQAAAAEM3NAKXyohZdXCFtacPu/m8XMK+7VbOGSSePxwzsA+RcDlg1m9p/5RWvBSJtrgNrjQ==", //abc123
                    ConcurrencyStamp = "de4df913-7e5b-4406-b710-ea134f7b4a43"
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
                    SecurityStamp = "TZEZOM5SNMCQIPT4UPQPHWDJZVAZDIQ2",
                    Id = "7d2b412e-7de8-4341-90f4-49b741e83466",
                    PasswordHash = "AQAAAAEAACcQAAAAEJCOeIX31jPsOvnmQsfwN+7lRjUAAGFJ8ALpjqPTTXjIT9AbdDcr5vJkgK2gsVqK7A==", //abc123
                    ConcurrencyStamp = "83fe0b78-26c9-4c24-a39c-140ec82493b8"
                }
            };

            var studentRole = new IdentityUserRole<string>()
            {
                UserId = students[0].Id,
                RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
            };
            var profRole = new IdentityUserRole<string>()
            {
                UserId = profs[0].Id,
                RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
            };

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Professor>().HasData(profs);
            modelBuilder.Entity<Student>().HasData(students);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(profRole);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(studentRole);
        }

        private void CreateFieldOfStudy(ModelBuilder modelBuilder)
        {
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
        }

        private void CreateCourse(ModelBuilder modelBuilder)
        {
            var courses = new List<Course> {
                new Course() {
                    id = 1,
                    number = "1303",
                    name = "Software Engineering",
                    description = "In dit opleidingsonderdeel maak je kennis met de processen, tools en technieken om complexe, correcte en bruikbare software te bouwen. De verschillende fases van een software engineering process worden bestudeerd. We starten met een basis van requirements engineering. We behandelen diverse procesmodellen voor de ontwikkeling van software, inclusief agiele processen. Technieken zoals test-driven development en refactoring komen aan bod. Na het volgen van dit opleidingsonderdeel, kunnen de studenten (1) principes en kwaliteitsattributen van proces en product uitleggen en nastreven, (2) de fasen van het ontwikkelingsproces, de activiteiten, de resultaten en gerelateerde terminologie uitleggen, (3) een probleem analyseren waarvoor software moet gemaakt worden en dit omzetten in een verzameling gestructureerde vereisten (requirements), (4) UML (Unified Modeling Language) gebruiken voor het maken van een object-georiënteerde analyse en ontwerp van een gesteld probleem, en (5) een software ontwerp omzetten in gestructureerde en onderhoudbare object-georiënteerde code. De studenten verkrijgen ook inzicht in validatie, verificatie en testing, en kunnen de aangeleerde benaderingen toepassen, en verwerven de basisvaardigheden om software design en code gradueel te laten evolueren.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 2,
                    number = "1282",
                    name = "Juridische aspecten van informatica",
                    description = "In dit opleidingsonderdeel worden de juridische aspecten van informatica belicht vanuit een praktische invalshoek. Het opleidingsonderdeel bestaat uit een combinatie van interactieve colleges en oefeningen op basis van casussen.\nEen aantal van de topics die in dit opleidingsonderdeel aan bod komen zijn: intellectuele eigendom, privacybescherming, elektronische contracten, elektronische handel, informaticamisdrijven, telecommunicatierecht, productaansprakelijkheid, consumentenbescherming en netwerkzoeking.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 3,
                    number = "2941",
                    name = "Kansrekening en statistiek",
                    description = "Voor het deel kansrekening:\n1. De student is vertrouwd met basisconcepten in de kansrekening.\n2. De student is vertrouwd met kansexperimenten en met het begrip stochastische variabele.\n3. De student kent de basisregels van de kansrekening.\n4. De student kent de basistechnieken van de combinatieleer.\n5. De student kent de belangrijkste discrete en continue verdelingen.\n6. De student is vertrouwd met voorwaardelijke verdelingen (discreet en continu).\n7. De student is vertrouwd met de verwachtingswaarde, het gemiddelde en de variantie, en de moment genererende functie (van discrete en continue verdelingen).\n8. De student kent de wet van de grote aantallen en de centrale limietstelling.\n9. De student kent de basisconcepten van simulaties en Monte Carlo methoden.\n10. De student kent de basisbegrippen van stochastische processen, telprocessen, Markov processen en Markov ketens.\n11. De student kent de basisconcepten van de simulatie van stochastische processen.\n12. De student is vertrouwd met wachtlijnen, single- en multi-server systemen.\n13. De student kent de basisconcepten van de simulatie van wachtlijn-systemen.\n14. De student kan kanstheoretische problemen oplossen en simulaties uitvoeren met een softwarepakket (R)\nVoor het deel statistiek\n1. De student is vertrouwd met basisconcepten in de beschrijvende statistiek.\n2. De student is vertrouwd met de begrippen steekproef en populatie en kent de basis van steekproeftheorie.\n3. De student is vertrouwd met de eigenschappen van schatters voor parameters.\n4. De student kent de basistechnieken van de statistische inferentie: het construeren van betrouwbaarheidsintervallen en het toetsen van hypothesen.\n5. De student kent de basistechnieken van niet-parametrische testen.\n6. De student kan data beschrijven, betrouwbaarheidsintervallen opstellen en hypothesen toetsen met behulp van de statistische software R\n7. De student kan een niet-parametrische test uitvoeren met het statistisch software pakket R",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 4,
                    number = "0660",
                    name = "Besturingssystemen",
                    description = "Het besturingssysteem is het eerste, en vaak ook het enige, programma dat steeds loopt als een computer werkt. Het verschaft een omgeving waarin andere programma's beschermd en efficient uitgevoerd kunnen worden. Het is een tussenlaag tussen de computer hardware en gebruikersprogrammatuur, en beheert systeemfaciliteiten zoals hoofdgeheugen, CPU rekentijd, en randapparatuur.\n\nIn dit opleidingsonderdeel maakt de student kennis met de fundamentele concepten van werking en programmering van besturingssystemen. De volgende onderwerpen komen onder meer aan bod: - Structuur van besturingssystemen;- Taken die door besturingssystemen vervuld worden - CPU werkindeling; - Procesmanagement: Gelijktijdige processen: interproces-communicatie, procescoördinatie, deadlockdetectie en -preventie; - Multithreading - Geheugenbeheer en het virtueel geheugen - Bestandsystemen en permanente gegevensopslag - Gegevensinvoer en -uitvoer en efficiënte gegevenstoegang - beveiliging en afscherming.\n\nTijdens de oefeningen wordt aandacht besteed aan verschillende technieken die gerelateerd zijn met de theorie, met nadruk op multithreaded programmering.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 5,
                    number = "4380",
                    name = "Bachelorproef",
                    description = "Zelfstandig kunnen analyseren, aanpakken/implementeren en evalueren van een gegeven probleemstelling, ondersteund door een geschreven werk. Het eindwerk is een uiteenzetting van de (nieuwe) informaticaleerstof die men heeft moeten aanwenden en/of bijleren om tot een voltooiing van de opdracht te komen. De student levert een eindproduct af op basis waarvan bepaald wordt op welke manier de specifieke eindcompetenties van de bachelorproef gehaald werden. Concreet omvat dit eindproduct de volgende elementen:\n\n1. de bachelorproeftekst\n2. een mondelinge (poster)presentatie op academisch niveau\n\nDe vereisten voor de bachelorproef en bovenstaande deelaspecten worden omschreven in een specifieke leidraad die de studenten ter beschikking wordt gesteld.\nBinnen de bachelorproef bestaat eveneens de mogelijkheid om seminaries te organiseren door leden van de vakgroep Informatica; dit neemt dan de vorm aan van lezingen die de verschillende profielen van de Master Informatica kaderen. Indien zij georganiseerd worden zijn studenten verplicht deze seminaries bij te wonen. ",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 6,
                    number = "4379",
                    name = "Computernetwerken",
                    description = "Dit opleidingsonderdeel behandelt de basisprincipes van computernetwerken. Onder meer volgende onderwerpen komen aan bod:\n\n- architectuur, ontwerpprincipes, algoritmiek en werking van computernetwerken\n- het hybride OSI-TCP/IP model voor computernetwerken en de principes van gelaagde netwerken, met specifieke focus op de TCP/IP protocolsuite\n- realisatie van praktische implementaties op basis van socket programming (in Python)\n- interpretatie van netwerktraces.\n\nTijdens elk hoorcollege komt een (deel van een) hoofdstuk uit het handboek aan bod, waarbij de docent verwacht dat de studenten dit op voorhand hebben gelezen. Er wordt voornamelijk ingegaan op de moeilijke onderwerpen en op de vragen van de studenten (en de prof). Van de studenten wordt dus zowel een degelijke voorbereiding als actieve participatie verwacht.\nDe theorie wordt verder aan de praktijk getoetst door enerzijds analyses van netwerktraces (die telkens in het volgende hoorcollege worden besproken) en anderzijds programmeeropdrachten waarin zelf netwerksoftware wordt geschreven met socket-gebaseerde communicatie in Python.\nDe practica dienen strikt individueel te worden uitgevoerd, zonder enig overleg met andere studenten of externen.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 7,
                    number = "4377",
                    name = "Theoretische informatica",
                    description = "1. De studenten verwerven de basiskennis van reguliere talen en kunnen deze toepassen.\n2. De studenten verwerven de basiskennis van context-vrije talen en kunnen deze toepassen.\n3. De studenten maken kennis met een toepassing van context-vrije talen: het CYK-parsingalgoritme.\n4. De studenten verwerven de basiskennis van Turing Machines en het begrip beslisbaarheid en kunnen deze toepassen.\n5. De studenten kunnen de verworven kennis over concepten en algoritmen verwerken in een implementatie.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                },
                new Course() {
                    id = 8,
                    number = "1588",
                    name = "Wetenschapsfilosofie",
                    description = "De student verwerft inzicht in de historische evolutie en het eigentijdse functioneren van de wetenschappen in hun maatschappelijke context en is op de hoogte van de diverse kennisbelangen die met wetenschap kunnen worden nagestreefd. Hij/zij kan mee discussiëren over tegenstellingen zoals o.m. deze tussen analytische kennis a priori (wiskunde, logica) versus synthetische kennis a posteriori (ervaringswetenschappen), wetenschappelijke kennis versus alledaagse kennis resp. religieuze en metafysische overtuigingen, reductionisme versus holisme, intrinsieke versus instrumentele waarde. Bovendien slaagt hij/zij er in deze betekenisvol toe te passen op het eigen vakgebied.",
                    courseEmail = "senn.robyns@student.uhasselt.be",
                    profEmail = "prof@uhasselt.be"
                }
            };

            var topics = new List<Topic> {
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
                    name = "Kanstheoretische",
                    courseId = courses[2].id,
                },

                new Topic {
                    id = 8,
                    name = "Steekproeftheorie",
                    courseId = courses[2].id,
                },

                new Topic {
                    id = 9,
                    name = "Procesmanagement",
                    courseId = courses[3].id,
                },

                new Topic {
                    id = 10,
                    name = "Bachelorproeftekst",
                    courseId = courses[4].id,
                },

                new Topic {
                    id = 11,
                    name = "Socket",
                    courseId = courses[5].id,
                },

                new Topic {
                    id = 12,
                    name = "CYK-parsingalgoritme",
                    courseId = courses[6].id,
                },

                new Topic {
                    id = 13,
                    name = "Ervaringswetenschappen",
                    courseId = courses[7].id,
                },
            };

            var questions = new List<Question> {
                new Question {
                    id = 1,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    courseId = courses[0].id,
                    title = "Question 1, all topics for course",
                    body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque dui libero, egestas gravida nisl vitae, luctus ornare felis. Donec eget orci vitae mauris ornare tempor ornare eu felis. Donec commodo nec orci eu lobortis. Nam nec feugiat nibh, quis rhoncus quam. Vivamus sit amet lobortis mi. Nulla bibendum orci ac finibus ultrices. Sed pellentesque quam ac metus elementum, sed gravida sem mollis. Proin facilisis id nisl ut varius. Interdum et malesuada fames ac ante ipsum primis in faucibus. Duis a ipsum fermentum, feugiat lectus eu, sollicitudin quam.",
                    time = DateTime.UtcNow.AddDays (-1)
                },
                new Question {
                    id = 2,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    courseId = courses[0].id,
                    title = "Question 2, single topic",
                    body = "Etiam erat dui, cursus vel pulvinar ac, accumsan varius diam. Nullam dignissim efficitur eros, eu semper dui viverra in. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer dignissim lectus in convallis aliquam. Aliquam consectetur ligula eget felis ultrices eleifend. Proin eu purus lectus. Phasellus pharetra suscipit tempor. Nullam mollis maximus quam, quis maximus mauris semper tincidunt. Nam venenatis, lorem eu pellentesque posuere, sem elit tempor velit, eu tincidunt felis tellus sed odio. Phasellus tristique maximus sem vitae ultrices. Cras ut pharetra nisl, sed varius tellus.",
                    time = DateTime.UtcNow.AddMonths (-1)
                },
                new Question {
                    id = 3,
                    userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                    courseId = courses[1].id,
                    title = "Question 3, no topics",
                    body = "In bibendum dictum mauris, vitae posuere mi fringilla at. Cras dapibus vestibulum risus eu pretium. In varius sed metus id consequat. Suspendisse at interdum leo, eu pharetra nibh. Sed in sollicitudin nunc, a dapibus elit. Phasellus posuere velit a lacinia mattis. Quisque volutpat magna metus, vel dictum dui porttitor id. Cras eu sapien pulvinar, imperdiet leo vel, tincidunt lorem. Nulla facilisi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus quis odio quam. Fusce nec libero eget arcu gravida faucibus. Sed vulputate porttitor ligula, non posuere lectus iaculis eu. Phasellus rhoncus risus laoreet, laoreet felis eu, bibendum ligula.",
                    time = DateTime.UtcNow.AddYears (-1)
                },

            };

            modelBuilder.Entity<Question>()
                .HasMany(q => q.topics)
                .WithMany(t => t.questions)
                .UsingEntity("QuestionTopic", typeof(Dictionary<string, object>),
                    r => r.HasOne(typeof(Topic)).WithMany().HasForeignKey("topicId"),
                    l => l.HasOne(typeof(Question)).WithMany().HasForeignKey("questionId"),
                    je =>
                    {
                        je.ToTable("questiontopics").HasData(
                            new { questionId = 1, topicId = 1 },
                            new { questionId = 1, topicId = 2 },
                            new { questionId = 1, topicId = 3 },
                            new { questionId = 2, topicId = 4 },
                            new { questionId = 2, topicId = 5 },
                            new { questionId = 2, topicId = 6 },
                            new { questionId = 3, topicId = 7 },
                            new { questionId = 3, topicId = 8 },
                            new { questionId = 4, topicId = 9 },
                            new { questionId = 5, topicId = 10 },
                            new { questionId = 6, topicId = 11 },
                            new { questionId = 7, topicId = 12 },
                            new { questionId = 8, topicId = 13 }
                        );
                    }
                );

            var answers = new List<Answer> {
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

            var channels = new List<TextChannel>{
                new TextChannel
                {
                    id = 1,
                    name = "UML Oefeningen",
                    courseId = courses[0].id,
                },
                new TextChannel
                {
                    id = 2,
                    name = "Refactoring Oefeningen",
                    courseId = courses[0].id,
                },
                new TextChannel
                {
                    id = 3,
                    name = "Examen Oef",
                    courseId = courses[0].id,
                },

                new TextChannel
                {
                    id = 4,
                    name = "Privacybescherming Oefeningen",
                    courseId = courses[1].id,
                },
                new TextChannel
                {
                    id = 5,
                    name = "Elektronische contracten Oefeningen",
                    courseId = courses[1].id,
                },
                new TextChannel
                {
                    id = 6,
                    name = "Netwerkzoeking Oefeningen",
                    courseId = courses[1].id,
                },

                new TextChannel
                {
                    id = 7,
                    name = "Kanstheoretische Oefeningen",
                    courseId = courses[2].id,
                },

                new TextChannel
                {
                    id = 8,
                    name = "Steekproeftheorie Oefeningen",
                    courseId = courses[2].id,
                },

                new TextChannel
                {
                    id = 9,
                    name = "Procesmanagement Oefeningen",
                    courseId = courses[3].id,
                },

                new TextChannel
                {
                    id = 10,
                    name = "Bachelorproeftekst Oefeningen",
                    courseId = courses[4].id,
                },

                new TextChannel
                {
                    id = 11,
                    name = "Socket Oefeningen",
                    courseId = courses[5].id,
                },

                new TextChannel
                {
                    id = 12,
                    name = "CYK-parsingalgoritme Oefeningen",
                    courseId = courses[6].id,
                },

                new TextChannel
                {
                    id = 13,
                    name = "Ervaringswetenschappen Oefeningen",
                    courseId = courses[7].id,
                }
            };

            var messages = new List<Message> {
                new Message {
                    id = 1,
                    channelId = channels[0].id,
                    userMail = "student@student.uhasselt.be",
                    body = "Message",
                    time = DateTime.UtcNow.AddDays (-1)
                },
                new Message {
                    id = 2,
                    channelId = channels[0].id,
                    userMail = "prof@uhasselt.be",
                    body = "Reply",
                    time = DateTime.UtcNow.AddHours (-23)
                }
            };

            modelBuilder.Entity<Course>().HasData(courses);
            modelBuilder.Entity<Topic>().HasData(topics);
            modelBuilder.Entity<Question>().HasData(questions);
            modelBuilder.Entity<Answer>().HasData(answers);
            modelBuilder.Entity<TextChannel>().HasData(channels);
            modelBuilder.Entity<Message>().HasData(messages);
        }
    }
}
