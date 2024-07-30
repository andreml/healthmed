using HealthMed.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HealthMed.Api.Controller
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected new IActionResult Response<T>(ResponseBase<T> result)
        {
            if (result.HasError)
                return ErrorResponse((int)HttpStatusCode.BadRequest, result.GetErrors());

            return HttpContext.Request.Method switch
            {
                "GET" => GetResponse(result),
                "POST" => PostResponse(result),
                "PUT" or "DELETE" => PutAndDeleteResponse(result),
                _ => StatusCode(result.StatusCode, result.Data),
            };
        }

        protected IActionResult ErrorResponse(string exception, string mensagem)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, new ErrorViewModel()
            {
                Messages = new List<string> { mensagem },
                Exception = exception
            });
        }
        protected IActionResult ErrorResponse(int statusCode, List<string> mensagens)
        {
            if (statusCode == 0) statusCode = (int)HttpStatusCode.BadRequest;

            return StatusCode(statusCode, new ErrorViewModel()
            {
                Messages = mensagens
            });
        }

        protected new virtual IActionResult Response(ResponseBase result)
        {
            return Response((ResponseBase<object>)result);
        }

        private IActionResult GetResponse<T>(ResponseBase<T> result)
        {
            if (result.Data == null)
                return NoContent();

            return Ok(result.Data);
        }

        private IActionResult PostResponse<T>(ResponseBase<T> result)
        {
            var status = result.StatusCode == 0 ? HttpStatusCode.Created : ((HttpStatusCode)result.StatusCode);

            return StatusCode((int)status, result.Data);
        }

        private IActionResult PutAndDeleteResponse<T>(ResponseBase<T> result)
        {
            var status = result.StatusCode == 0 ? HttpStatusCode.OK : ((HttpStatusCode)result.StatusCode);

            return StatusCode((int)status, result.Data);
        }

        protected Guid GetLoggedUserId() =>
            Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")!.Value);
    }
}
