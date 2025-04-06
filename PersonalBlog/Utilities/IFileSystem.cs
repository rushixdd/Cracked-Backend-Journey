namespace PersonalBlog.Utilities
{
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        string[] GetFiles(string path, string searchPattern);
        bool FileExists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
        void DeleteFile(string path);
    }
}
