using System;

namespace WaveTech.KillSwitch.Model.Results
{
	public class FileSecureDeleteResult
	{
		public string OriginalPath { get; set; }
		public string FinalPath { get; set; }
		public int WriteItterations { get; set; }
		public int RenameItterations { get; set; }
		public bool DeleteSuccessful { get; set; }
		public Exception Error { get; set; }
		public string ErrorMessage { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}