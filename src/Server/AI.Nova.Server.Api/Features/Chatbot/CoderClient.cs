namespace AI.Nova.Server.Api.Features.Chatbot;

public class CoderClient(IChatClient client)
{
    public IChatClient Client => client;
}
