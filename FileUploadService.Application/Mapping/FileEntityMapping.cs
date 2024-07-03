using AutoMapper;
using FileUploadService.Domain.Dto;
using FileUploadService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Application.Mapping
{
    public class FileEntityMapping: Profile
    {
        public FileEntityMapping()
        {
            CreateMap<FileEntity,FileEntityDto>()
                .ForCtorParam("Id",o=>o.MapFrom(s=>s.Id))
                .ForCtorParam("Name",o=>o.MapFrom(s=>s.Name))
                .ForCtorParam("ProjectId",o=>o.MapFrom(s=>s.ProjectId))
                .ReverseMap();
        }
    }
}
