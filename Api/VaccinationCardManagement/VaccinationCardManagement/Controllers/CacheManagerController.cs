using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheManagerController : ControllerBase
{
    private readonly ICacheManager _cache;
    public CacheManagerController(ICacheManager cacheManager)
    {
        _cache = cacheManager;
    }

    /// <summary>
    /// Lista todas as keys gravadas na MEMORIA
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("GetAllKeysInCache")]
    public async Task<IActionResult> GetAllKeysInCache()
    {
        List<string> response = await _cache.GetAllKeysInCache();
        return Ok(response);
    }

    /// <summary>
    /// Exclui um item do cache pela chave
    /// </summary>
    [AllowAnonymous]
    [HttpDelete($"{nameof(DeleteCacheByKey)}/{{key}}")]
    public async Task<IActionResult> DeleteCacheByKey(string key)
    {
        await _cache.ClearCacheByKeyAsync(key);
        return Ok();
    }

    /// <summary>
    /// Limpa todo o cache gravado na MEMORIA
    /// </summary>
    [AllowAnonymous]
    [HttpDelete]
    [Route("ClearCache")]
    public async Task ClearCache()
    {
        await _cache.ClearCacheAsync();
    }
}
