﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Settings
{
    public class AwsSettings
    {
        public string ServiceURL { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set;}
        public string BucketName { get; set;}
    }
}
