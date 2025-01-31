namespace TestBackEnd.Domain.DTOs
{
    public class ResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }


        public ResponseDTO(int statusCode, string message = "", object data = null)
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Data = data;
        }
    }
}
