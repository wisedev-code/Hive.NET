namespace Hive.NET.Demo.FE;

using Api.EntityModel;
using Api.RequestModel;

public class HiveApiClient
{
    private readonly HttpClient _httpClient;

    public HiveApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await _httpClient.GetFromJsonAsync<List<Customer>>("api/Customer") ?? [];
    }

    public async Task PostCustomer(CustomerCreateRequest request)
    {
        await _httpClient.PostAsJsonAsync("api/Customer", request);
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _httpClient.GetFromJsonAsync<List<Order>>("api/Order") ?? [];
    }

    public async Task PostOrder(OrderCreateRequest request)
    {
        await _httpClient.PostAsJsonAsync("api/Order", request);
    }

    public async Task DeleteOrder(Guid id)
    {
        await _httpClient.DeleteAsync($"api/Order/{id}");
    }

    public async Task ApplyDiscount(Guid customerId, CustomerDiscountRequest request)
    {
        await _httpClient.PutAsJsonAsync($"api/Customer/{customerId}", request);
    }
}
