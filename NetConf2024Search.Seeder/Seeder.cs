using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetConf2024Search.Seeder.Model;

namespace NetConf2024Search.Seeder;

public class Seeder(
    ILogger<Seeder> logger,
    SearchDbContext context,
    IHostEnvironment env)
{
    private readonly ILogger<Seeder> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));
    private readonly SearchDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly static List<Author> _authors = [
        new Author()
        {
            Id = Guid.NewGuid(),
            FirstName = "Stephen",
            LastName = "King",
            Bio = "Stephen King is a renowned author known for his horror novels. He was born in Portland, Maine. With his vivid imagination and masterful storytelling, King has captivated readers around the world. His works, such as 'Carrie', 'The Shining', and 'It', have become iconic in the horror genre. King's ability to create complex characters and build suspenseful narratives has earned him numerous awards and a dedicated fan base."
        },
        new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "J.K.",
            LastName = "Rowling",
            Bio = "J.K. Rowling is the author of the Harry Potter series, one of the most successful book series in history. She was born in Yate, England. Rowling's magical world of Hogwarts and beloved characters like Harry Potter, Ron Weasley, and Hermione Granger have captured the hearts of millions of readers. Her books have inspired a generation and sparked a global phenomenon. Rowling's storytelling prowess and themes of friendship, bravery, and the power of love have made her a literary icon."
        },
        new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "J.R.R.",
            LastName = "Tolkien",
            Bio = "J.R.R. Tolkien is a legendary author known for his fantasy novels, including The Hobbit and The Lord of the Rings. He was born in Bloemfontein, South Africa. Tolkien's richly imagined world of Middle-earth, populated by elves, dwarves, and hobbits, has become a cornerstone of the fantasy genre. His epic tales of adventure, heroism, and the battle between good and evil have inspired generations of readers and influenced countless authors. Tolkien's meticulous world-building and intricate storytelling continue to captivate readers to this day."
        },
        new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "Agatha",
            LastName = "Christie",
            Bio = "Agatha Christie is a famous author known for her mystery novels and iconic detective characters. She was born in Torquay, England. Christie's works, such as 'Murder on the Orient Express' and 'And Then There Were None', have become timeless classics of the mystery genre. Her intricate plots, clever twists, and memorable characters have made her the best-selling novelist of all time. Christie's ability to keep readers guessing until the very end has solidified her status as the Queen of Crime."
        },
        new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "Bad",
            LastName = "Author",
            Bio = "This is a really bad author, nobody should read anything from him."
        },
        new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "Annonymous",
            LastName = "Annonymous",
            Bio = "Annonymous author"
        }
    ];

    private readonly static List<Book> _books =
        [
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Shining",
                Reference = "978-0-385-12167-5",
                Gendre = "Horror",
                AuthorId = _authors[0].Id,
                Author = _authors[0],
                PublishedOn = new DateTime(1977, 1, 28),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 39.99,
                NumberOfPages = 447,
                InStock = false,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Great book!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    },
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Not my cup of tea.",
                        Author = "Jane Smith",
                        PublishedOn = new DateTime(2023, 1, 29)
                    }
                ],
                Summary = "A thrilling horror novel by Stephen King. The main characters are Jack Torrance, Wendy Torrance, and Danny Torrance. The story revolves around the haunted Overlook Hotel and the descent into madness of Jack Torrance, a struggling writer and recovering alcoholic."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "It",
                Reference = "978-0-670-81302-5",
                Gendre = "Horror",
                AuthorId = _authors[0].Id,
                Author = _authors[0],
                PublishedOn = new DateTime(1986, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 19.99,
                NumberOfPages = 1138,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A terrifying and epic tale!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "It is a horror novel that follows a group of friends known as the Losers' Club as they confront an ancient evil entity that takes the form of a clown named Pennywise. The story alternates between their childhood in the 1950s and their adulthood in the 1980s."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Pet Sematary",
                Reference = "978-0-385-12168-2",
                Gendre = "Horror",
                AuthorId = _authors[0].Id,
                Author = _authors[0],
                PublishedOn = new DateTime(1983, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 14.99,
                NumberOfPages = 374,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A haunting and tragic story!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "Pet Sematary is a horror novel about a family who moves to a rural town and discovers a mysterious burial ground in the nearby woods. When they use the burial ground to bring their deceased pet back to life, they unleash a series of horrifying events."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Misery",
                Reference = "978-0-670-81364-3",
                Gendre = "Thriller",
                AuthorId = _authors[0].Id,
                Author = _authors[0],
                PublishedOn = new DateTime(1987, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 14.99,
                NumberOfPages = 310,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A gripping and intense psychological thriller!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "Misery is a psychological thriller about an author who is held captive by his number one fan after a car accident. As the fan's obsession with the author grows, she becomes increasingly violent and controlling."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Stand",
                Reference = "978-0-385-12168-2",
                Gendre = "Post-apocalyptic",
                AuthorId = _authors[0].Id,
                Author = _authors[0],
                PublishedOn = new DateTime(1978, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 19.99,
                NumberOfPages = 1153,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "An epic and immersive journey!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "The Stand is a post-apocalyptic novel that depicts a world devastated by a superflu pandemic. The story follows a group of survivors as they navigate the aftermath and confront the forces of good and evil in a battle for the future of humanity."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Philosopher's Stone",
                Reference = "978-0-7475-3269-6",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(1997, 6, 26),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 223,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "I love this book!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The first book in the Harry Potter series, introducing the magical world of Hogwarts. The main characters are Harry Potter, Ron Weasley, and Hermione Granger. The story follows Harry Potter as he discovers his magical abilities, attends Hogwarts School of Witchcraft and Wizardry, and faces the dark wizard Lord Voldemort."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Hobbit",
                Reference = "978-0-395-48976-2",
                Gendre = "Fantasy",
                AuthorId = _authors[2].Id,
                Author = _authors[2],
                PublishedOn = new DateTime(1937, 9, 21),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English,Spanish",
                Price = 19.99,
                NumberOfPages = 310,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Great book!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "A classic fantasy adventure by J.R.R. Tolkien. The main characters are Bilbo Baggins, Gandalf, and Thorin Oakenshield. The story follows Bilbo Baggins, a hobbit who is swept into an epic quest to reclaim the Lonely Mountain and its treasure from the dragon Smaug."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Lord of the Rings",
                Reference = "978-0-395-43723-8",
                Gendre = "Fantasy",
                AuthorId = _authors[2].Id,
                Author = _authors[2],
                PublishedOn = new DateTime(1954, 7, 29),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English,Spanish",
                Price = 19.99,
                NumberOfPages = 1178,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "One of the greatest fantasy novels of all time!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "An epic fantasy trilogy that follows the journey of the One Ring. The main characters are Frodo Baggins, Aragorn, and Gandalf. The story depicts the struggle against the dark lord Sauron and the quest to destroy the One Ring and save Middle-earth."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "And Then There Were None",
                Reference = "978-0-123-45678-9",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1939, 11, 6),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 10.99,
                NumberOfPages = 272,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Exciting read!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "A gripping mystery novel by Agatha Christie. The main characters are Justice Lawrence Wargrave, Vera Claythorne, and Philip Lombard. The story revolves around ten strangers who are lured to a secluded island and accused of past crimes, leading to a series of murders."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = ".Net Core Conf Madrid 2024",
                Reference = "978-0-123-45678-NetCoreConf",
                Gendre = "I.T.",
                AuthorId = _authors[4].Id,
                Author = _authors[4],
                PublishedOn = new DateTime(2024, 3, 23),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 24.99,
                NumberOfPages = 365,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Amazing event!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "One of the essential events of the Microsoft community. Very interesting talks that address the different newest topics on the technological scene. With an organization that reviews each of the details to have the best experience. In session 2 in room 4 at 10:15 we can find José Alberto Coronado speaking abot Azure AI Search.\r\n"
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Silmarillion",
                Reference = "978-0-395-43723-9",
                Gendre = "Fantasy",
                AuthorId = _authors[2].Id,
                Author = _authors[2],
                PublishedOn = new DateTime(1977, 9, 15),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 365,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "A must-read for Tolkien fans!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "A collection of mythopoeic works by J.R.R. Tolkien. The Silmarillion provides the background and history of Middle-earth, including the creation of the world, the wars among the Elves, and the rise and fall of Morgoth, the first Dark Lord."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Chamber of Secrets",
                Reference = "978-0-7475-3849-0",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(1998, 7, 2),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 251,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Another magical adventure!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The second book in the Harry Potter series. Harry Potter returns to Hogwarts School of Witchcraft and Wizardry for his second year, where he encounters the mysterious Chamber of Secrets and the monster within."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Prisoner of Azkaban",
                Reference = "978-0-7475-4215-2",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(1999, 7, 8),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 317,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Thrilling and full of surprises!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The third book in the Harry Potter series. Harry Potter learns about the escaped prisoner Sirius Black and the truth about his parents' murder, while facing the Dementors and the time-traveling device called the Time-Turner."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Goblet of Fire",
                Reference = "978-0-7475-4624-2",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(2000, 7, 8),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 636,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Action-packed and intense!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The fourth book in the Harry Potter series. Harry Potter competes in the Triwizard Tournament, facing dangerous challenges and uncovering a plot involving Lord Voldemort's return."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Order of the Phoenix",
                Reference = "978-0-7475-5100-0",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(2003, 6, 21),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 870,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Epic and emotional!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The fifth book in the Harry Potter series. Harry Potter returns to Hogwarts School of Witchcraft and Wizardry, facing new challenges and the rise of Lord Voldemort's forces."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Half-Blood Prince",
                Reference = "978-0-7475-8108-4",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(2005, 7, 16),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 607,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "Intense and captivating!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The sixth book in the Harry Potter series. Harry Potter delves into the past and learns more about Lord Voldemort's history, while preparing for the ultimate battle."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Deathly Hallows",
                Reference = "978-0-545-01022-1",
                Gendre = "Fantasy",
                AuthorId = _authors[1].Id,
                Author = _authors[1],
                PublishedOn = new DateTime(2007, 7, 21),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 29.99,
                NumberOfPages = 607,
                InStock = true,
                Comments =
                [
                    new() {
                        Id = Guid.NewGuid(),
                        Text = "A satisfying conclusion!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2023, 1, 28)
                    }
                ],
                Summary = "The seventh and final book in the Harry Potter series. Harry Potter and his friends embark on a dangerous mission to destroy the remaining Horcruxes and defeat Lord Voldemort once and for all."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Murder on the Orient Express",
                Reference = "978-0-06-269366-2",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1934, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 256,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A brilliant detective story!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "A thrilling murder mystery set on the luxurious Orient Express train. Detective Hercule Poirot must solve the murder of a fellow passenger, using his keen observation and deduction skills."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Death on the Nile",
                Reference = "978-0-06-207355-6",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1937, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 352,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "Another brilliant mystery by Agatha Christie!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "A captivating murder mystery set on a luxurious cruise ship. Detective Hercule Poirot must unravel the complex web of relationships and motives to solve the murder of a young heiress."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Murder of Roger Ackroyd",
                Reference = "978-0-06-207356-3",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1926, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A brilliant and unexpected twist!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "The Murder of Roger Ackroyd is a classic detective novel featuring Hercule Poirot. When wealthy Roger Ackroyd is found dead, Poirot must unravel the secrets and motives of the suspects to solve the murder."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Murder at the Vicarage",
                Reference = "978-0-06-207358-7",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1930, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A delightful village mystery!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "Murder at the Vicarage is the first novel featuring Miss Marple. When Colonel Protheroe is found dead in the vicar's study, Miss Marple uses her sharp wit and observation skills to uncover the truth."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The ABC Murders",
                Reference = "978-0-06-207360-0",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1936, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A clever and suspenseful tale!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "The ABC Murders follows Hercule Poirot as he investigates a series of murders committed in alphabetical order. Poirot must decipher the killer's pattern and identity before they strike again."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Mysterious Affair at Styles",
                Reference = "978-0-06-207358-7",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1920, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "The first appearance of Hercule Poirot!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "The Mysterious Affair at Styles introduces the iconic detective Hercule Poirot. When Emily Inglethorp is found dead, Poirot must use his brilliant deduction skills to solve the murder in her country estate."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Five Little Pigs",
                Reference = "978-0-06-207360-0",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1942, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A compelling tale of love and betrayal!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "Five Little Pigs follows Poirot as he investigates a murder that occurred sixteen years ago. Poirot must uncover the truth behind the five suspects' alibis to solve the case."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Secret Adversary",
                Reference = "978-0-06-207355-6",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1922, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 352,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "An exciting espionage adventure!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "The Secret Adversary introduces the characters Tommy and Tuppence, who become amateur detectives. They must uncover a secret organization and prevent a political conspiracy."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crooked House",
                Reference = "978-0-06-207358-7",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1949, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A twisted and suspenseful tale!",
                        Author = "John Doe",
                        PublishedOn = new DateTime(2024, 3, 10)
                    }
                ],
                Summary = "Crooked House is a psychological thriller set in a dysfunctional family. When the patriarch is murdered, the family members become suspects, and secrets are revealed."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Body in the Library",
                Reference = "978-0-06-207360-0",
                Gendre = "Mystery",
                AuthorId = _authors[3].Id,
                Author = _authors[3],
                PublishedOn = new DateTime(1942, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 12.99,
                NumberOfPages = 288,
                InStock = true,
                Comments =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Text = "A classic whodunit!",
                        Author = "Jane Doe",
                        PublishedOn = new DateTime(2024, 3, 23)
                    }
                ],
                Summary = "The Body in the Library follows Miss Marple as she investigates the murder of a young woman found in a wealthy family's library. Miss Marple must uncover the truth behind the seemingly perfect family."
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Bad book",
                Reference = "978-0-385-12167-7",
                Gendre = "Horror",
                AuthorId = _authors.First(a => a.FirstName=="Bad").Id,
                Author = _authors.First(a => a.FirstName=="Bad"),
                PublishedOn = new DateTime(2024, 1, 1),
                LastModifiedDt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Languages = "English",
                Price = 39.99,
                NumberOfPages = 447,
                InStock = false,
                Summary = "Bad book from bad author."
            },
        ];


    public async Task SeedAsync()
    {
        var policy = ResiliencyExtensions.CreateSQLRetryPolicyAsync(_logger, nameof(Seeder));
        await policy.ExecuteAsync(async () =>
        {
            await SeedBooksAsync();
        });
    }

    private async Task SeedBooksAsync()
    {
        var updated = false;
        foreach (var bookToSeed in _books)
        {
            var existingBook = await _context.Books.SingleOrDefaultAsync(e => e.Title == bookToSeed.Title);
            if (existingBook == null)
            {
                _logger.LogInformation("Seeding book {Title}", bookToSeed.Title);
                _context.Books.Add(bookToSeed);
                updated = true;
            }
        }

        if (updated)
        {
            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Books seeding complete");
    }
}
