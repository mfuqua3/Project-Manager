namespace ProjectManager.Common.Encryption
{
	/// <summary>
	/// Interface for reading encrypted settings.
	/// </summary>
	public interface IEncryptionReader
	{
		/// <summary>
		/// Retrieves the encrypted setting based on the given input.
		/// </summary>
		/// <param name="input">The input to be encrypted.</param>
		/// <returns>The encrypted setting.</returns>
		/// <remarks>
		/// This method takes the input string and encrypts it to provide the encrypted setting.
		/// The encrypted setting can be used to securely store and retrieve sensitive information.
		/// </remarks>
		string GetEncryptedSetting(string input);
	}
}
