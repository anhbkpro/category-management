using CategoryManagement.Core.Domain.Entities;
using CategoryManagement.Infrastructure.Persistence;

public static class DataGenerator
{
    public static void GenerateTestData(IServiceProvider serviceProvider, int sessionCount = 1000)
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

            // Generate categories if they don't exist
            if (!context.Categories.Any())
            {
                GenerateCategories(context);
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
                    var now = DateTime.UtcNow;

                    var session = new Session
                    {
                        Title = $"Session {i + j + 1}: {GetRandomSessionTitle(random)}",
                        Description = GetRandomDescription(random),
                        StartDate = startDate,
                        EndDate = startDate.AddHours(durationHours),
                        Location = locations[random.Next(locations.Length)],
                        IsOnline = random.Next(2) == 0, // 50% chance of being online
                        Status = "Scheduled",
                        CreatedAt = now,
                        UpdatedAt = now,
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
                            Speaker = allSpeakers[speakerIndex],
                            CreatedAt = now,
                            UpdatedAt = now
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
        var now = DateTime.UtcNow;
        var tags = new List<Tag>
        {
            new Tag { Name = "aws", Description = "Amazon Web Services", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "azure", Description = "Microsoft Azure", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "gcp", Description = "Google Cloud Platform", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "kubernetes", Description = "Container Orchestration", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "docker", Description = "Container Platform", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "beginner", Description = "Beginner Level", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "intermediate", Description = "Intermediate Level", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "advanced", Description = "Advanced Level", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "web", Description = "Web Development", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "mobile", Description = "Mobile Development", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "cloud", Description = "Cloud Computing", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "security", Description = "Security", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "database", Description = "Database", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "frontend", Description = "Frontend Development", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "backend", Description = "Backend Development", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "devops", Description = "DevOps", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "machine-learning", Description = "Machine Learning", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "ai", Description = "Artificial Intelligence", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "blockchain", Description = "Blockchain", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "iot", Description = "Internet of Things", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "networking", Description = "Networking", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "architecture", Description = "Software Architecture", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "performance", Description = "Performance Optimization", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "testing", Description = "Testing", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "agile", Description = "Agile Methodology", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "microservices", Description = "Microservices", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "serverless", Description = "Serverless Computing", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "javascript", Description = "JavaScript", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "python", Description = "Python", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "csharp", Description = "C#", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "java", Description = "Java", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "react", Description = "React", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "angular", Description = "Angular", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "vue", Description = "Vue.js", CreatedAt = now, UpdatedAt = now },
            new Tag { Name = "dotnet", Description = ".NET", CreatedAt = now, UpdatedAt = now }
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
        var now = DateTime.UtcNow;

        for (int i = 0; i < 50; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var speakerExpertise = expertise[random.Next(expertise.Length)];

            speakers.Add(new Speaker
            {
                Name = $"{firstName} {lastName}",
                Bio = $"{speakerExpertise} professional with {random.Next(2, 20)} years of industry experience.",
                ProfileImage = "", // Empty string to satisfy NOT NULL constraint
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        context.Speakers.AddRange(speakers);
        context.SaveChanges();

        Console.WriteLine($"Added {speakers.Count} speakers");
    }

    private static void GenerateCategories(ApplicationDbContext context)
    {
        var now = DateTime.UtcNow;
        var categories = new List<Category>
        {
            new Category
            {
                Name = "Cloud Computing",
                Description = "Sessions focused on cloud platforms and services",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "aws", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "azure", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "gcp", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "cloud", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Container Technologies",
                Description = "Sessions about containerization and orchestration",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "docker", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "kubernetes", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Web Development",
                Description = "Sessions covering web development technologies",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "web", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "frontend", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "backend", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Advanced Topics",
                Description = "Sessions for experienced developers",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "advanced", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.ExcludeTag, Value = "beginner", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Beginner Friendly",
                Description = "Sessions suitable for beginners",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.IncludeTag, Value = "beginner", CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.ExcludeTag, Value = "advanced", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Online Sessions",
                Description = "All online sessions",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.Location, Value = "Online", CreatedAt = now, UpdatedAt = now }
                }
            },
            new Category
            {
                Name = "Upcoming Sessions",
                Description = "Sessions scheduled for the next 30 days",
                CreatedAt = now,
                UpdatedAt = now,
                Conditions = new List<CategoryCondition>
                {
                    new CategoryCondition { Type = ConditionType.StartDateMin, Value = DateTime.UtcNow.ToString("O"), CreatedAt = now, UpdatedAt = now },
                    new CategoryCondition { Type = ConditionType.StartDateMax, Value = DateTime.UtcNow.AddDays(30).ToString("O"), CreatedAt = now, UpdatedAt = now }
                }
            }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        Console.WriteLine($"Added {categories.Count} categories with conditions");
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
