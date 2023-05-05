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

        public override  Task<ReadBlogResponse> ReadBlog(ReadBlogRequest request, ServerCallContext context)
        {
            var blogId = request.BlogId;

            // GetBlogById
            

            // var f= new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(blogId))
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId( blogId));

            var result = mongoCollection.Find(filter).FirstOrDefault();
            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound, "The Blog id " + blogId + " Not found "));



            Blog blog = new Blog()
            {
                AuthorId = result.GetValue("author_id").AsString,
                Content = result.GetValue("content").AsString,
                Title = result.GetValue("title").AsString
            };

            return Task.FromResult(new ReadBlogResponse()
            {
                Blog = blog
            });

        }



        public override async Task<UpdateBlogResponse> UpdateBlog(UpdateBlogRequest request, ServerCallContext context)
        {

            var blogId = request.Blog.Id;

            var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", blogId);

            var result = mongoCollection.Find(filter).FirstOrDefault();

            if (result == null)
                throw new RpcException(new Status(StatusCode.NotFound, "The blog id" + blogId + " NotFound"));



            BsonDocument doc = new BsonDocument("author_id", request.Blog.AuthorId)
                                                .Add("title", request.Blog.Title)
                                                .Add("content", request.Blog.Content);

            mongoCollection.ReplaceOne(filter, result);

            // Response with Updated Virsion 

            Blog blog = new Blog()
            {
                AuthorId = doc.GetValue("author_id").AsString,
                Content = doc.GetValue("content").AsString,
                Title = doc.GetValue("title").AsString
            };

            return new UpdateBlogResponse()
            {
                Blog = blog
            };







        }



    }
}
