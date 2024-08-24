
using System.Net;

namespace PokeAPI.Modelos
{
    public class RespuestaApi
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<String> ErrorMessages { get; set; }
        public object Result { get; set; }


        public RespuestaApi()
        {
            ErrorMessages = new List<String>();
        }

    }
}
