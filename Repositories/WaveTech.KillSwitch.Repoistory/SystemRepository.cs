

using System.IO;
using System.Reflection;
using WaveTech.KillSwitch.Model;
using WaveTech.KillSwitch.Model.Interfaces.Providers;
using WaveTech.KillSwitch.Repoistory.Properties;

namespace WaveTech.KillSwitch.Repoistory
{
	public class SystemRepository
	{
		#region Private Readonly Members
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly ISymmetricEncryptionProvider _encryptionProvider;
		private readonly string _path;
		#endregion Private Readonly Members

		#region Constructor
		public SystemRepository(IObjectSerializationProvider objectSerializationProvider, ISymmetricEncryptionProvider encryptionProvider)
		{
			_objectSerializationProvider = objectSerializationProvider;
			_encryptionProvider = encryptionProvider;

			_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			_path = _path.Replace("file:\\", "");
		}
		#endregion Constructor

		#region Private Properties
		private string Name
		{
			get { return _path + "\\" + Resources.FileName; }
		}

		private bool FileExists()
		{
			return File.Exists(Name);
		}
		#endregion Private Properties

		public MasterConfiguration GetMasterConfiguration(EncryptionInfo encryptionInfo)
		{
			if (File.Exists(Name) == false)
				return null;

			string rawFileData;
			using (StreamReader reader = new StreamReader(Name))
			{
				rawFileData = reader.ReadToEnd();
			}

			string plainTextObjectData;
			plainTextObjectData = _encryptionProvider.Decrypt(rawFileData, encryptionInfo);

			MasterConfiguration mc = _objectSerializationProvider.Deserialize<MasterConfiguration>(plainTextObjectData);

			return mc;
		}

		public MasterConfiguration SaveMasterConfiguration(MasterConfiguration masterConfiguration, EncryptionInfo encryptionInfo)
		{
			if (File.Exists(Name))
				File.Delete(Name);

			string plainTextObjectData;
			plainTextObjectData = _objectSerializationProvider.Serialize(masterConfiguration);

			string encryptedObjectData;
			encryptedObjectData = _encryptionProvider.Encrypt(plainTextObjectData, encryptionInfo);

			using (StreamWriter writer = new StreamWriter(Name))
			{
				writer.Write(encryptedObjectData);
			}

			return GetMasterConfiguration(encryptionInfo);
		}
	}
}