using System.Runtime.InteropServices;

namespace Api.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;
        
        public static class Post
        {
            public const string GetAll = Base + "/posts";
            public const string Update = Base + "/posts/{postId:long}";
            public const string Delete = Base + "/posts/{postId:long}";
            public const string Get = Base + "/posts/{postId:long}";
            public const string Create = Base + "/posts";
        }
        public static class Tags
        {
            public const string GetAll = Base + "/tags";
        }
    }
}