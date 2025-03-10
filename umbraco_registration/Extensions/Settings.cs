namespace umbraco_registration.Extensions
{
    public class Settings
    {
        public Robots? Robots { get; init; }
    }

    public class Robots
    {
        public string[] Allow { get; init; } = { };
        public string[] Disallow { get; init; } = { };
    }
}
