using System;
using StackExchange.Redis;

namespace ExampleRedisSubscriber
{
    class Program
    {
        private const string channel = "WEATHER";


        static void Main(string[] args)
        {
            Console.WriteLine($"Subscribing to channel {channel}");

            ConfigurationOptions redisOptions = new ConfigurationOptions()
            {
                EndPoints =
                {
                    {"127.0.0.1", 6379}
                },
                AbortOnConnectFail = false
            };

            using (var multiplexer = ConnectionMultiplexer.Connect(redisOptions))
            {
                PerformSubscription(multiplexer.GetSubscriber());
                Console.WriteLine("Press Any Key to Continue...");
                Console.ReadKey();
            }
        }

        static void PerformSubscription(ISubscriber subscriber)
        {
            subscriber.Subscribe(channel, (redisChannel, value) =>
            {
                Console.WriteLine($"Message Received on channel {redisChannel}: \"{value}\"");

            });
        }
    }
}
