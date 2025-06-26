using NUnit.Framework;
using Microsoft.Playwright;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestBase;
using Models;

namespace UiTests
{
    [TestFixture]
    public class DataUiTests : UiTestBase
    {
        [Test]
        public async Task Test_VerifyCreatedItemIsVisible()
        {
            // Arrange
            var payload = new CreateDataItemRequest
            {
                Name = "Test Item " + Guid.NewGuid(),
                Description = "This is a test description"
            };

            var options = new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                Data = JsonSerializer.Serialize(payload, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            };

            // Act
            var response = await API.PostAsync("/api/data", options);
            JsonElement responseBody = (JsonElement)await response.JsonAsync();

            // Assert
            Assert.IsTrue(responseBody.TryGetProperty("id", out JsonElement idProperty), "Response does not contain 'id'");
            var itemId = idProperty.GetInt32();
            Assert.IsTrue(responseBody.TryGetProperty("name", out JsonElement nameProperty), "Response does not contain 'name'");
            var itemName = nameProperty.GetString();

            TestContext.WriteLine("📤 Request Payload:\n" + JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
            TestContext.WriteLine("📥 Response Body:\n" + responseBody.ToString());

            // Navigate to the UI
            await Page.GotoAsync("http://localhost:5000");
            TestContext.WriteLine("🌐 Navigating to UI: http://localhost:5000");

            // Verify the item appears in the list
            var itemSelector = $"li[data-item-id='{itemId}']";
            var itemElement = await Page.QuerySelectorAsync(itemSelector);
            Assert.IsNotNull(itemElement, $"Item with ID {itemId} not found in the UI");
            Assert.AreEqual(itemName, await itemElement.InnerTextAsync(), "The name of the item in the UI does not match");

            // Highlight the item
            await Page.EvalOnSelectorAsync(itemSelector, "el => el.style.border = '3px solid red'");
            TestContext.WriteLine($"🔍 Verifying item with selector: {itemSelector}");

            // Capture screenshot
            var screenshotPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
            TestContext.WriteLine($"📸 Screenshot saved at: {screenshotPath}");
            TestContext.AddTestAttachment(screenshotPath);

            TestContext.WriteLine("✅ UI verification successful.");
        }
    }
}