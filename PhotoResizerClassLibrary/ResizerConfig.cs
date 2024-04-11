using Lombok.NET;

namespace PhotoResizerLibrary;

[AllArgsConstructor]
[With]
public partial class ResizerConfig
{
    public long MaxImageSizeInKiloBytes;
    public string WorkingDirectory;
}