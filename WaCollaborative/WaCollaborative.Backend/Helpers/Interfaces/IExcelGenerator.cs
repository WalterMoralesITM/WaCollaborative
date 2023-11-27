using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Helpers.Interfaces
{
    public interface IExcelGenerator
    {
        Task<(string filePath, string fileName)> GenerateExcelFileAsync(List<CollaborativeDemandDTO> collaborativeDemandDTOs);
    }
}
