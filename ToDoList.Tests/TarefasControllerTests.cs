using ToDoList.Infrastructure.Common.Exceptions;
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

        #region Listar Tarefas

        [Fact]
        public void GetAll_WithoutFilter_ReturnsAllTarefasInOrder()
        {
            // Arrange
            var tarefas = new List<Tarefa>
             {
                 new Tarefa { Id = 1, OrdemApresentacao = 1, Nome = "Teste 1", Custo = 50 },
                 new Tarefa { Id = 2, OrdemApresentacao = 2, Nome = "Teste 2", Custo = 1500 } // High cost
             };

            var filterDto = new TarefaFilterDTO { Paginate = false };
            _serviceMock.Setup(s => s.GetAll(filterDto)).Returns(tarefas);

            // Act
            var result = _controller.GetAll(filterDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedTarefas = Assert.IsAssignableFrom<IEnumerable<Tarefa>>(actionResult.Value);
            Assert.Equal(tarefas.Count, returnedTarefas.Count());
        }

        [Fact]
        public void GetAll_WithHighCostFilter_ReturnsTarefasWithHighCost()
        {
            var filter = new TarefaFilterDTO { CustoMin = 1000 };
            var tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 2, OrdemApresentacao = 2, Nome = "Teste 2", Custo = 1500 } // Only the high-cost task
            };
            _serviceMock.Setup(s => s.GetAll(filter)).Returns(tarefas);

            // Act
            var result = _controller.GetAll(filter);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedTarefas = Assert.IsAssignableFrom<IEnumerable<Tarefa>>(actionResult.Value);
            Assert.Single(returnedTarefas);
        }

        [Fact]
        public void GetAll_InvalidDateInterval_ReturnsBadRequest()
        {
            // Arrange
            var filterDto = new TarefaFilterDTO
            {
                DataLimiteMin = DateTime.Now.AddDays(5),
                DataLimiteMax = DateTime.Now.AddDays(2)
            };

            // Act
            var result = _controller.GetAll(filterDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date interval. The start date must be equal or before the end date.", badRequestResult.Value);
        }

        #endregion

        #region Excluir

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

        #endregion

        #region Editar

        [Fact]
        public void Update_MatchingIds_ReturnsUpdatedTask()
        {
            // Arrange
            int taskId = 1;
            var tarefaDto = new TarefaUpdateDTO
            {
                Id = taskId,
                Nome = "UpdatedName",
                Custo = 100,
                OrdemApresentacao = 1
            };

            var existingTarefa = new Tarefa
            {
                Id = taskId,
                Nome = "OldName",
                Custo = 50,
                OrdemApresentacao = 1
            };

            _serviceMock.Setup(s => s.GetById(taskId)).Returns(existingTarefa);
            _serviceMock.Setup(s => s.Update(It.IsAny<Tarefa>())).Returns(existingTarefa);
            // Act
            var result = _controller.Update(taskId, tarefaDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTarefa = Assert.IsType<Tarefa>(okResult.Value);
            Assert.Equal(tarefaDto.Nome, returnedTarefa.Nome); // Verify the change
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
        public void Update_DuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ITarefaService>();

            // Simulando que uma tarefa com ID 1 existe.
            mockService.Setup(s => s.GetById(1)).Returns(new Tarefa { Id = 1 });

            // Configurando o mock para lançar BusinessException ao tentar atualizar uma tarefa.
            mockService.Setup(s => s.Update(It.IsAny<Tarefa>())).Throws(new BusinessException("Task name already exists!"));

            var controller = new TarefasController(mockService.Object);

            var tarefaDto = new TarefaUpdateDTO
            {
                Id = 1,
                Nome = "NomeDuplicado",
                Custo = 100,
                DataLimite = DateTime.Now,
                OrdemApresentacao = 1
            };

            // Act
            var result = controller.Update(1, tarefaDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task name already exists!", badRequestResult.Value);
        }





        [Fact]
        public void Update_DuplicateOrdemApresentacao_AdjustsOrder()
        {
            // Arrange
            var tarefaDto = new TarefaUpdateDTO { Id = 1, OrdemApresentacao = 2 };
            var tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 2, OrdemApresentacao = 2 },
                new Tarefa { Id = 1, OrdemApresentacao = 3 } // The order was adjusted by backend
            };
            var filterDto = new TarefaFilterDTO { Paginate = false };
            _serviceMock.Setup(s => s.GetAll(filterDto)).Returns(tarefas);
       

            // Act
            _controller.Update(1, tarefaDto);
            var result = _controller.GetAll(filterDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedTarefas = Assert.IsAssignableFrom<IEnumerable<Tarefa>>(actionResult.Value);
            var editedTarefa = returnedTarefas.FirstOrDefault(t => t.Id == 1);
            Assert.Equal(3, editedTarefa.OrdemApresentacao);  // Verifies that the order was adjusted
        }

        #endregion

        #region Incluir

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
            _serviceMock.Setup(s => s.Add(It.IsAny<Tarefa>())).Throws(new BusinessException("Task name already exists!"));

            // Act & Assert
            Assert.Throws<BusinessException>(() => _controller.Add(tarefaDto));
        }

        #endregion
    }
}
