using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourierController : ControllerBase
    {
        public CourierController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            // STUB
            // Записываем в основную базу

            return Ok();
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
    }
}
