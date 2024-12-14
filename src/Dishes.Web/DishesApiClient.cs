using Dishes.Common.Models;

namespace Dishes.Web;

public class DishesApiClient(HttpClient httpClient)
{
    public async Task<DishDTO[]> GetDishesAsync()
    {
        return await httpClient.GetFromJsonAsync<DishDTO[]>("/dishes") ?? [];
    }
}
