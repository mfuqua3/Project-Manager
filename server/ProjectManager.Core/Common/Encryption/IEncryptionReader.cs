namespace ProjectManager.Common.Encryption
{
	public interface IEncryptionReader
	{
		string GetEncryptedSetting(string input);
	}
}
