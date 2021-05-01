namespace MyKlc.Plugin.Infrastructure.Messages
{
    public delegate void MessageReceivedEventHandler(KlcMessage message);
    public delegate void MessageSentEventHandler(bool value);
}
