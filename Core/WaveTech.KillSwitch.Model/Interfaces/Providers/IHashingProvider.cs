
namespace WaveTech.KillSwitch.Model.Interfaces.Providers
{
	public interface IHashingProvider
	{
		string ComputeHashWithSalt(string plainText,
											 string hashAlgorithm,
											 byte[] saltBytes);

		bool VerifyHashWithSalt(string plainText,
										string hashAlgorithm,
										string hashValue);

		string ComputeHash(string plainText, string hashAlgorithm);
	}
}