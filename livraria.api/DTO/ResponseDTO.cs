namespace livraria.api.DTO
{
    public class ResponseDTO
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
