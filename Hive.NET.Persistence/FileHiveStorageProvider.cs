using System.Text.Json;
using Hive.NET.Core.Configuration.Storage;

namespace Hive.NET.Persistence;

  public class FileHiveStorageProvider : IHiveStorageProvider
    {
        private readonly string _filePath;

        public FileHiveStorageProvider(string filePath)
        {
            _filePath = filePath;
            EnsureFileCreated();
        }

        public void UpsertHive(Guid id, Core.Components.Hive? hive)
        {
            var hives = GetAllHives();
            var existingHiveIndex = hives.FindIndex(existing => existing!.Id == id);

            if (existingHiveIndex != -1)
            {
                // Update existing hive
                hives[existingHiveIndex] = hive;
            }
            else
            {
                // Insert new hive
                hives.Add(hive);
            }

            WriteHivesToFile(hives!);
        }

        public Core.Components.Hive? GetHive(Guid id)
        {
            var hives = GetAllHives();
            return hives.Find(existing => existing!.Id == id);
        }

        public List<Core.Components.Hive> GetAllHives()
        {
            var hives = new List<Core.Components.Hive>();

            try
            {
                using (var fileStream = File.OpenRead(_filePath))
                using (var streamReader = new StreamReader(fileStream))
                {
                    var jsonString = streamReader.ReadToEnd();
                    hives = JsonSerializer.Deserialize<List<Core.Components.Hive>>(jsonString);
                }
            }
            catch (FileNotFoundException)
            {
                // Ignore if the file is not found, as it will be created on the next write
            }

            return (hives ?? new List<Core.Components.Hive>())!;
        }
        
        private void EnsureFileCreated()
        {
            if (!File.Exists(_filePath))
            {
                using (File.Create(_filePath)) { }
            }
        }

        private void WriteHivesToFile(List<Core.Components.Hive> hives)
        {
            var jsonString = JsonSerializer.Serialize(hives);

            using (var fileStream = File.OpenWrite(_filePath))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(jsonString);
            }
        }
    }