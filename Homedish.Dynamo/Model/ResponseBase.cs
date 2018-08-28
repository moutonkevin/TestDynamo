using System.Net;

namespace Homedish.Aws.Dynamo.Model
{
    public class ResponseBase
    {
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorCode) &&
                                 string.IsNullOrWhiteSpace(ErrorMessage) &&
                                 HttpStatusCode == HttpStatusCode.OK;

        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    }
}