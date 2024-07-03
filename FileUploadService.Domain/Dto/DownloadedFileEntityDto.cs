using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Dto
{
    public record DownloadedFileEntityDto(string Id, string Name,string FileURL);
}
