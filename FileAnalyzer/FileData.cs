namespace FileAnalyzer
{
    public class FileData
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public DateTime CreationDate { get; set; }


        public FileData(string fullName, string name, string extension, long length, DateTime creationTime)
        {
            FullName =  string.IsNullOrEmpty(fullName) ? throw new ArgumentException("Полное имя файла не может принимать такое значение") : fullName;
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentException("Имя файла не может принимать такое значение") : name; 
            Extension = string.IsNullOrEmpty(extension) ? throw new ArgumentException("Расширение не может принимать такое значение") : extension;
            Length = length < 0 ? throw new ArgumentException("Размер файла не может принимать такое значение") : length;
            CreationDate = creationTime;
        }
        

        public override string ToString()
        {
            return $"Имя файла: {Name}\tРасширение файла: {Extension}\tРазмер файла: {Length}\tДата создания файла: {CreationDate}\n";
        }
        public override bool Equals(object? obj)
        {
            if (obj is FileData fileData) 
                return Name == fileData.Name && Length == fileData.Length && Extension == fileData.Extension;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
