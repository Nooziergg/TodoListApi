using ToDoList.Common.Exceptions;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Services.Interfaces;

namespace ToDoList.Tests
{
    public class TarefasControllerTests
    {
        private readonly TarefasController _controller;
        private readonly Mock<ITarefaService> _serviceMock = new Mock<ITarefaService>();

        public TarefasControllerTests()
        {
            _controller = new TarefasController(_serviceMock.Object);
        }

        [Fact]
        public void GetAll_WithValidFilter_ReturnsOk()
        {
            // Arrange
            var filter = new TarefaFilterDTO();
            _serviceMock.Setup(s => s.GetAll(filter)).Returns(new List<Tarefa>());

            // Act
            var result = _controller.GetAll(filter);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tarefa());

            // Act
            var result = _controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns((Tarefa)null);

            // Act
            var result = _controller.GetById(999); // assuming 999 is a non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Add_ValidTarefas_ReturnsCreatedAtAction()
        {
            // Arrange
            var tarefaDto = new TarefaInsertDTO();
            _serviceMock.Setup(s => s.Add(It.IsAny<Tarefa>()));

            // Act
            var result = _controller.Add(tarefaDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Add_DuplicateTarefas_ThrowsException()
        {
            // Arrange
            var tarefaDto = new TarefaInsertDTO { Nome = "DuplicateName" };
            _serviceMock.Setup(s => s.Add(It.IsAny<Tarefa>())).Throws(new BusinessException("Nome da tarefa já existe!"));

            // Act & Assert
            Assert.Throws<BusinessException>(() => _controller.Add(tarefaDto));
        }

        [Fact]
        public void Update_MatchingIds_ReturnsNoContent()
        {
            // Arrange
            var tarefaDto = new TarefaUpdateDTO { Id = 1 };
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tarefa());

            // Act
            var result = _controller.Update(1, tarefaDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_MismatchingIds_ReturnsBadRequest()
        {
            // Arrange
            var tarefaDto = new TarefaUpdateDTO { Id = 2 };

            // Act
            var result = _controller.Update(1, tarefaDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tarefa());

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns((Tarefa)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
