namespace Application.Helpers
{
    public static class GenerateRandomCode
    {
        static string code;
        static GenerateRandomCode()
        {
            code = "";
        }
		private static readonly Random _random = new Random();

		public static string GetCode()
        {
			return _random.Next(0, 1000000).ToString("D6");
		}
	}
}
