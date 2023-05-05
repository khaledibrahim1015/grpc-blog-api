using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace server.Impl
{
    public class BlogServiceImpl : BlogService.BlogServiceBase
    {
        private static string connectionString = "mongodb://localhost:27017";
        private static MongoClient mongoClient = new MongoClient(connectionString);

        private static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("BolgDB");

        private static IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>("Blog");

        public override Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
        {

            //string id = 1;
            //string author_id = 2;
            //string title = 3;
            //string content = 4;

            var blogDocument = request.Blog;

            BsonDocument document = new BsonDocument("author_id", blogDocument.AuthorId)
                                                        .Add("title", blogDocument.Title)
                                                        .Add("content", blogDocument.Content);


            // insert one document => without id => _id Guid 

            mongoCollection.InsertOne(document);

            // Get _id to response for Clinet

            string id = document.GetValue("_id").ToString();

            blogDocument.Id = id;
            return Task.FromResult(

                new CreateBlogResponse()
                {
                    Blog = blogDocument
                });

        }






    }
}
