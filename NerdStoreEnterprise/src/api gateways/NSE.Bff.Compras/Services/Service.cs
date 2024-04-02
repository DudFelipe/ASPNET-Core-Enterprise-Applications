using NSE.Core.Communication;
using System.Net;
using System.Text;
using System.Text.Json;

namespace NSE.Bff.Compras.Services
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            response.EnsureSuccessStatusCode(); //Garantir que seja um retorno de sucesso como código 200 ou 201 por exemplo
            return true;
        }

        protected ResponseResult RetornoOk()
        {
            return new ResponseResult();
        }
    }
}
