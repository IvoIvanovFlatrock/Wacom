using NUnit.Framework;
using Moq;
using WacomBusiness.Services;
using WacomCore.Contracts;
using Microsoft.AspNetCore.Http;

namespace Wacom.UnitTests
{

	public class FileServiceTests
	{
		[Test]
		public async Task PushFile_WhenNoFileName_ThrowError()
		{
			//Arrange
			var fileRepository = new Mock<IFileRepository>();
			var file = new Mock<FormFile>();
			file.Setup(f => f.FileName).Returns("");
			var fileService = new FileService(fileRepository.Object);
			var userId = Guid.NewGuid();
			var expectedErrorMessage = "File name required.";

			//Act
			var result = Assert.ThrowsAsync<InvalidDataException>(() =>
				fileService.PushAsync(file.Object, userId));

			//Assert
			Assert.That(result.Message, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo(expectedErrorMessage));
		}
	}
}
