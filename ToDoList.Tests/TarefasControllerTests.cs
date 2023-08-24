using ToDoList.Common.Exceptions;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Services.Interfaces;

namespace ToDoList.Tests
{
    public class TasksControllerTests
    {
        private readonly TasksController _controller;
        private readonly Mock<ITasksService> _serviceMock = new Mock<ITasksService>();

        public TasksControllerTests()
        {
            _controller = new TasksController(_serviceMock.Object);
        }

        [Fact]
        public void GetAll_WithValidFilter_ReturnsOk()
        {
            // Arrange
            var filter = new TasksFilterDTO();
            _serviceMock.Setup(s => s.GetAll(filter)).Returns(new List<Tasks>());

            // Act
            var result = _controller.GetAll(filter);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tasks());

            // Act
            var result = _controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns((Tasks)null);

            // Act
            var result = _controller.GetById(999); // assuming 999 is a non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Add_ValidTasks_ReturnsCreatedAtAction()
        {
            // Arrange
            var tarefaDto = new TasksInsertDTO();
            _serviceMock.Setup(s => s.Add(It.IsAny<Tasks>()));

            // Act
            var result = _controller.Add(tarefaDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Add_DuplicateTasks_ThrowsException()
        {
            // Arrange
            var tarefaDto = new TasksInsertDTO { Nome = "DuplicateName" };
            _serviceMock.Setup(s => s.Add(It.IsAny<Tasks>())).Throws(new BusinessException("Nome da tarefa já existe!"));

            // Act & Assert
            Assert.Throws<BusinessException>(() => _controller.Add(tarefaDto));
        }

        [Fact]
        public void Update_MatchingIds_ReturnsNoContent()
        {
            // Arrange
            var tarefaDto = new TasksUpdateDTO { Id = 1 };
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tasks());

            // Act
            var result = _controller.Update(1, tarefaDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_MismatchingIds_ReturnsBadRequest()
        {
            // Arrange
            var tarefaDto = new TasksUpdateDTO { Id = 2 };

            // Act
            var result = _controller.Update(1, tarefaDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Tasks());

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns((Tasks)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
