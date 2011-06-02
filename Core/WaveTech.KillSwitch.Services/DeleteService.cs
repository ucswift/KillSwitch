// FileSecureDelete code inspired from http://www.codeproject.com/KB/cs/SharpWipe.aspx

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using WaveTech.KillSwitch.Model.Interfaces.Services;
using WaveTech.KillSwitch.Model.Results;

namespace WaveTech.KillSwitch.Services
{
	public class DeleteService : IDeleteService
	{
		private const int MAX_PASSES = 10;

		public FileSecureDeleteResult FileSecureDelete(string filePath)
		{
			FileSecureDeleteResult result = new FileSecureDeleteResult();
			result.OriginalPath = filePath;
			result.StartTime = DateTime.Now;

			try
			{
				if (File.Exists(filePath))
				{
					byte[] wipeBuffer = new byte[512];
					RNGCryptoServiceProvider randomData = new RNGCryptoServiceProvider();
          
					File.SetAttributes(filePath, FileAttributes.Normal);
					FileStream fileToWipeStream = new FileStream(filePath, FileMode.Open);
					double blocks = Math.Ceiling(new FileInfo(filePath).Length / 512d);

					for (int i = 0; i < MAX_PASSES; i++)
					{
						fileToWipeStream.Position = 0;

						for (int blocksWritten = 0; blocksWritten < blocks; blocksWritten++)
						{
							randomData.GetBytes(wipeBuffer);
							fileToWipeStream.Write(wipeBuffer, 0, wipeBuffer.Length);
						}

						result.WriteItterations++;
					}

					fileToWipeStream.SetLength(0);
					fileToWipeStream.Close();

					DateTime dt = new DateTime(2037, 1, 1, 0, 0, 0);
					File.SetCreationTime(filePath, dt);
					File.SetLastAccessTime(filePath, dt);
					File.SetLastWriteTime(filePath, dt);
					File.SetCreationTimeUtc(filePath, dt);
					File.SetLastAccessTimeUtc(filePath, dt);
					File.SetLastWriteTimeUtc(filePath, dt);

					string newFilePath = filePath;
					for (int i = 0; i < MAX_PASSES; i++)
					{
						string tempPath = Path.GetDirectoryName(newFilePath) + Guid.NewGuid();
						File.Move(newFilePath, Path.GetDirectoryName(newFilePath) + tempPath);

						newFilePath = tempPath;
						result.RenameItterations++;
					}

					File.Delete(newFilePath);
					if (File.Exists(newFilePath))
					{
						result.DeleteSuccessful = false;
						result.ErrorMessage = string.Format("File {0} existed after Delete message sent.", newFilePath);
					}
				}
			}
			catch (Exception ex)
			{
				result.DeleteSuccessful = false;
				result.ErrorMessage = ex.ToString();
				result.Error = ex;
			}

			result.EndTime = DateTime.Now;
			return result;
		}

		public DirectorySecureDeleteResult DirectorySecureDelete(string path)
		{
			DirectorySecureDeleteResult result = new DirectorySecureDeleteResult();

			try
			{
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

					Parallel.ForEach(files, currentFile =>
					                        	{
																			result.FileDeleteResults.Add(FileSecureDelete(currentFile));
					                        	});


				}
			}
			catch (Exception ex)
			{
				
				throw;
			}

			return result;
		}
	}
}