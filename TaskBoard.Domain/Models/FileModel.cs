using CSharpFunctionalExtensions;

namespace TaskBoard.Domain.Models
{
    public class FileModel
    {
        private FileModel(string fileName)
        {
            FileName = fileName;
        }
        public Guid Id { get; set; }
        public string FileName { get; private set; } = string.Empty;
        public Guid OwnerId { get; set; } = Guid.Empty;

        public static Result<FileModel> Create(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Result.Failure<FileModel>($"'{nameof(fileName)}' cannot be null or empty");
            }

            var file = new FileModel(fileName);

            return Result.Success<FileModel>(file);
        }
    }
}
