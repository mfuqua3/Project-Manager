namespace ProjectManager.Core.Utility.Encryption
{
	public interface IEncryptionReader
	{
		string GetEncryptedSetting(string input);
	}
}
