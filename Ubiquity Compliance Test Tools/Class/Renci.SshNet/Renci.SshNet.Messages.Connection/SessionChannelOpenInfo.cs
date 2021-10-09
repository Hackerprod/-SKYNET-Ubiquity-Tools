using Renci.SshNet.Messages.Connection;

internal class SessionChannelOpenInfo : ChannelOpenInfo
{
    public const string NAME = "session";

    public override string ChannelType => "session";
}