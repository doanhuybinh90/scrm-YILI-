using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.EntityFrameworkCore;
using Pb.Wechat.YiliOrganizeCitys;

namespace Pb.Wechat.Migrations.Seed.Scrm
{
    public class DefaultOrganizeCitysCreator
    {
        public static List<YiliOrganizeCity> InitialOrganizeCitys => GetInitialOrganizeCitys();

        private readonly AbpZeroTemplateDbContext _context;

        private static List<YiliOrganizeCity> GetInitialOrganizeCitys()
        {
            var now = DateTime.Now;
            return new List<YiliOrganizeCity>
            {
                new YiliOrganizeCity(){Code=2, Name="奶粉东北大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){ Code=3,Name="奶粉鲁豫大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=4, Name="奶粉华北大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=5, Name="奶粉西北大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=6, Name="奶粉苏皖大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=7, Name="奶粉华中大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=8, Name="奶粉华南大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=9, Name="奶粉华东大区",ParentCode=0,CreationTime=now,LastModificationTime=now},
                 new YiliOrganizeCity(){Code=0, Name="奶粉西南大区",ParentCode=0,CreationTime=now,LastModificationTime=now}

            };
        }

        public DefaultOrganizeCitysCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();
        }

        private void CreateLanguages()
        {
            foreach (var city in InitialOrganizeCitys)
            {
                AddlOrganizeCityIfNotExists(city);
            }
        }

        private void AddlOrganizeCityIfNotExists(YiliOrganizeCity city)
        {
            if (_context.YiliOrganizeCitys.IgnoreQueryFilters().Any(l => l.Name == city.Name))
            {
                return;
            }

            _context.YiliOrganizeCitys.Add(city);

            _context.SaveChanges();
        }
    }
}