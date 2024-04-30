using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using WacomAPI.Controllers;
using WacomCore.Contracts;
using WacomCore.Settings;

namespace Wacom.NUnitTests
{
	public class FileControllerTests
	{
		private FilesController fileController;
		private Mock<IOptions<FileSettings>> fileSettingsMock;
		private int fileSize = 1000;
		[SetUp]
		public void Setup()
		{
			this.fileSettingsMock = new Mock<IOptions<FileSettings>>();
			var fileServiceMock = new Mock<IFileService>();
			var jwtSettings = new FileSettings() { FileSize = fileSize };
			fileSettingsMock.Setup(x => x.Value).Returns(jwtSettings);
			this.fileController = new FilesController(fileSettingsMock.Object,
				fileServiceMock.Object);
		}

		[Test]
		public async Task PostFile_WhenFileIsNull_ReturnBadReq()
		{
			//Act
			var response = await fileController.Post(null);

			//Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(response);
			var badReq = (BadRequestObjectResult)response;
			Assert.That(badReq, Is.Not.Null);
			Assert.That(badReq.StatusCode, Is.EqualTo(400));
		}

		[Test]
		public async Task PostFile_WhenFileIsSizeIsLarger_ReturnBadReq()
		{
			//Arrange
			var stream = new MemoryStream();
			var file = new FormFile(
				stream, 0, fileSize+1, "id_from_form", "");

			//Act
			var response = await fileController.Post(file);

			//Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(response);
			var badReq = (BadRequestObjectResult)response;
			Assert.That(badReq, Is.Not.Null);
			Assert.That(badReq.StatusCode, Is.EqualTo(400));
			Assert.That(badReq.Value, Is.EqualTo($"File is too large. Maximum of {this.fileSize}"));
		}
	}
}
