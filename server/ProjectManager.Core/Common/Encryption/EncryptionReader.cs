namespace ProjectManager.Common.Encryption;

public class EncryptionReader: IEncryptionReader
{
    private readonly string _key;
		
    public EncryptionReader(string key)
    {
        _key = key;
    }

    public string GetEncryptedSetting(string input) 
        => AesEncryptionUtility.Decrypt(input, _key);
}