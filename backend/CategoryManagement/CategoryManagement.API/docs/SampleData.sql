
-- Insert sample tags
INSERT INTO Tags (Name) VALUES
('aws'), ('azure'), ('gcp'), ('kubernetes'), ('docker'),
('beginner'), ('intermediate'), ('advanced'),
('web'), ('mobile'), ('cloud'), ('security'), ('database'),
('frontend'), ('backend'), ('devops'), ('machine-learning');

-- Insert sample speakers
INSERT INTO Speakers (Name, Bio, ProfileImage) VALUES
('Jane Smith', 'Cloud architect with 10+ years of experience in AWS and Azure.', ''),
('John Doe', 'Senior DevOps engineer specializing in Kubernetes and Docker.', ''),
('Alice Johnson', 'Database expert with focus on performance optimization.', ''),
('Bob Wilson', 'Web developer and frontend specialist.', ''),
('Emily Davis', 'Security consultant with expertise in cloud security.', '');

-- Insert sample sessions
INSERT INTO Sessions (Title, Description, StartDate, EndDate, Location, IsOnline) VALUES
('Introduction to AWS', 'Learn the basics of AWS cloud services.', '2025-05-10T10:00:00', '2025-05-10T12:00:00', 'Online', 1),
('Advanced Azure Architecture', 'Deep dive into Azure architecture patterns.', '2025-05-15T14:00:00', '2025-05-15T17:00:00', 'Online', 1),
('Kubernetes for Beginners', 'Introduction to container orchestration with Kubernetes.', '2025-05-20T09:00:00', '2025-05-20T12:00:00', 'Seattle', 0),
('Cloud Security Best Practices', 'Learn how to secure your cloud infrastructure.', '2025-05-25T13:00:00', '2025-05-25T16:00:00', 'Online', 1),
('Database Performance Tuning', 'Advanced techniques for optimizing database performance.', '2025-05-12T10:00:00', '2025-05-12T17:00:00', 'Chicago', 0),
('DevOps Pipeline Automation', 'Build efficient CI/CD pipelines for your projects.', '2025-05-18T09:00:00', '2025-05-18T18:00:00', 'San Francisco', 0),
('Machine Learning on AWS', 'Introduction to using AWS for machine learning workloads.', '2025-05-22T14:00:00', '2025-05-22T16:00:00', 'Online', 1),
('Frontend Framework Comparison', 'Compare popular frontend frameworks like React, Vue, and Angular.', '2025-05-05T11:00:00', '2025-05-05T12:30:00', 'Online', 1),
('Microservices with .NET Core', 'Building scalable microservices with .NET Core.', '2025-05-08T09:00:00', '2025-05-09T17:00:00', 'New York', 0),
('Docker for Developers', 'Learn Docker fundamentals for development environments.', '2025-05-30T10:00:00', '2025-05-30T15:00:00', 'Boston', 0);

-- Link sessions with speakers
INSERT INTO SessionSpeakers (SessionId, SpeakerId) VALUES
(1, 1), -- Jane Smith for AWS Intro
(2, 1), -- Jane Smith for Azure Advanced
(3, 2), -- John Doe for Kubernetes
(4, 5), -- Emily Davis for Security
(5, 3), -- Alice Johnson for Database
(6, 2), -- John Doe for DevOps
(7, 1), -- Jane Smith for ML on AWS
(8, 4), -- Bob Wilson for Frontend
(9, 4), -- Bob Wilson for Microservices
(10, 2); -- John Doe for Docker

-- Link sessions with tags
INSERT INTO SessionTags (SessionId, TagId) VALUES
-- AWS Intro
(1, 1), -- aws
(1, 6), -- beginner
(1, 11), -- cloud

-- Azure Advanced
(2, 2), -- azure
(2, 8), -- advanced
(2, 11), -- cloud

-- Kubernetes Beginners
(3, 4), -- kubernetes
(3, 6), -- beginner
(3, 16), -- devops

-- Cloud Security
(4, 11), -- cloud
(4, 12), -- security
(4, 7), -- intermediate

-- Database Performance
(5, 13), -- database
(5, 8), -- advanced

-- DevOps Pipeline
(6, 16), -- devops
(6, 7), -- intermediate

-- ML on AWS
(7, 1), -- aws
(7, 17), -- machine-learning
(7, 7), -- intermediate

-- Frontend Comparison
(8, 14), -- frontend
(8, 9), -- web
(8, 7), -- intermediate

-- Microservices with .NET
(9, 15), -- backend
(9, 7), -- intermediate

-- Docker for Devs
(10, 5), -- docker
(10, 6), -- beginner
(10, 16); -- devops

-- Sample categories
-- Sample categories with CreatedAt values
INSERT INTO Categories (Name, Description, CreatedAt) VALUES
('Cloud Fundamentals', 'Introductory sessions about cloud computing platforms', GETUTCDATE()),
('Advanced DevOps', 'Advanced sessions focused on DevOps practices and tools', GETUTCDATE()),
('May Web Development', 'Web development sessions in May 2025', GETUTCDATE()),
('Online Sessions', 'All sessions that are conducted online', GETUTCDATE());

-- Category conditions
-- Cloud Fundamentals Category
INSERT INTO CategoryConditions (CategoryId, Type, Value) VALUES
(1, 0, 'cloud'), -- Include cloud tag
(1, 0, 'aws'), -- Include aws tag
(1, 0, 'azure'), -- Include azure tag
(1, 1, 'advanced'), -- Exclude advanced tag
(1, 2, 'Online'); -- Location: Online

-- Advanced DevOps Category
INSERT INTO CategoryConditions (CategoryId, Type, Value) VALUES
(2, 0, 'devops'), -- Include devops tag
(2, 1, 'beginner'), -- Exclude beginner tag
(2, 0, 'kubernetes'); -- Include kubernetes tag

-- May Web Development Category
INSERT INTO CategoryConditions (CategoryId, Type, Value) VALUES
(3, 0, 'web'), -- Include web tag
(3, 3, '2025-05-01'), -- Start date minimum
(3, 4, '2025-05-31'); -- Start date maximum

-- Online Sessions Category
INSERT INTO CategoryConditions (CategoryId, Type, Value) VALUES
(4, 2, 'Online'); -- Location: Online
