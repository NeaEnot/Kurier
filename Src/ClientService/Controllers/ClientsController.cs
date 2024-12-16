using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.ClientService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientsController : ControllerBase
    {
        public ClientsController()
        {

        }

        [HttpPost]
        public async Task<Guid> Register([FromBody] UserRequest request)
        {
            // STUB
            // Записываем в основную базу

            return Guid.NewGuid();
        }

        [HttpPost]
        public async Task<Guid> Auth([FromBody] UserRequest request)
        {
            // STUB
            // Получаем юзера по логину-паролю из основной базы
            // client = db.GetClient(request.Login, request.Password)

            UserAuthToken token = new UserAuthToken
            {
                TokenId = Guid.NewGuid(),
                //UserId = client.ClientId,
                EndTime = DateTime.Now.AddMinutes(20)
            };

            // Записываем token в redis

            return token.TokenId;
        }

        [HttpGet]
        public async Task<NotificationsList> GetNotifications([FromBody] Guid clientId)
        {
            // STUB
            // Получаем список уведомлений из Redis
            // Удаляем список уведомлений из Redis

            return null;
        }
    }
}
