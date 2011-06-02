
using WaveTech.KillSwitch.Model.Results;

namespace WaveTech.KillSwitch.Model.Interfaces.Services
{
	public interface IDeleteService
	{
		FileSecureDeleteResult FileSecureDelete(string filePath);
		DirectorySecureDeleteResult DirectorySecureDelete(string path);
	}
}