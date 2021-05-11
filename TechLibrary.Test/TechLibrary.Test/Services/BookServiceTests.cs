using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLibrary.Contracts.Requests;
using TechLibrary.Domain;
using TechLibrary.Repositories;
using TechLibrary.Services;

namespace TechLibrary.Test.Services
{
    [TestFixture()]
    [Category("BookServiceTests")]
    public class BookServiceTests
    {
        private Mock<ILogger<BookService>> _mockLogger;
        private Mock<IBookRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;

        [OneTimeSetUp]
        public void TestSetup()
        {   
            _mockLogger = new Mock<ILogger<BookService>>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IBookRepository>();
        }

        [Test()]
        public async Task SearchBooksByTitle()
        {
            // Arrange
            var service = new BookService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            _mockRepository.Setup(x => x.GetBooksByTitle(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<List<Book>>()));
            // Act
            var result = await service.SearchBooks("Android", "");
            // Assert
            _mockRepository.Verify(s => s.GetBooksByTitle("Android"), Times.Once, "Expected GetBooksByTitle to have been called once");
            // _mockRepository.Verify(s => s.GetBooksByDescription("Android"), Times.Never, "Expected GetBooksByDescription to have not called");
        }

        [Test()]
        public async Task SearchBooksByDescription()
        {
            // Arrange
            var service = new BookService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            _mockRepository.Setup(x => x.GetBooksByDescription(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<List<Book>>()));
            // Act
            var result = await service.SearchBooks("", "Android");
            // Assert
            _mockRepository.Verify(s => s.GetBooksByDescription(It.IsAny<string>()), Times.Once, "Expected GetBooksByDescription to have been called once");
            _mockRepository.Verify(s => s.GetBooksByTitle(It.IsAny<string>()), Times.Never, "Expected GetBooksByTitle to have not called");
        }

        [Test()]
        public async Task SearchBooksByTitleAndDescription()
        {
            // Arrange
            var service = new BookService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            _mockRepository.Setup(x => x.GetBooksByTitleAndDescription(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<IList<Book>>()));
            _mockRepository.Setup(x => x.GetBooksByDescription(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<List<Book>>()));
            // Act
            var result = await service.SearchBooks("Android", "Android");
            // Assert
            _mockRepository.Verify(s => s.GetBooksByTitleAndDescription("Android", "Android"), Times.Once, "Expected GetBooksByTitleAndDescription to have been called once");
        }

        [Test()]
        public async Task EditBook()
        {
            // Arrange
            var service = new BookService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            Book book = new Book();
            _mockRepository.Setup(x => x.GetBookByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(book));
            _mockRepository.Setup(x => x.UpdateBookAsync(It.IsAny<Book>()));

            // Act
            var request = new EditBookRequest { BookId = 1, Title = "Andriod Action", ShortDescr = "android", PublishedDate = "", ThumbnailUrl = "" };
            await service.EditBook(request);

            // Assert
            _mockRepository.Verify(x => x.GetBookByIdAsync(It.IsAny<int>()), Times.Once, "Get book call once");
            _mockRepository.Verify(x => x.UpdateBookAsync(It.IsAny<Book>()), Times.Once, "Edit book should have called once");
        }

        // TO DO: add more test cases / scenarios
    }
}
