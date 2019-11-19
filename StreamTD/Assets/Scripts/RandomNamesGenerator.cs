﻿using System.IO;
using Assets.Scripts.Controllers;

namespace Assets.Scripts
{
    public static class RandomNamesGenerator
    {
        public static string GetRandomHumanName()
        {
            return _names[GameController.RandomGenerator.Next(0, _names.Length)];
        }

        private static readonly string[] _names =
        {
            "Julia Onciu",
            "Roger Armstrong",
            "Ηριδανός Ελευθεριάδης",
            "Filofteia Vâlcu",
            "Didina Loteanu",
            "Ondrej Puška",
            "Κωνσταντίνος Κοσμόπουλος",
            "Stella Schulte",
            "Marian Caranica",
            "Anju Limbu",
            "Μενέλαος Ζωγράφος",
            "Pietro Barbieri",
            "Ενιπέας Μιχαλολιάκος",
            "Gilberto Zamora",
            "Dagmara Mucha",
            "Axinte Holda",
            "Octavian Almaș",
            "Tinca Bodnăraș",
            "Codruța Tăbăraș",
            "Frederic Văsescu",
            "Elena Papacioc",
            "Nazlı Şahin",
            "Kristiina Stepanov",
            "Codruța Asachi",
            "Jacopo Leone",
            "Eusațiu Sîrghie",
            "宋 菁",
            "Тимофей Базовкин",
            "Melinda Kuba",
            "Eftimie Dogaru",
            "Charlotte Wood",
            "Gyimesi Edina",
            "Ευαίων Φραγκούδης",
            "Malvina Vatamanu",
            "Beáta Mareček",
            "Darius Pășcanu",
            "Kala Gadal",
            "Miralem Gudelj",
            "Kieran Johnson",
            "Andrea Jenkins",
            "Paridhi Subramanium",
            "Rózsavölgyi Jakab",
            "Mioara Braghiș",
            "Veronika Riha",
            "نرگس علی پور",
            "Ava Page",
            "Daniel Negoiță",
            "Petronela Fajnor",
            "Nora Mocioni",
            "Ica Bucurescu",
            "Luiza Gonzáles",
            "Catherine Garnier",
            "Aura Puțuri",
            "Olga Irimescu",
            "Olympia Chalupka",
            "Λαοδάμας Δραγούμης",
            "Lucreția Catargi",
            "Blahoslav Abrahám",
            "Małgorzata Król",
            " María Carmen Molina",
            "Jeana Pușcaș",
            "西村 あゆみ",
            "Anila Myftiu",
            "Maya Smith",
            "Bryan Contreras",
            "Wale Obi",
            "Eliza Tudorache",
            "Διόδοτος Τρικούπη",
            "Benjamín Guillén",
            "Haralambie Lotru",
            "Teodor Petruș",
            "Luca Schwarz",
            "Madan Shahi",
            "Libby Holmes",
            "Dávid Zsolt",
            "Marta Tudorache",
            "Teresa Williamson",
            "Ngô Ánh",
            "Viorica Rotariu",
            "Ramón Crespo",
            "Simion Sadoveanu",
            "Taalai Bakytov",
            "Valentin Dietrich",
            "Tudora Pleșan",
            "Daniel Fulea",
            "Lennox Arnold",
            "Bethany Allen",
            "Δίας Πολίτης",
            "Lilia Saldaña",
            "Maricica Movilă",
            "Afsana Mahmud",
            "Bakytbek Imanov",
            "Paulína Krížik",
            "Karan Dorje",
            "Arina Pärn",
            "Paweł Kaźmierczak",
            "Gregor Riha",
            "Romeo Răceanu",
            "Εύρυτος Μακρή",
            "Antonia Fulea",
            "Ionică Țenescu",
            "Vratislav Uram",
            "Liam Hardy",
            "Fərhad Qasımlı",
            "Iosif Grigoriu",
            "Herczeg Aladár",
            "Mina Popoviciu",
            "Διόνυσος Κορωναίος",
            "Laura Astaloș",
            "Kai Robinson",
            "Liana Lupescu",
            "Elena Armenta",
            "Tadeáš Medveď",
            "Ružena Jančo",
            "Олег Шаповалов",
            "Ηρωδιανός Αλεβιζόπουλος",
            "Anouk Petit",
            "Hayley Calder",
            "Gicu Papacioc",
            "Martha Sandoval",
            "Müslüm Ergon",
            "Δάμασος Πρωτονοτάριος",
            "Marten Saks",
            "Gizela Král",
            "Minodora Bordea",
            "Alicia Bertrand",
            "Panait Udrea",
            "Κλεομένης Ζωγράφος",
            "Sunita Kharal",
            "Steluța Dolha",
            "Dionýz Huba",
            "Irene Enríquez",
            "Mihail Coteanu",
            "Paulica Bîrlădeanu",
            "Viliam Koza",
            "Ευκλείδης Κορομηλάς",
            "Ethan Aubry",
            "Abdullah Vural",
            "Floriana Dumbrăveanu",
            "Natalia Mora",
            "Crin Bîrlea",
            "Valér Polák",
            "Lily Bonnet",
            "Ερμής Γεωργίου",
            "付 朋",
            "Arsenie Gherghel",
            "Ernis Sultanov",
            "Αφαρέας Κορνάρος",
            "Bagus Hamzah",
            "Maia Hristu",
            "Robin Leblanc",
            "Κλεομήδης Ιωαννίδης",
            "Tanner Gauthier",
            "Antoaneta Bârcianu",
            "Arthur Gillet",
            "Μόψος Φραγκούδης",
            "Geanina Murgeanu",
            "Ahmet Şahin",
            "László Ágoston",
            "Leana Adamescu",
            "Zak Powell",
            "Angélica Barajas",
            "Γεράσιμος Μελετόπουλος",
            "Silvia Drăghici",
            "Arthur Segers",
            "حسن رمضان",
            "Ева Тимофеевa",
            "ياسر وصفي",
            "Melina Buia",
            "Κέφαλος Παπακωνσταντίνου",
            "Filip Ján",
            "Enrico Salvi",
            "David Drăghiceanu",
            "Clément Gilbert",
            "Balázs Flóra",
            "Επίμαχος Καραβίας",
            "Zacharis Berger",
            "Mónica Rubio",
            "Barbu Muraru",
            "Namık Göz",
            "Lydia Kennedy",
            "Kai Kivelä",
            "Karla Rakić",
            "Λέων Ζέρβας",
            "Jana Papacioc",
            "Pătru Olari",
            "Gherghina Raksi",
            "Csertus Rudolf",
            "Rebeca Cihac",
            "Đinh Dung",
            "Teodor Honcescu",
            "Makai Huba",
            "Robin Koppel",
            "Grațiela Cernea",
            "Brianna Illich",
            "Alojz Moravec",
            "Slavomír Vaska",
            "Oscar Knapp",
            "Стас Тимофеев",
            "Ηρόδοτος Χατζηιωάννου",
            "Diana Hušo",
            "Várhegyi Huba",
            "Peter Doyle",
            "Oto Vojtko",
            "Melina Ciorbea",
            "Edward Miller",
            "Ισαάκιος Κομνηνός",
            "Velit Üstündaş",
            "Dora Paşa",
            "Bina Ayer",
            "Camil Lăzărescu",
            "Αριστομένης Δεσποτόπουλος",
            "Олеся Шаповаловa",
            "Ελισσώνας Βαμβακάς",
            "Isidro Solís",
            "Ισαάκιος Βασιλικός",
            "Bianca Teodorașcu",
            "Costel Moțoc",
            "Izabela Țepeneag",
            "Bibiána Fedor",
            "Julio Sáez",
            "Declan Thompson",
            "Olimpiu Captaru",
            "José Delgado",
            "Elin Isaksen",
            "Sebastian Friedrichs",
            "Διοκλής Καλάρης",
            "Amelia Goian",
            "Marlon Busch",
            "Ευαίων Βάμβας",
            "Catrinel Ivănescu",
            "Aurică Șteflea",
            "Bautista Álvarez",
            "James Joss",
            "Jasmine Pawson",
            "Kushum Bhatt",
            "Teodora Lăzăreanu",
            "Amanda Maican",
            "Oto Krištof",
            "Θεαγένης Καραμανλής",
            "Ramazan Sabuncu",
            "Geanina Petrescu",
            "Ónódi Péter",
            "Delia Pogor",
            "Octavia Chindriș",
            "Ruxandra Prelipceanu",
            "Florian Huidu",
            "Mărioara Tăriceanu",
            "Tursubek Bakytbekov",
            "Álex Fuentes",
            "Eva Τρικούπη",
            "Caterina Ghideanu",
            "Sarah Williams",
            "Veniamin Pavelescu",
            "Aida Anghelescu",
            "Gabriel Bunea",
            "Kadir Benli",
            "Isla Cox",
            "Θεάγης Ρόκας",
            "Carina Baconschi",
            "Valentin Michaud",
            "Csorba Tádé",
            "Louis Chevallier",
            "Δάμασος Ταρσούλη",
            "Carolina Romo",
            "Marianne Abrahamsen",
            "Valentin Denis",
            "Ιπποκλής Μαρκόπουλος",
            "Răducu Nemescu",
            "Adela Petrescu",
            "Sali Tünde",
            "Francisc Păcurariu",
            "يسرا أدهم",
            "Μέμνων Λύτρας",
            "Valentin Carre",
            "Ευμένης Πανταζής",
            "Paul Caranica",
            "Codrin Tăriceanu",
            "ابوالفضل صارمی",
            "Gabriel Hernández",
            "Léa Leroy",
            "Inés Iglesias",
            "علاء ليتم",
            "Ayşıl Bensoy",
            "Zafer Mustafa",
            "Bilal Şen",
            "Bina Kunwar",
            "Başak Sezer",
            "Alicia Martin",
            " José Guadalupe May",
            "Mark van der Wal",
            "Henrieta Negoiță",
            "Hannah Mason",
            "Alexandrina Manciulea",
            "Milica Marek",
            "Σταμάτης Κουντουριώτης",
            "Declan Pelletier",
            "Váradi Irén",
            "Roberto Ortega",
            "Ευμένης Κορωναίος",
            "Temel Mehmet",
            "Magdaléna Marcin",
            "Javier Zaragoza",
            "Jean Walsh",
            "Ερυσίχθονας Γερμανός",
            "Ioana Papacioc",
            "Ηρωδιανός Καραβίας",
            "Rebecca Evans",
            "Nguyễn Lan",
            "Ariana Radovici",
            "Ștefana Seciu",
            "Ion Iepureanu",
            "Márton Cintia",
            "Timea Ogăraru",
            "Genci Dervishi",
            "Olimpia Costea",
            "Ruben Lemmens",
            "松本 杏",
            "Vörös Dolli",
            "Patricia Santiago",
            "邱 建",
            "Φαίδιμος Κουταλιανός",
            "Deák Erzsébet",
            "Δημήτριος Χρηστόπουλος",
            " Luis Alberto Escobedo",
            "Blanka Križan",
            "Virginia Datcu",
            "Maryna Krawczyk",
            "Nicuță Oancea",
            "Mark Johanson",
            "Denisa Nottara",
            "Bogdana Leca",
            "Сергей Цыганков",
            "Anisha Adhikari",
            "Елена Цыганковa",
            "Simon Giraud",
            "Κωνσταντίνος Κουντουριώτης",
            "Tumar Taalaibekov",
            "Roberta Captaru",
            "Saveta Calotă",
            "Bruno Fleury",
            "Denise Clark",
            "Robert Kennedy",
            "石川 海翔",
            "Ιερεμίας Μεταξάς",
            "Octav Celibidache",
            "Ερυσίχθονας Κομνηνός",
            "Jonas Poulsen",
            "Savannah Paquette",
            "Διοκλής Γερμανός",
            "Salomé Da silva",
            "Teresa Castañeda",
            "Miloslava Kozák",
            "Alisha Knight",
            "Albert Mokroš",
            "Angelina McCarter",
            "Ικάριος Γαλάνη",
            "Pamfil Băsescu",
            "Эльвира Цыпченко",
            "Adam Șerbănescu",
            "Juhász Flóra",
            "Bebe Mitu",
            "민 영철",
            "Kaio Santos",
            "Hegyi Szofi",
            "Lentin Avramescu",
            "Relu Irimescu",
            "Olaf van der Pol",
            "Reece Stevens",
            "Hegedűs Rezső",
            "Enver Hendek",
            "Rudolf Sikora",
            "Μενοίτιος Βασιλειάδης",
            "Hugo Muñoz",
            "Δαμέας Παπαδάκης",
            "Ľudovít Truban",
            "David Cioacă",
            "Stanca Iacobescu",
            "Liana Breban",
            "Yenny Montoya",
            "Ιάκωβος Μακρής",
            "Maida Alečković",
            "Izabela Coteanu",
            "木村 徹",
            "Romanița Maluțan",
            "Κωνσταντίνος Λιάπης",
            "Μένανδρος Δημητρακόπουλος",
            "Pompiliu Lucescu",
            "Eduard Căianu",
            "Ioan Văduva",
            "Manuela Pătraș",
            "Noah Janssens",
            "Jani Tibor",
            "Miloslav Melichár",
            "Πτολεμαίος Αλεξάνδρου",
            "Sarkadi Kamilla",
            "Μανουήλ Βαμβακάς",
            "خديجة الوسلاتي",
            "Relu Livescu",
            "李 珊",
            "白 俊",
            "Salamat Aktanbiev",
            "Lauren Kirk",
            "Gilbert Morel",
            "Timothy Wells",
            "Merve Yılmaz",
            "Noemi Gliga",
            "Jeevan Kyestha",
            "Rodrigo Salinas",
            "Alex Arnăutu",
            "Maia Richards",
            "Francesco Greco",
            "Bülent Kızılkaya",
            "Mikayıl Səmədli",
            "Gloria de la Rosa",
            "Benjamin Schumacher",
            "Μέντωρ Κρεστενίτης",
            "Ισμηνίας Ζωγράφος",
            "Ctibor Melicherík",
            "Patrick Hunt",
            "Carol Stroici",
            "Lorena Runceanu",
            "Baki Yılmaz",
            "Cantemir Runceanu",
            "Veerle van den Berg",
            "Γρηγόριος Ηλιόπουλος",
            "董 壮",
            "Maelys Robert",
            "Pătru Bodescu",
            "Horia Coteanu",
            "염 수민",
            "Stephanie Sanders",
            "Sébastien Geerts",
            "Fabia Covaci",
            "Ηγησίας Αλεξόπουλος",
            "Ștefania Frosin",
            "Petrică Soltan",
            "Lucia Fulga",
            "Pavel Baragadiru",
            "Zoja Fabuš",
            "Sunita Limbu",
            "Eremia Medeleanu",
            "Mathieu Lemaitre",
            "Venera Lupașcu",
            "Gabriela Găitan",
            "Brayden White",
            "Jankó Amanda",
            "Camila Vélez",
            "Upendra Bhumi",
            "Demetra Radovici",
            "Marie Møller",
            "Elvíra Cesnak",
            "Toby Tate",
            "Flora Deleanu",
            "Jordan Laporte",
            "Emilian Udrea",
            "Bronislava Vojtek",
            "Haralamb Sadoveanu",
            "Michael Saunderson",
            "Sânziana Sătmărean",
            "汪 文",
            "Arne Mathieu",
            "Eva Döring",
            "莫 波",
            "Κλεινίας Παπαφιλίππου",
            "Даниил Плотников",
            "Πτολεμαίος Αντωνοπούλου",
            "Győri Kamilla",
            "Kevin Järv",
            "Corina Neculce",
            "Zsíros Mirabella",
            "Ξενόφαντος Βασιλόπουλος",
            "Océane Arnaud",
            "Işılay Erdem",
            "Carlos Cardoso",
            "Μίμας Ζέρβας",
            "Λεωτυχίδας Λιακόπουλος",
            "Iuliana Șchiopu",
            "Daiana Moțoc",
            "Ηλίας Ζαχαρίου",
            "Nae Găitan",
            "Ηρωδιανός Μητσοτάκης",
            "Madarász Róbert",
            "Marjut Aalto",
            "Celia Barron",
            " Juan Carlos Flores",
            "Jancsó János",
            "Ευμένης Παπαϊωάννου",
            "Eugene Ray",
            "Makula Jázmin",
            "Nicolaie Florea",
            "Ingrid Jakovlev",
            "Luciana Spirescu",
            "Amy Guzman",
            "朱 婷",
            "Goran Muratović",
            "Francisca Peña",
            "Αχαιός Βούλγαρης",
            "Λύκος Κουντουριώτης",
            "Aaron Layton"
        };
    }
}