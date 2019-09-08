namespace PowerPlanSwitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(args);
            client.Run();
        }
    }
}
