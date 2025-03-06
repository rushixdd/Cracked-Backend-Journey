﻿namespace PersonalBlog.Models
{
    public class Article
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; } = DateTime.Now;
    }
}
