namespace CRUD.Models
{
    public class ValidationModel
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public Dictionary<string, List<string>> Erros { get; set; } = [];
    }
}
