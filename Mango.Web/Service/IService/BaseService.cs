using Mango.Web.Models;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Service.IService
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenProvider tokenprovider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenprovider) 
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenprovider = tokenprovider;
        }
        public async Task<ResponseDTO?> SendAsync(RequestDto requesDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                if(withBearer) 
                {
                    var token = tokenprovider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token} ");
                }
                message.RequestUri = new Uri(requesDto.Url);
                if (requesDto != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requesDto), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage responseMessage;

                switch (requesDto.ApiType)
                {
                    case Utility.SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utility.SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Utility.SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }
                responseMessage = await client.SendAsync(message);

                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await responseMessage.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                        //var apiResponseDto = responseMessage.Content.ReadFromJsonAsync<ResponseDTO>();
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDTO
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
                return dto;
            }
        }
    }
}
