using Grpc.Core;

namespace client
{
    internal class Program
    {
        const string target = "127.0.0.1:50052";
        static async Task Main(string[] args)
        {
            // Configuration To Connect on Server 
            Channel channel = new Channel(target, ChannelCredentials.Insecure);
            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The Client Connected Successfully");
                }
            });









            channel.ShutdownAsync().Wait();
            Console.ReadKey();


        }
    }
}