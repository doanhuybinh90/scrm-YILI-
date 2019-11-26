using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.EntityFrameworkCore;
using Pb.Wechat.YiliMemberTypes;

namespace Pb.Wechat.Migrations.Seed.Scrm
{
    public class DefaultMemberTypesCreator
    {
        public static List<YiliMemberType> InitialMemberTypes => GetInitialMemberTypes();

        private readonly AbpZeroTemplateDbContext _context;

        private static List<YiliMemberType> GetInitialMemberTypes()
        {
            var now = DateTime.Now;
            //var tenantId = AbpZeroTemplateConsts.MultiTenancyEnabled ? null : (int?)1;
            return new List<YiliMemberType>
            {
                new YiliMemberType(){ Name="1.VIP会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="2.活跃会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="3.游离会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="4.似曾相识",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="5.潜在休眠",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="6.按需采购",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="7.新晋会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="8.孕妇会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="9.潜在会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="10.流失会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="11.特殊会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="12.无效会员",CreationTime=now,LastModificationTime=now},
                new YiliMemberType(){ Name="未分类",CreationTime=now,LastModificationTime=now}
                           
            };
        }

        public DefaultMemberTypesCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();
        }

        private void CreateLanguages()
        {
            foreach (var city in InitialMemberTypes)
            {
                AddlMemberTypeIfNotExists(city);
            }
        }

        private void AddlMemberTypeIfNotExists(YiliMemberType city)
        {
            if (_context.YiliMemberTypes.IgnoreQueryFilters().Any(l => l.Name == city.Name))
            {
                return;
            }

            _context.YiliMemberTypes.Add(city);

            _context.SaveChanges();
        }
    }
}