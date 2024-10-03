using System.Diagnostics;
using System.Drawing.Imaging;

namespace PhotoResizerLibrary;

public class ResizerService
{
    public long MAX_PHOTO_SIZE_KB = 1024;
    public ResizerConfig ResizerConfig { get; set; }
    public ResizerService()
    {
        ResizerConfig = new ResizerConfig().WithMaxImageSizeInKiloBytes(MAX_PHOTO_SIZE_KB);
    }

    public ResizerService(ResizerConfig resizerConfig)
    {
        ResizerConfig = resizerConfig;
    }

    public IEnumerable<string> GetImages(string workingDirectory)
    {
        ResizerConfig = ResizerConfig.WithWorkingDirectory(workingDirectory);
        var outputDirectory = workingDirectory + @"\Resized\";
        if (Directory.Exists(outputDirectory))
        {
            Directory.Delete(outputDirectory, true);    
        }
        Directory.CreateDirectory(outputDirectory);

        var photos = Directory.EnumerateFiles(workingDirectory, "*.jpg");

        if (!photos.Any())
        {
            Console.WriteLine($"No images to process in {workingDirectory}");
            return new List<string>() { $"No images to process in {workingDirectory}" };
        }
        return photos;
    }

    public FileInfo ProcessImage(string photo)
    {
        var workingDirectory = ResizerConfig.WorkingDirectory;
        var outputDirectory = workingDirectory + @"\Resized\";
        var photoName = Path.GetFileNameWithoutExtension(photo);
        var fi = new FileInfo(photo);
        Console.WriteLine($"Photo: {photo} {fi.Length / 1000}kB");

        if (fi.Length / 1000 > ResizerConfig.MaxImageSizeInKiloBytes)
        {
            using (var image = Image.FromFile(photo))
            {
                using (var stream = DownscaleImage(image))
                {
                    if(!File.Exists(outputDirectory + photoName + "-resized.jpg"))
                    {
                        using (var file = File.Create(outputDirectory + photoName + "-resized.jpg"))
                        {
                            stream.CopyTo(file);
                            Console.WriteLine($"Resized: {file.Name} {file.Length / 1000}kB \n");
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"Skipping Resize, copying file {photoName}\n");
            if (!File.Exists(outputDirectory + photoName + "-resized.jpg"))
            {
                File.Copy(workingDirectory + @"\" + photoName + ".jpg", outputDirectory + photoName + "-resized.jpg");
            }
        }
        return new FileInfo(outputDirectory + photoName + "-resized.jpg");
    }

    public IEnumerable<string> ProcessImages(string workingDirectory)
    {
        ResizerConfig = ResizerConfig.WithWorkingDirectory(workingDirectory);
        var outputDirectory = workingDirectory + @"\Resized\";
        Directory.CreateDirectory(outputDirectory);
            
        var photos = Directory.EnumerateFiles(workingDirectory, "*.jpg");

        if (!photos.Any()) 
        {
            Console.WriteLine($"No images to process in {workingDirectory}");
            return new List<string>() { $"No images to process in {workingDirectory}" };
        }


        foreach (var photo in photos)
        {
            var photoName = Path.GetFileNameWithoutExtension(photo);
            var fi = new FileInfo(photo);
            Console.WriteLine($"Photo: {photo} {fi.Length / 1000}kB");

            if (fi.Length / 1000 > ResizerConfig.MaxImageSizeInKiloBytes)
            {
                using (var image = Image.FromFile(photo)) 
                {
                    using (var stream = DownscaleImage(image))
                    {
                        // TODO if does not exist
                        using (var file = File.Create(outputDirectory + photoName + "-resized.jpg"))
                        {
                            stream.CopyTo(file);
                            Console.WriteLine($"Resized: {file.Name} {file.Length / 1000}kB \n");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Skipping Resize, copying file {photoName}\n");
                // TODO if does not exist
                File.Copy(workingDirectory + @"\"+ photoName + ".jpg", outputDirectory + photoName + "-resized.jpg");
            }
        }
        var outputPhotos = Directory.EnumerateFiles(outputDirectory, "*.jpg");
        return outputPhotos;
    }
    
    private MemoryStream DownscaleImage(Image photo)
    {
        MemoryStream resizedPhotoStream = new MemoryStream();

        long resizedSize = 0;
        var quality = 93;
        long lastSizeDifference = 0;
        do
        {
            resizedPhotoStream.SetLength(0);

            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            photo.Save(resizedPhotoStream, ici, eps);
            resizedSize = resizedPhotoStream.Length / 1000;

            long sizeDifference = resizedSize - ResizerConfig.MaxImageSizeInKiloBytes;
            //Console.WriteLine(resizedSize + "(" + sizeDifference + " " + (lastSizeDifference - sizeDifference) + ")");
            lastSizeDifference = sizeDifference;
            quality--;

        } while (resizedSize > ResizerConfig.MaxImageSizeInKiloBytes);

        resizedPhotoStream.Seek(0, SeekOrigin.Begin);

        return resizedPhotoStream;
    }
    
    private ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        var encoders = ImageCodecInfo.GetImageEncoders().Where(p => p.MimeType == mimeType).ToList();
        return encoders.FirstOrDefault() ?? throw new InvalidOperationException($"Unable to find encoder for {mimeType}");
    }
}