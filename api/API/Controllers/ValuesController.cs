using Domain.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class ValuesController : ApiController
    {
        /// <response code="200">Success</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("buscar-todos")]
        [HttpPost]
        [ResponseType(typeof(responseAPI))]
        public HttpResponseMessage GetAll([FromBody] requestAPI resquest)
        {
            if (ModelState.IsValid)
            {
                responseAPI response = _baseFriaService.GerarPdf(faturaPDFRequest);

                if (response == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        /// <response code="200">Success</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("buscar-um")]
        [HttpPost]
        [ResponseType(typeof(responseAPI))]
        public HttpResponseMessage  SingleGet([FromBody] requestAPI resquest)
        {
            return "value";
        }

  
        /// <response code="200">Success</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("cadastrar")]
        [HttpPost]
        [ResponseType(typeof(responseAPI))]
        public void Post([FromBody] string value)
        {
        }

    }
}
