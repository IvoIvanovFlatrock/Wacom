namespace WacomCore.Settings
{
	public class JWTSettings
	{
		public string Secret {  get; set; }

		public int ExpireTime { get; set; }
		public string Type { get; set; }
		public int RefreshTokenLength { get; set; }
	}
}
