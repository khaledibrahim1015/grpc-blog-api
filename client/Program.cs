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
            //   CreateNewBlog(channel);


            // 2 - Call BlogService => and call ReadBlog
            //   ReadBlogById(channel);


            // 3 - Call BlogService => and call UpdateBlog
            UpdateBlog(channel);












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



        private static void ReadBlogById(Channel channel)
        {

            var client = new BlogService.BlogServiceClient(channel);

            try
            {
                var response = client.ReadBlog(new ReadBlogRequest()
                {
                    BlogId = "6454668fc20bdaa2049332c2"
                });

                Console.WriteLine($"The Blog {response.Blog.AuthorId} {response.Blog.Title} {response.Blog.Content}");
            }
            catch (RpcException e ) when(e.StatusCode == StatusCode.NotFound)
            {

                Console.WriteLine("Error" + e.Status.Detail);
            }
            
           


        }


        private static void UpdateBlog( Channel channel)
        {
            var client = new BlogService.BlogServiceClient(channel);
            var response = client.UpdateBlog(new UpdateBlogRequest()
            {
                Blog = new Blog()
                {
                    Id = "",
                    AuthorId = "Ibra",
                    Content = "Hello World , this is the Updated blog ",
                    Title = "Updated Blog !"
                }
            });
            Console.WriteLine(response.Blog.ToString());



        }



    }
}