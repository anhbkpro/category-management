using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CategoryManagement.Core.Application.DTOs;
using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain.Entities;
using CategoryManagement.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CategoryManagement.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CategoryService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Test Category 1", Description = "Description 1" },
                new Category { Id = 2, Name = "Test Category 2", Description = "Description 2" }
            };
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Test Category 1", Description = "Description 1" },
                new CategoryDto { Id = 2, Name = "Test Category 2", Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);
            _mockMapper.Setup(m => m.Map<List<CategoryDto>>(categories))
                .Returns(categoryDtos);

            // Act
            var result = await _service.GetAllCategoriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(categoryDtos);
        }

        [Fact]
        public async Task GetCategoryById_WithValidId_ShouldReturnCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category", Description = "Description" };
            var categoryDto = new CategoryDto { Id = 1, Name = "Test Category", Description = "Description" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryDto>(category))
                .Returns(categoryDto);

            // Act
            var result = await _service.GetCategoryByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task GetCategoryById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Category)null);
            _mockMapper.Setup(m => m.Map<CategoryDto>(null))
                .Returns((CategoryDto)null);

            // Act
            var result = await _service.GetCategoryByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateCategory_WithValidData_ShouldCreateCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "New Category", Description = "New Description" };
            var category = new Category { Name = categoryDto.Name, Description = categoryDto.Description };
            var createdCategoryDto = new CategoryDto
                { Id = 1, Name = categoryDto.Name, Description = categoryDto.Description };

            _mockMapper.Setup(m => m.Map<Category>(categoryDto))
                .Returns(category);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryDto>(category))
                .Returns(createdCategoryDto);

            // Act
            var result = await _service.CreateCategoryAsync(categoryDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(categoryDto.Name);
            result.Description.Should().Be(categoryDto.Description);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
        }
    }
}
