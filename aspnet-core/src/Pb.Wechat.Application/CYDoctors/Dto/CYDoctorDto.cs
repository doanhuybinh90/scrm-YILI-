using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Pb.Wechat.CYDoctors.Dto
{
    [AutoMap(typeof(CYDoctor))]
    public class CYDoctorDto : EntityDto
    {
        public string CYId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string LevelTitle { get; set; }
        public string Clinic { get; set; }
        public string ClinicNO { get; set; }
        public string Hospital { get; set; }
        public string HospitalGrand { get; set; }
        public string GoodAt { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
