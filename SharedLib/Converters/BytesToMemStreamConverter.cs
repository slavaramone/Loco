using System.IO;

namespace SharedLib.Converters
{
    public static class BytesToMemStreamConverter
    {
		public static MemoryStream Convert(byte[] fileBytes)
		{
			var ms = new MemoryStream();
			ms.Write(fileBytes, 0, fileBytes.Length);
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}
	}
}
