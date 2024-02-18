using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System;
using System.Text.Json.Nodes;

namespace PayPalWebsiteTeil.Pages
{
    //code for paypal implementation done with this tutorial: https://www.youtube.com/watch?v=qLXDsoYOopU&t=654s

    [IgnoreAntiforgeryToken]
    public class CheckoutModel : PageModel
    {
        public string PayPalClientId { get; set; } = "";
        public string PayPalSecret { get; set; } = "";
        public string PayPalUrl { get; set; } = "";

        public string Total { get; set; } = "";
        public string ProductIdentifiers { get; set; } = "";
        public string Email { get; set; } = "";


        public CheckoutModel(IConfiguration config)
        {
            PayPalClientId = config["PaypalSettings:ClientId"]!;
            PayPalSecret = config["PaypalSettings:PayPalSecret"]!;
            PayPalUrl = config["PaypalSettings:PayPalUrl"]!;
        }

        public void OnGet()
        {
            Console.WriteLine("" + TempData["Total"] + "" + TempData["ProductIdentifiers"]);

            Total = TempData["Total"]?.ToString() ?? "";
            ProductIdentifiers = TempData["ProductIdentifiers"]?.ToString() ?? "";
            Email = TempData["Email"]?.ToString() ?? "";


            TempData.Keep();

            if (Total == "" || ProductIdentifiers == "")
            {
                Response.Redirect("/");
                return;
            }
        }

        public JsonResult OnPostCreateOrder()
        {
            Total = TempData["Total"]?.ToString() ?? "";
            ProductIdentifiers = TempData["ProductIdentifiers"]?.ToString() ?? "";
            
            TempData.Keep();

            if (Total == "" || ProductIdentifiers == "")
            {
                return new JsonResult("");
            }

            //RequestBody
            JsonObject createOrderRequest = new JsonObject();

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "EUR");
            amount.Add("value", Total);

            JsonObject pruchaseUnits1 = new JsonObject();
            pruchaseUnits1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(pruchaseUnits1);

            createOrderRequest.Add("purchase_units", purchaseUnits);
            createOrderRequest.Add("intent", "CAPTURE");

            string accessToken = GetPayPalAccessToken();

            Console.WriteLine(createOrderRequest);
            Console.WriteLine(accessToken);

            string url = PayPalUrl + "/v2/checkout/orders";


            string orderId = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        Console.WriteLine(jsonResponse);

                        orderId = jsonResponse["id"]?.ToString() ?? "";
                    }
                }
            }

            var response = new
            {
                Id = orderId
            };
            return new JsonResult(response);
        }

        public JsonResult OnPostCompleteOrder([FromBody] JsonObject data) 
        { 
            if(data == null || data["orderID"] == null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();

            string accesToken = GetPayPalAccessToken();

            string url = PayPalUrl + "/v2/checkout/orders/" + orderID + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accesToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        Console.WriteLine(jsonResponse);

                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";

                        if (paypalOrderStatus == "COMPLETED" && paypalOrderId != "")
                        {
                            string success_message = "SUCCESS";
                            ChangeStatusPayed(success_message, paypalOrderId);
                            TempData.Clear();
                            return new JsonResult("success");
                        }
                    }
                }
            }

            return new JsonResult("");
        }

        public JsonResult OnPostCancelOrder([FromBody] JsonObject data)
        {
            if (data == null || data["orderID"] == null) return new JsonResult("");

            var orderID = data["orderID"]!.ToString();

            string cancle_message = "CANCELED";
            ChangeStatusPayed(cancle_message, orderID);


            return new JsonResult("");
        }

        private String GetPayPalAccessToken()
        {
            string accesToken = "";
            string urlaccesstoken = PayPalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PayPalClientId + ":" + PayPalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, urlaccesstoken);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var responeseTask = client.SendAsync(requestMessage);
                responeseTask.Wait();

                var result = responeseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accesToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }

            return accesToken;
        }

        public async void ChangeStatusPayed(string message, string paypalOrderId)
        {
            string url = "https://oliverserver.ddns.net/";
            //string url = "https://localhost:7188/";

            var _client = Connect(url);
            //"accepted" oder "cancled" message mitgeben
            var requestUri = url + $"api/MenuPersonAPI/updatePayedOrder?userEmail={Uri.EscapeDataString(Email)}";

            var requestMessageChangeStatusPayed = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(requestUri),
            };
            using var response = await _client.SendAsync(requestMessageChangeStatusPayed);

        }
        public HttpClient Connect(string url)
        {
            if (url == "https://localhost:7188/")
            {
                HttpClient _localhost_client = new HttpClient();
                return _localhost_client;
            }
            else if (url == "https://oliverserver.ddns.net/")
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var _oliverserver_client = new HttpClient(handler);

                return _oliverserver_client;
            }
            else
            {
                throw new Exception("Konnte nicht verbunden werden");
            }

        }

    }
}
