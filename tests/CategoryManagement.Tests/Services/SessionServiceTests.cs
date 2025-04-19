using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CategoryManagement.Core.Domain.Entities;
using CategoryManagement.Core.Domain.Interfaces;
using CategoryManagement.Core.DTOs;
using CategoryManagement.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CategoryManagement.Tests.Services
{
    public class SessionServiceTests
    {
        private readonly Mock<ISessionRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SessionService _service;

        public SessionServiceTests()
        {
            _mockRepository = new Mock<ISessionRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new SessionService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllSessions_ShouldReturnAllSessions()
        {
            // Arrange
            var sessions = new List<Session>
            {
                new Session { Id = 1, Title = "Session 1", Description = "Description 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) },
                new Session { Id = 2, Title = "Session 2", Description = "Description 2", StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3) }
            };
            var sessionDtos = new List<SessionDto>
            {
                new SessionDto { Id = 1, Title = "Session 1", Description = "Description 1", StartDate = sessions[0].StartDate, EndDate = sessions[0].EndDate },
                new SessionDto { Id = 2, Title = "Session 2", Description = "Description 2", StartDate = sessions[1].StartDate, EndDate = sessions[1].EndDate }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(sessions);
            _mockMapper.Setup(m => m.Map<List<SessionDto>>(sessions))
                .Returns(sessionDtos);

            // Act
            var result = await _service.GetAllSessionsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(sessionDtos);
        }

        [Fact]
        public async Task GetSessionById_WithValidId_ShouldReturnSession()
        {
            // Arrange
            var session = new Session
            {
                Id = 1,
                Title = "Test Session",
                Description = "Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            var sessionDto = new SessionDto
            {
                Id = 1,
                Title = "Test Session",
                Description = "Description",
                StartDate = session.StartDate,
                EndDate = session.EndDate
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(session);
            _mockMapper.Setup(m => m.Map<SessionDto>(session))
                .Returns(sessionDto);

            // Act
            var result = await _service.GetSessionByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sessionDto);
        }

        [Fact]
        public async Task GetSessionById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Session)null);
            _mockMapper.Setup(m => m.Map<SessionDto>(null))
                .Returns((SessionDto)null);

            // Act
            var result = await _service.GetSessionByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateSession_WithValidData_ShouldCreateSession()
        {
            // Arrange
            var sessionDto = new SessionDto
            {
                Title = "New Session",
                Description = "New Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Test Location",
                IsOnline = true
            };
            var session = new Session
            {
                Title = sessionDto.Title,
                Description = sessionDto.Description,
                StartDate = sessionDto.StartDate,
                EndDate = sessionDto.EndDate,
                Location = sessionDto.Location,
                IsOnline = sessionDto.IsOnline
            };
            var createdSessionDto = new SessionDto
            {
                Id = 1,
                Title = sessionDto.Title,
                Description = sessionDto.Description,
                StartDate = sessionDto.StartDate,
                EndDate = sessionDto.EndDate,
                Location = sessionDto.Location,
                IsOnline = sessionDto.IsOnline
            };

            _mockMapper.Setup(m => m.Map<Session>(sessionDto))
                .Returns(session);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Session>()))
                .ReturnsAsync(session);
            _mockMapper.Setup(m => m.Map<SessionDto>(session))
                .Returns(createdSessionDto);

            // Act
            var result = await _service.CreateSessionAsync(sessionDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(sessionDto.Title);
            result.Description.Should().Be(sessionDto.Description);
            result.Location.Should().Be(sessionDto.Location);
            result.IsOnline.Should().Be(sessionDto.IsOnline);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Session>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSession_WithValidData_ShouldUpdateSession()
        {
            // Arrange
            var sessionDto = new SessionDto
            {
                Id = 1,
                Title = "Updated Session",
                Description = "Updated Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Updated Location",
                IsOnline = false
            };
            var existingSession = new Session
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Description",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now,
                Location = "Old Location",
                IsOnline = true
            };
            var updatedSessionDto = new SessionDto
            {
                Id = 1,
                Title = "Updated Session",
                Description = "Updated Description",
                StartDate = sessionDto.StartDate,
                EndDate = sessionDto.EndDate,
                Location = "Updated Location",
                IsOnline = false
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingSession);
            _mockMapper.Setup(m => m.Map(sessionDto, existingSession))
                .Returns(existingSession);
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Session>()))
                .ReturnsAsync(existingSession);
            _mockMapper.Setup(m => m.Map<SessionDto>(existingSession))
                .Returns(updatedSessionDto);

            // Act
            var result = await _service.UpdateSessionAsync(1, sessionDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(sessionDto.Title);
            result.Description.Should().Be(sessionDto.Description);
            result.Location.Should().Be(sessionDto.Location);
            result.IsOnline.Should().Be(sessionDto.IsOnline);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Session>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSession_WithValidId_ShouldDeleteSession()
        {
            // Arrange
            var session = new Session { Id = 1, Title = "Test Session" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(session);
            _mockRepository.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteSessionAsync(1);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteSession_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Session)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteSessionAsync(999));
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetSessionsByCategory_WithValidCategoryId_ShouldReturnMatchingSessions()
        {
            // Arrange
            var categoryId = 1;
            var sessions = new List<Session>
            {
                new Session { Id = 1, Title = "Session 1" },
                new Session { Id = 2, Title = "Session 2" }
            };
            var sessionDtos = new List<SessionDto>
            {
                new SessionDto { Id = 1, Title = "Session 1" },
                new SessionDto { Id = 2, Title = "Session 2" }
            };

            _mockRepository.Setup(repo => repo.GetSessionsByCategoryAsync(categoryId))
                .ReturnsAsync(sessions);
            _mockMapper.Setup(m => m.Map<List<SessionDto>>(sessions))
                .Returns(sessionDtos);

            // Act
            var result = await _service.GetSessionsByCategoryAsync(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(sessionDtos);
        }
    }
}
