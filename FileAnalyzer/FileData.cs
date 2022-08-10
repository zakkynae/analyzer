﻿namespace FileAnalyzer
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
            FullName = fullName;
            Name = name;
            Extension = extension;
            Length = length;
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
