using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Enum
{
    public enum ErrorCodes
    {
        FileEntityNotFound = 0,
        FileEntitiesNotFound = 1,
        FileEntityAlreadyExists = 2,
    }
}
