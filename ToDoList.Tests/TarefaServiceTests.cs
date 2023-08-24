using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Services.Implementations;
using ToDoList.Repository.Interfaces;
using ToDoList.Infrastructure.Common.Exceptions;

namespace ToDoList.Tests
{


    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _mockRepo;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _mockRepo = new Mock<ITarefaRepository>();
            _service = new TarefaService(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_WithFilter_ReturnsExpectedTarefas()
        {
            // Arrange
            var testFilter = new TarefaFilterDTO();
            var testData = new List<Tarefa>
        {
            new Tarefa { Id = 1, Nome = "Test1" },
            new Tarefa { Id = 2, Nome = "Test2" },
        };

            _mockRepo.Setup(repo => repo.GetAll(testFilter)).Returns(testData);

            // Act
            var result = _service.GetAll(testFilter);

            // Assert
            Assert.Equal(testData, result);
        }

        [Fact]
        public void GetById_ValidId_ReturnsExpectedTarefa()
        {
            // Arrange
            var testTarefa = new Tarefa { Id = 1, Nome = "Test Tarefa" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(testTarefa);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Equal(testTarefa, result);
        }

        [Fact]
        public void Add_ExistingName_ThrowsBusinessException()
        {
            // Arrange
            var testTarefa = new Tarefa { Id = 1, Nome = "Test1" };
            _mockRepo.Setup(repo => repo.GetByName(testTarefa.Nome)).Returns(testTarefa);

            // Act & Assert
            Assert.Throws<BusinessException>(() => _service.Add(testTarefa));
        }

        [Fact]
        public void Add_NewName_AddsSuccessfully()
        {
            // Arrange
            var testTarefa = new Tarefa { Id = 1, Nome = "Test1" };
            _mockRepo.Setup(repo => repo.GetByName(testTarefa.Nome)).Returns((Tarefa)null);
            _mockRepo.Setup(repo => repo.GetMaxOrdemApresentacao()).Returns(0);

            // Act
            _service.Add(testTarefa);

            // Assert
            _mockRepo.Verify(repo => repo.Add(testTarefa), Times.Once);
        }

        [Fact]
        public void Update_ExistingNameDifferentId_ThrowsBusinessException()
        {
            // Arrange
            var testTarefa = new Tarefa { Id = 2, Nome = "Test1" };
            var existingTarefa = new Tarefa { Id = 1, Nome = "Test1" };
            _mockRepo.Setup(repo => repo.GetByName(testTarefa.Nome)).Returns(existingTarefa);

            // Act & Assert
            Assert.Throws<BusinessException>(() => _service.Update(testTarefa));
        }

        [Fact]
        public void Update_SameOrderDifferentId_ShiftsOrder()
        {
            // Arrange
            var testTarefa = new Tarefa { Id = 2, Nome = "Test1", OrdemApresentacao = 1 };
            var existingTarefa = new Tarefa { Id = 1, Nome = "Test2", OrdemApresentacao = 1 };
            _mockRepo.Setup(repo => repo.GetByName(testTarefa.Nome)).Returns((Tarefa)null);
            _mockRepo.Setup(repo => repo.GetByOrder(1)).Returns(existingTarefa);

            // Act
            _service.Update(testTarefa);

            // Assert
            _mockRepo.Verify(repo => repo.ShiftOrderFrom(1), Times.Once);
        }

        [Fact]
        public void Delete_ValidId_DeletesSuccessfully()
        {
            // Act
            _service.Delete(1);

            // Assert
            _mockRepo.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public void GetMaxOrdemApresentacao_ReturnsExpectedValue()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetMaxOrdemApresentacao()).Returns(5);

            // Act
            var result = _service.GetMaxOrdemApresentacao();

            // Assert
            Assert.Equal(5, result);
        }
    }

}
