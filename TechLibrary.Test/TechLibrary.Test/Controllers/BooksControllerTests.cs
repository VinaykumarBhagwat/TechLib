using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLibrary.Contracts.Requests;
using TechLibrary.Models;
using TechLibrary.Services;

namespace TechLibrary.Controllers.Tests
{
    [TestFixture()]
    [Category("ControllerTests")]
    public class BooksControllerTests
    {

        private  Mock<ILogger<BooksController>> _mockLogger;
        private  Mock<IBookService> _mockBookService;
        private  Mock<IMapper> _mockMapper;
        private NullReferenceException _expectedException;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _expectedException = new NullReferenceException("Test Failed...");
            _mockLogger = new Mock<ILogger<BooksController>>();
            _mockBookService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Test()]
        public async Task GetAllTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBooksAsync()).Returns(Task.FromResult(It.IsAny<List<BookResponse>>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object);

            //  Act
            var result = await sut.GetAll();

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");
        }

        [Test()]
        public async Task GetPaginatedTest()
        {
            // Arrange
            _mockBookService.Setup(b => b.GetBooksAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<List<BookResponse>>()));
            var controller = new BooksController(_mockLogger.Object, _mockBookService.Object);
            
            // Act
            GetAllBookRequest request = new GetAllBookRequest() { PageNumber = 1 };
            var result = await controller.GetPaginated(request);

            // Assert
            _mockBookService.Verify(b => b.GetBooksAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once, "Expected GetBooksAsync with pagination to have called once");
        }

        [Test()]
        public async Task GetBookById()
        {
            // Arrange
            _mockBookService.Setup(b => b.GetBookByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<BookResponse>()));
            var controller = new BooksController(_mockLogger.Object, _mockBookService.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            _mockBookService.Verify(b => b.GetBookByIdAsync(It.IsAny<int>()), Times.Once, "Expected GetBookByIdAsync to have called once");
        }

        [Test()]
        public async Task EditBook()
        {
            // Arrange
            var request = new EditBookRequest { BookId = 1, Title = "Andriod Action", ShortDescr = "android", PublishedDate = "", ThumbnailUrl = "" };
            _mockBookService.Setup(b => b.EditBook(It.IsAny<EditBookRequest>())).Returns(Task.FromResult(It.IsAny<Task>()));
            var controller = new BooksController(_mockLogger.Object, _mockBookService.Object);

            // Act
            var result = await controller.Edit(request);

            // Assert
            _mockBookService.Verify(b => b.EditBook(It.IsAny<EditBookRequest>()), Times.Once, "Expected EditBook to have called once");
        }

        // TO DO: add more test cases for negative test scenarios
        [Test]
        public async Task EditBook_EmptyBookTitle()
        {   
        }

        [Test]
        public async Task EditBook_InvalidBookId()
        {
        }

    }
}