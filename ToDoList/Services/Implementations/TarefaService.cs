﻿using ToDoList.Infrastructure.Common.Exceptions;
using ToDoList.Models.DbModels;
using ToDoList.Models.DTOs;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementations
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;

        public TarefaService(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Tarefa> GetAll(TarefaFilterDTO filter)
        {
            return _repository.GetAll(filter);
        }

        public Tarefa GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Tarefa tarefa)
        {
            var existingTask = _repository.GetByName(tarefa.Nome);

            if (existingTask != null)
            {
                throw new BusinessException("Task name already exists!");
            }

            tarefa.OrdemApresentacao = _repository.GetMaxOrdemApresentacao() + 1;

            _repository.Add(tarefa);
        }

        public Tarefa Update(Tarefa tarefa)
        {
            var existingTask = _repository.GetByName(tarefa.Nome);

            if (existingTask != null && existingTask.Id != tarefa.Id)
            {
                throw new BusinessException("Task name already exists!");
            }

            var taskWithSameOrder = _repository.GetByOrder(tarefa.OrdemApresentacao);

            if (taskWithSameOrder != null && taskWithSameOrder.Id != tarefa.Id)
            {
                _repository.ShiftOrderFrom(tarefa.OrdemApresentacao);
            }

            
            return _repository.Update(tarefa);

        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        public int GetMaxOrdemApresentacao()
        {
            return _repository.GetMaxOrdemApresentacao();
        }

    }
}
