using LibraryManager.Models;

namespace LibraryManager.Utils;

/// <author>YR 2025-02-03</author>
public static class DemoBookMaker
{
    public static SimpleBookModel GenerateBook()
    {
        var id = Random.Shared.Next();
        var title = GenerateTitle();
        var author = GenerateAuthor();
        var year = GetRandomInt(1934, 2025, generatedYear);
        var pages = GetRandomInt(1, 777, generatedPages);
        return new SimpleBookModel(id, author, title, year, pages);
    }


    private static string GenerateTitle()
    {
        string title;
        do
        {
            var numTitles = random.Next(2, 4);
            var titles = new string[numTitles];

            for (var i = 0; i < numTitles - 1; i++)
            {
                titles[i] = random.Next(2) == 0 ? GetRandomElement(title1) : GetRandomElement(title2);
            }

            titles[numTitles - 1] = GetRandomElement(title3);
            title = string.Join(GetRandomElement(titleCombining), titles);
        }
        while (generatedTitles.Contains(title));

        generatedTitles.Add(title);

        if (generatedTitles.Count > totalVariations)
            generatedTitles.RemoveAt(0);

        return title;
    }

    private static string GenerateAuthor()
    {
        string author;
        do
        {
            var author1Part = GetRandomElement(author1);
            var author2Part = GetRandomElement(author2);

            author = random.Next(2) == 0 ? $"{author1Part} {author2Part}" : $"{author2Part} {author1Part}";
        }
        while (generatedAuthors.Contains(author));

        generatedAuthors.Add(author);

        if (generatedAuthors.Count > totalVariations)
            generatedAuthors.RemoveAt(0);

        return author;
    }

    private static T GetRandomElement<T>(T[] array)
    {
        return array[random.Next(array.Length)];
    }

    private static int GetRandomInt(int min, int max, List<int> generatedInt)
    {
        int expectedInt;
        do { expectedInt = random.Next(min, max); }
        while (generatedInt.Contains(expectedInt));

        generatedInt.Add(expectedInt);

        if (generatedInt.Count > totalVariations)
            generatedInt.RemoveAt(0);

        return expectedInt;
    }




    private static readonly string[] titleCombining = [". ", ": ", " - "];

    private static readonly string[] title1 = [
        "Against Method",
            "Total Efficiency",
            "Practical Strategies",
            "The Cooperative Game",
            "Software Writing",
            "Wild Software",
            "Debugging the Development Process",
            "Survival Guide",
            "Computing Calamities",
            "Software Craftsmanship",
            "Writing Excellent Code",
            "Computer Vision",
            "Commonsense Reasoning",
            "Computer Algebra",
            "Computer Explorations of Fractals"
        ];

    private static readonly string[] title2 = [
        "Clean Code",
            "Code Complete",
            "Getting Real",
            "Perfect Software",
            "Software Engineering",
            "Software Estimation",
            "Agile Software Development",
            "Software Project Survival Guide",
            "Building Solid",
            "Rapid Development",
            "The Annotated Turing",
            "Data Structures",
            "he Little Schemer",
            "The Art of Computer Programming",
            "Proofs and Refutations"
    ];

    private static readonly string[] title3 = [
        "C#",
        "Java",
        "C++",
        "Python",
        "Visual Basic",
        "Visual Studio",
        "Cobol",
        "Fortran",
        "Javascript",
        "HTML",
        "Internet",
        "CSS",
        ".NET",
        "Unity",
        "Android",
        "Swift"
        ];
    private static readonly string[] author1 = [
        "McConnell",
            "Hunt",
            "Thomas",
            "McConnell",
            "Riabchenko",
            "Twain",
            "Yavorska",
            "Adams",
            "Bradbury",
            "Haddon",
            "K. Dick",
            "Lee",
            "Kundera",
            "Rankin",
            "Shevchenko"
    ];
    private static readonly string[] author2 = [
        "Steve",
            "Andrew",
            "David",
            "Steve",
            "Iurii",
            "Mark",
            "Illa",
            "Douglas",
            "Ray",
            "Mark",
            "Philip",
            "Harper",
            "Milan",
            "Robert",
            "Taras"
    ];


    private const int totalVariations = 50;

    private static readonly Random random = new();
    private static readonly List<string> generatedAuthors = new();
    private static readonly List<string> generatedTitles = new();
    private static readonly List<int> generatedYear = new();
    private static readonly List<int> generatedPages = new();
}
