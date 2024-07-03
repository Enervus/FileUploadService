using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Dto
{
    public record FileEntityDto(string Id, string Name, string ProjectId);
}
