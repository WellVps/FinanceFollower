namespace JobsService.Services.Interfaces;

public interface IUpdateAssetsService
{
    Task UpdateAssets(bool makeSnapshot = false);
}