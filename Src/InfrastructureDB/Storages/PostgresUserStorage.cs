using Ardalis.Specification;
using InfrastructureDB.Specifications;
using Kurier.Common.Interfaces;
using Kurier.Common.Models.Entities;
using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;
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

        public async Task<UserAuthResponse> Auth(UserAuthRequest request)
        {
            var user = await _repository.FirstOrDefaultAsync(new ArdalisSpecificationUser(request.Email, request.Password))
                ?? throw new Exception();
            return new UserAuthResponse { UserId = user.Id, Permissions = user.Permissions };
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
