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



            // 1 - Call BlogService => and call CreateBlog 
               CreateNewBlog(channel);









            channel.ShutdownAsync().Wait();
            Console.ReadKey();


        }


        // Call BlogService => and call CreateBlog 
        private static void CreateNewBlog(Channel channel)
        {
            var client = new BlogService.BlogServiceClient(channel);

            var BlogResponse = client.CreateBlog(new CreateBlogRequest()
            {
                Blog = new Blog()
                {
                    Id = Guid.NewGuid().ToString(),
                    AuthorId = "khaled",
                    Content = "Hello World , this is the new blog",
                    Title = "New Blog !"
                }
            });

            Console.WriteLine("The Blog " + BlogResponse.Blog.Id + " Was Created");
        }
    }
}