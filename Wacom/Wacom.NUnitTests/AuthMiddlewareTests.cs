using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.ComponentModel.DataAnnotations;
using WacomAPI.Middleware;
using WacomCore.Contracts;
using WacomCore.Settings;

namespace Wacom.NUnitTests
{
	public class AuthMiddlewareTests
	{
		private Mock<RequestDelegate> requestDelegate;
		private DefaultHttpContext context;

		[SetUp]
		public void Setup()
		{
			this.requestDelegate = new Mock<RequestDelegate>();
			this.context = new DefaultHttpContext();
		}

		[Test]
		public async Task Invoke_WhenTokenheaderIsNotBearer_ThrowError()
		{
			//Arrange
			var userService = new Mock<IUserService>();
			var jwtSettingsMock = new Mock<IOptions<JWTSettings>>();
			var jwtSettings = new JWTSettings() { Type = "Bearer" };
			jwtSettingsMock.Setup(x => x.Value).Returns(jwtSettings);
			var noBearer = "\"NoBearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImQ2Y2M1MWViLWRjOTEtNGYyYy1hZmYyLTVkMTE1MTk3MDhlMyIsIm5iZiI6MTcxNDQ2OTQ3OCwiZXhwIjoxNzE0NDczMDc4LCJpYXQiOjE3MTQ0Njk0Nzh9.7qIOdEiHkGxIv5OPQA_9_9Er8LtJ5YwRCVlZCFrbmhA\"";
			this.context.Request.Headers.Add("Authorization", noBearer);
			var authMiddleware = new AuthMiddleware(jwtSettingsMock.Object,
				userService.Object);

			//Act
			var result = Assert.ThrowsAsync<ValidationException>(() =>
				authMiddleware.InvokeAsync(this.context, this.requestDelegate.Object));

			//Assert
			Assert.That(result.Message, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo("Error Authentication."));
		}
	}
}
