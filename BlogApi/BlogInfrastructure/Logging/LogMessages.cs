namespace BlogInfrastructure.Logging
{
    public static class LogMessages
    {
        // Startup
        public const string AppStarting = "Starting application...";
        public const string AppStartupFailed = "Application startup failed";

        // Blog CRUD
        public const string CreatingPost = "Creating a new blog post.";
        public const string PostCreated = "Successfully created blog post with ID {PostId}.";
        public const string GettingAllPosts = "Fetching all blog posts.";
        public const string GettingPostById = "Fetching blog post with ID {PostId}.";
        public const string PostNotFound = "Blog post with ID {PostId} not found.";
        public const string UpdatingPost = "Updating blog post with ID {PostId}.";
        public const string PostUpdated = "Successfully updated blog post with ID {PostId}.";
        public const string DeletingPost = "Deleting blog post with ID {PostId}.";
        public const string PostDeleted = "Successfully deleted blog post with ID {PostId}.";

        // Errors
        public const string UnexpectedError = "Unhandled exception occurred: {Error}";
    }
}
