using FileUploadService.Application.Mapping;
using FileUploadService.Application.Services;
using FileUploadService.Application.Validations.FluentValidations.FileEntityValidations;
using FileUploadService.Domain.Dto;
using FileUploadService.Domain.Interfaces.Services;
using FileUploadService.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FileEntityMapping));
            services.InitServices();
        }

        private static void InitServices(this IServiceCollection services)
        {
            services.AddScoped<IFileEntityValidator, FileEntityValidator>();
            services.AddScoped<IValidator<UploadFileEntityDto>, UploadFileEntityValidator>();

            services.AddScoped<IFileEntityService, FileEntityService>();
        }
    }
}
