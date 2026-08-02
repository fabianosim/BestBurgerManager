using System;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using BestBurgerManagerAPI.Test.Resources;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BestBurgerManager.Entities;
using System.Linq;
using BestBurgerManager.Entities.Enums;

namespace BestBurgerManagerAPI.Test.Integration
{
    public class OrderControlletTests : IClassFixture<WebApplicationFactory<BestBurgerManagerAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public OrderControlletTests(WebApplicationFactory<BestBurgerManagerAPI.Startup> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Tests the order's main flow.
        /// </summary>
        /// <returns>Returns an async Task.</returns>
        [Fact]
        public async Task OrderMainFlowTest()
        {
            // Configure http client
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Place a single order using the order data from resource.
            using (var request = new HttpRequestMessage(HttpMethod.Post, "/api/orders/placeorder"))
            {
                using (var stringContent = new StringContent(JsonResources.singleOrder, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken())
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }

            // Get the created order with ID = 1
            var httpResponseGet = await client.GetAsync("/api/orders/orderbyid/1");
            httpResponseGet.EnsureSuccessStatusCode();

            JObject responseOrder = JsonConvert.DeserializeObject<JObject>(httpResponseGet.Content.ReadAsStringAsync().Result);
            Order resultOrder = JsonConvert.DeserializeObject<Order>(responseOrder["message"].Value<string>());
            Product orderProduct = resultOrder.Items.FirstOrDefault(); // For this test, only 1 product.

            Assert.NotNull(resultOrder);
            Assert.NotNull(orderProduct);
            Assert.Equal(OrderStatus.Pending, resultOrder.Status);
            Assert.Equal(ProductStatus.Queued, orderProduct.Status);

            // Kitchen user will now change the product status to Preparing
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"/api/kitchen/order/{resultOrder.Id}/product/preparing/{orderProduct.Id}"))
            {
                using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken())
                        .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }

            // Get the updated order and check if the status was updated.
            httpResponseGet = await client.GetAsync($"/api/orders/orderbyid/{resultOrder.Id}");
            httpResponseGet.EnsureSuccessStatusCode();

            JObject updatedOrderResponse = JsonConvert.DeserializeObject<JObject>(httpResponseGet.Content.ReadAsStringAsync().Result);
            Order updatedOrder = JsonConvert.DeserializeObject<Order>(updatedOrderResponse["message"].Value<string>());
            Product updatedProduct = updatedOrder.Items.FirstOrDefault();

            Assert.Equal(OrderStatus.InProgress, updatedOrder.Status);
            Assert.Equal(ProductStatus.Preparing, updatedProduct.Status);

            // Kitchen user will now change the product status to Ready.
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"/api/kitchen/order/{updatedOrder.Id}/product/ready/{updatedProduct.Id}"))
            {
                using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken())
                        .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }

            // Get the updated order and product.
            httpResponseGet = await client.GetAsync($"/api/orders/orderbyid/{updatedOrder.Id}");
            httpResponseGet.EnsureSuccessStatusCode();

            JObject inprogressOrderResponse = JsonConvert.DeserializeObject<JObject>(httpResponseGet.Content.ReadAsStringAsync().Result);
            Order inprogressOrder = JsonConvert.DeserializeObject<Order>(inprogressOrderResponse["message"].Value<string>());
            Product readyProduct = inprogressOrder.Items.FirstOrDefault();

            Assert.Equal(OrderStatus.InProgress, inprogressOrder.Status);
            Assert.Equal(ProductStatus.Ready, readyProduct.Status);


            // Kitchen user will now change the order status to Ready.
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"/api/kitchen/order/ready/{inprogressOrder.Id}"))
            {
                using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken())
                        .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }

            // Get the updated order. Now it should be ready.
            httpResponseGet = await client.GetAsync($"/api/orders/orderbyid/{inprogressOrder.Id}");
            httpResponseGet.EnsureSuccessStatusCode();

            JObject readyOrderResponse = JsonConvert.DeserializeObject<JObject>(httpResponseGet.Content.ReadAsStringAsync().Result);
            Order readyOrder = JsonConvert.DeserializeObject<Order>(readyOrderResponse["message"].Value<string>());

            Assert.Equal(OrderStatus.Ready, readyOrder.Status);
        }
    }
}
