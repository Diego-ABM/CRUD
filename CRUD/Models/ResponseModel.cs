namespace CRUD.Models
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public Dictionary<string, List<string>> RequestErros { get; set; } = [];
        public dynamic? Data { get; set; }

    }
}
