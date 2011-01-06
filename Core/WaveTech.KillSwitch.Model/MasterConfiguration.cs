using System.Collections.Generic;

namespace WaveTech.KillSwitch.Model
{
	public class MasterConfiguration
	{
		public string MasterKey { get; set; }
		public SwitchTypes SwitchType { get; set; }
		public List<Wipe> Wipes { get; set; }
		public EncryptionInfo EncryptionInfo { get; set; }
	}
}
