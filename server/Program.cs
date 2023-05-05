using Grpc.Core;
using server.Impl;

namespace server
{
    internal class Program
    {


        const int port = 50052;
        static void Main(string[] args)
        {

            Server server = null;

            try
            {

                server = new Server()
                {

                    // Register services
                    Services = {BlogService.BindService(new BlogServiceImpl()),
                    
                    
                    },
                    Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine("The Server is listening on the port " + port);




                Console.ReadKey();



            }
            catch (IOException e )
            {
                Console.WriteLine("The Server is Failed to start : " + e.Message);
                throw;
                
            }
            finally
            {
                if (server != null)
                    server.ShutdownAsync().Wait();
            }
        }
    }
}