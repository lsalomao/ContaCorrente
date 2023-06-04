using Led.ContaCorrente.Domain.Enums;
using Led.ContaCorrente.Domain.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Led.ContaCorrente.Api.Controllers.Base
{

    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {
        public ActionResult HandleError<T>(Response<T> response)
        {
            ObjectResult DefaultError()
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Notificacao { DetalheErro = response.DetalheErro });
            }

            return response.MotivoErro switch
            {
                MotivoErro.Conflict => Conflict(new Notificacao { DetalheErro = response.DetalheErro }),
                MotivoErro.BadRequest => BadRequest(new Notificacao { DetalheErro = response.DetalheErro }),
                MotivoErro.NotFound => NotFound(new Notificacao { DetalheErro = response.DetalheErro }),
                MotivoErro.NoContent => NoContent(),
                MotivoErro.InternalServerError => DefaultError(),
                _ => DefaultError()
            };
        }
    }
}
