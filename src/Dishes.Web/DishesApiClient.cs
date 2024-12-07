using Dishes.Common.Models;

namespace Dishes.Web;

public class DishesApiClient(HttpClient httpClient)
{
    public async Task<DishDto[]> GetDishesAsync()
    {
        return await httpClient.GetFromJsonAsync<DishDto[]>("/dishes") ?? [];
    }
}
