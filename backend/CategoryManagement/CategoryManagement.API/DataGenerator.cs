public static class DataGenerator
{
    public static void GenerateTestData(IServiceProvider serviceProvider, int sessionCount = 100000)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if data already exists
            if (context.Sessions.Any())
            {
                Console.WriteLine("Database already contains sessions. Skipping test data generation.");
                return;
            }

            Console.WriteLine($"Generating {sessionCount} test sessions...");

            // Generate tags if they don't exist
            if (!context.Tags.Any())
            {
                GenerateTags(context);
            }

            // Generate speakers if they don't exist
            if (!context.Speakers.Any())
            {
                GenerateSpeakers(context);
            }

            // Get existing tags and speakers
            var allTags = context.Tags.ToList();
            var allSpeakers = context.Speakers.ToList();

            // Locations and other data for random selection
            var locations = new[] { "Online", "New York", "San Francisco", "Seattle", "Chicago", "Austin", "Boston", "London", "Berlin", "Tokyo", "Sydney" };
            var random = new Random();

            // Generate sessions in batches to improve performance
            const int batchSize = 1000;
            for (int i = 0; i < sessionCount; i += batchSize)
            {
                var sessionsToAdd = new List<Session>();

                for (int j = 0; j < Math.Min(batchSize, sessionCount - i); j++)
                {
                    var startDate = DateTime.UtcNow.AddDays(random.Next(-180, 180));
                    var durationHours = random.Next(1, 8);

                    var session = new Session
                    {
                        Title = $"Session {i + j + 1}: {GetRandomSessionTitle(random)}",
                        Description = GetRandomDescription(random),
                        StartDate = startDate,
                        EndDate = startDate.AddHours(durationHours),
                        Location = locations[random.Next(locations.Length)],
                        IsOnline = random.Next(2) == 0, // 50% chance of being online
                        SessionTags = new List<SessionTag>(),
                        SessionSpeakers = new List<SessionSpeaker>()
                    };

                    // Add 1-5 random tags
                    var tagCount = random.Next(1, 6);
                    var selectedTagIndices = new HashSet<int>();
                    while (selectedTagIndices.Count < tagCount)
                    {
                        selectedTagIndices.Add(random.Next(allTags.Count));
                    }

                    foreach (var tagIndex in selectedTagIndices)
                    {
                        session.SessionTags.Add(new SessionTag
                        {
                            Session = session,
                            Tag = allTags[tagIndex]
                        });
                    }

                    // Add 1-3 random speakers
                    var speakerCount = random.Next(1, 4);
                    var selectedSpeakerIndices = new HashSet<int>();
                    while (selectedSpeakerIndices.Count < speakerCount)
                    {
                        selectedSpeakerIndices.Add(random.Next(allSpeakers.Count));
                    }

                    foreach (var speakerIndex in selectedSpeakerIndices)
                    {
                        session.SessionSpeakers.Add(new SessionSpeaker
                        {
                            Session = session,
                            Speaker = allSpeakers[speakerIndex]
                        });
                    }

                    sessionsToAdd.Add(session);
                }

                context.Sessions.AddRange(sessionsToAdd);
                context.SaveChanges();

                Console.WriteLine($"Added {Math.Min(batchSize, sessionCount - i)} sessions. Progress: {i + batchSize}/{sessionCount}");
            }

            Console.WriteLine("Test data generation completed.");
        }
    }

    private static void GenerateTags(ApplicationDbContext context)
    {
        var tags = new List<Tag>
        {
            new Tag { Name = "aws" },
            new Tag { Name = "azure" },
            new Tag { Name = "gcp" },
            new Tag { Name = "kubernetes" },
            new Tag { Name = "docker" },
            new Tag { Name = "beginner" },
            new Tag { Name = "intermediate" },
            new Tag { Name = "advanced" },
            new Tag { Name = "web" },
            new Tag { Name = "mobile" },
            new Tag { Name = "cloud" },
            new Tag { Name = "security" },
            new Tag { Name = "database" },
            new Tag { Name = "frontend" },
            new Tag { Name = "backend" },
            new Tag { Name = "devops" },
            new Tag { Name = "machine-learning" },
            new Tag { Name = "ai" },
            new Tag { Name = "blockchain" },
            new Tag { Name = "iot" },
            new Tag { Name = "networking" },
            new Tag { Name = "architecture" },
            new Tag { Name = "performance" },
            new Tag { Name = "testing" },
            new Tag { Name = "agile" },
            new Tag { Name = "microservices" },
            new Tag { Name = "serverless" },
            new Tag { Name = "javascript" },
            new Tag { Name = "python" },
            new Tag { Name = "csharp" },
            new Tag { Name = "java" },
            new Tag { Name = "react" },
            new Tag { Name = "angular" },
            new Tag { Name = "vue" },
            new Tag { Name = "dotnet" }
        };

        context.Tags.AddRange(tags);
        context.SaveChanges();

        Console.WriteLine($"Added {tags.Count} tags");
    }

    private static void GenerateSpeakers(ApplicationDbContext context)
    {
        var firstNames = new[] { "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth",
            "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen", "Daniel", "Lisa",
            "Matthew", "Nancy", "Anthony", "Margaret", "Mark", "Betty", "Donald", "Sandra", "Steven", "Ashley", "Andrew", "Dorothy",
            "Paul", "Kimberly", "Joshua", "Emily", "Kenneth", "Donna" };

        var lastNames = new[] { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
            "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark",
            "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott",
            "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter" };

        var expertise = new[] { "Cloud Architecture", "DevOps Engineering", "Web Development", "Mobile Development", "Database Administration",
            "Security", "Machine Learning", "Artificial Intelligence", "Network Engineering", "Software Architecture", "Quality Assurance",
            "Agile Coaching", "Frontend Development", "Backend Development", "IoT Development", "Blockchain", "Data Science",
            "UI/UX Design", "Product Management", "System Administration" };

        var random = new Random();
        var speakers = new List<Speaker>();

        for (int i = 0; i < 50; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var speakerExpertise = expertise[random.Next(expertise.Length)];

            speakers.Add(new Speaker
            {
                Name = $"{firstName} {lastName}",
                Bio = $"{speakerExpertise} professional with {random.Next(2, 20)} years of industry experience.",
                ProfileImage = "" // Empty string to satisfy NOT NULL constraint
            });
        }

        context.Speakers.AddRange(speakers);
        context.SaveChanges();

        Console.WriteLine($"Added {speakers.Count} speakers");
    }

    private static string GetRandomSessionTitle(Random random)
    {
        var titlePrefixes = new[] { "Introduction to", "Advanced", "Mastering", "Deep Dive into", "Building with", "The Future of",
            "Understanding", "Exploring", "Practical", "Enterprise" };

        var titleSubjects = new[] { "Azure", "AWS", "Kubernetes", "Docker", "Microservices", "DevOps", "Cloud Security", "Serverless Computing",
            "React", "Vue.js", "Angular", ".NET Core", "Python", "Machine Learning", "Artificial Intelligence", "Blockchain", "IoT",
            "Database Performance", "Web Development", "Mobile Development", "API Design", "Test Automation", "CI/CD Pipelines",
            "Agile Methodologies", "Data Science" };

        return $"{titlePrefixes[random.Next(titlePrefixes.Length)]} {titleSubjects[random.Next(titleSubjects.Length)]}";
    }

    private static string GetRandomDescription(Random random)
    {
        var descriptions = new[]
        {
            "Learn the fundamentals and best practices that will help you build scalable and reliable solutions.",
            "Deep dive into advanced techniques that will take your skills to the next level.",
            "A comprehensive overview of key concepts, tools, and methodologies.",
            "Hands-on workshop designed to give you practical experience with real-world scenarios.",
            "Explore cutting-edge technologies and how they're shaping the future of the industry.",
            "Join us for this technical session where we'll cover architecture patterns and implementation details.",
            "This session will provide you with the knowledge to solve complex problems and optimize performance.",
            "From beginner to expert: everything you need to know to get started and advance your career.",
            "Learn from industry experts about the latest trends, challenges, and opportunities.",
            "A case study approach to understanding how these technologies are used in production environments."
        };

        return descriptions[random.Next(descriptions.Length)];
    }
}
