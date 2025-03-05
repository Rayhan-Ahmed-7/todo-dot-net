namespace Application.Interfaces
{
    public interface IPdfGenerationService
    {
        Task<byte[]> GeneratePdfAsync(string template, object data);
    }
}
