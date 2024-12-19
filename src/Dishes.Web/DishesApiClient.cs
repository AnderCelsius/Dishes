using Dishes.Common.Models;

namespace Dishes.Web;

public class DishesApiClient(HttpClient httpClient)
{
    public async Task<List<DishListDTO>> GetDishesAsync()
    {
        var result = await httpClient.GetFromJsonAsync<PagedResponse<DishListDTO>>("/odata/dishes");
        return result.Items.ToList();
    }
}
