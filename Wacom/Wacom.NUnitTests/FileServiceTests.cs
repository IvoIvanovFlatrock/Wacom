using Microsoft.AspNetCore.Http;
using Moq;
using WacomBusiness.Services;
using WacomCore.Contracts;

namespace Wacom.NUnitTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public async Task PushFile_WhenNoFileName_ThrowError()
		{
			//Arrange
			var fileRepository = new Mock<IFileRepository>();
			var stream = new MemoryStream();
			var file = new FormFile(stream, 0, stream.Length, "id_from_form", "");
			var fileService = new FileService(fileRepository.Object);
			var userId = Guid.NewGuid();
			var expectedErrorMessage = "File name required.";

			//Act
			var result = Assert.ThrowsAsync<InvalidDataException>(() =>
				fileService.PushAsync(file, userId));

			//Assert
			Assert.That(result.Message, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo(expectedErrorMessage));
		}
	}
}