namespace Renci.SshNet.Common
{
	internal class ChannelDataEventArgs : ChannelEventArgs
	{
		public byte[] Data
		{
			get;
			private set;
		}

		public uint DataTypeCode
		{
			get;
			private set;
		}

		public ChannelDataEventArgs(uint channelNumber, byte[] data)
			: base(channelNumber)
		{
			Data = data;
		}

		public ChannelDataEventArgs(uint channelNumber, byte[] data, uint dataTypeCode)
			: this(channelNumber, data)
		{
			DataTypeCode = dataTypeCode;
		}
	}
}
