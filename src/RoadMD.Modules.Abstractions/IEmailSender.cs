namespace RoadMD.Modules.Abstractions
{
    public interface IEmailSender
    {
        public void Send(string to, string email);
    }
}
