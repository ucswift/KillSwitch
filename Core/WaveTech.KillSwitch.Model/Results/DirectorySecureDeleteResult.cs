using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveTech.KillSwitch.Model.Results
{
	public class DirectorySecureDeleteResult
	{
		public List<FileSecureDeleteResult> FileDeleteResults { get; set; }

		public DirectorySecureDeleteResult()
		{
			FileDeleteResults = new List<FileSecureDeleteResult>();
		}
	}
}
