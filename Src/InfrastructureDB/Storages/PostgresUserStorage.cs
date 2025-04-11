using Ardalis.Specification;
using InfrastructureDB.Specifications;
using Kurier.Common.Interfaces;
using Kurier.Common.Models.Entities;
using Kurier.Common.Models.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDB.Storages
{
    public class PostgresUserStorage : IUserStorage
    {

        IRepository<User> _repository;

        public PostgresUserStorage(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Auth(UserAuthRequest request)
        {
            var user = await _repository.GetByIdAsync(new ArdalisSpecificationUser(request.Email, request.Password))
                ?? throw new Exception();
            return user.Id;
        }

        public Task Register(UserRegisterInStorageRequest request)
        {
            _repository.AddAsync(new User
            {
                Id = new Guid(),
                Password = request.Password,
                Email = request.Email,
                Name = request.Name,
                Permissions = request.Permissions
            });
            return Task.CompletedTask;
        }
    }
}
