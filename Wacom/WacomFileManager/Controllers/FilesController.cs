using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WacomCore.Contracts;
using WacomCore.Helpers;
using WacomCore.Models;
using WacomCore.Settings;

namespace WacomAPI.Controllers
{
	[ApiController]
	[Authorize]
	public class FilesController : ControllerBase
	{
		private readonly FileSettings fileSetting;
		private readonly IFileService fileService;

		public FilesController(IOptions<FileSettings> fileSettings,
			IFileService fileService) 
		{
			this.fileSetting = fileSettings.Value;
			this.fileService = fileService;
		}

		[HttpGet("GetFile")]
		public async Task<FileResult> Get()
		{
			var user = new object();
			this.HttpContext.Items.TryGetValue("User", out user);
			var id = (user as UserModel).Id;
			var result = await fileService.GetByIdAsync(id);
			var path = Path.Combine(result.Path, result.FileName);
			var file = System.IO.File.ReadAllBytes(path);
			return new FileContentResult(file, result.FileInfo);
		}

		[HttpGet("GetFileByName")]
		public async Task<FileResult> GetByName(string name)
		{
			var user = new object();
			this.HttpContext.Items.TryGetValue("User", out user);
			var id = (user as UserModel).Id;
			var result = await fileService.GetByIdAndNameAsync(name, id);
			var path = Path.Combine(result.Path, result.FileName);
			var file = System.IO.File.ReadAllBytes(path);
			return new FileContentResult(file, result.FileInfo);
		}

		[HttpPost("Post")]
		public async Task<IActionResult> Post(IFormFile file)
		{
			if (file == null)
			{
				return BadRequest("File is required.");
			}

			if (file.Length > fileSetting.FileSize)
			{
				return BadRequest($"File is too large. Maximum of {fileSetting.FileSize}");
			}

			var type = Path.GetExtension(file.FileName);
			if (type != ".img" && type != ".pdf")
			{
				return BadRequest($"File type is not allowed.");
			}

			var user = new object();
			this.HttpContext.Items.TryGetValue("User", out user);
			var id = (user as UserModel).Id;
			await fileService.PushAsync(file, id);

			return Ok(file.FileName);
		}
	}
}
