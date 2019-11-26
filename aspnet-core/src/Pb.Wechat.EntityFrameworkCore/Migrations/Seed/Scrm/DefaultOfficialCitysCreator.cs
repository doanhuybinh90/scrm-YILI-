using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.EntityFrameworkCore;
using Pb.Wechat.YiliOfficialCitys;

namespace Pb.Wechat.Migrations.Seed.Scrm
{
    public class DefaultOfficialCitysCreator
    {
        public static List<YiliOfficialCity> InitialOfficialCitys => GetInitialOfficialCitys();

        private readonly AbpZeroTemplateDbContext _context;

        private static List<YiliOfficialCity> GetInitialOfficialCitys()
        {
            var now = DateTime.Now;
            //var tenantId = AbpZeroTemplateConsts.MultiTenancyEnabled ? null : (int?)1;
            return new List<YiliOfficialCity>
            {
                new YiliOfficialCity(){Id=1, Name="中国",ParentCode=0,IsDeleted=true,CreationTime=now,LastModificationTime=now},
                 new YiliOfficialCity(){ Name="安徽省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                 new YiliOfficialCity(){ Name="澳门特别行政区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                  new YiliOfficialCity(){ Name="北京市",ParentCode=1,CreationTime=now,LastModificationTime=now},
                   new YiliOfficialCity(){ Name="福建省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                    new YiliOfficialCity(){ Name="甘肃省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                     new YiliOfficialCity(){ Name="广东省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                      new YiliOfficialCity(){ Name="广西壮族自治区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                       new YiliOfficialCity(){ Name="贵州省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                        new YiliOfficialCity(){ Name="海南省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                         new YiliOfficialCity(){ Name="河北省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                          new YiliOfficialCity(){ Name="河南省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                           new YiliOfficialCity(){ Name="黑龙江省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                            new YiliOfficialCity(){ Name="湖北省",ParentCode=1,CreationTime=now,LastModificationTime=now},

                             new YiliOfficialCity(){ Name="湖南省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                              new YiliOfficialCity(){ Name="吉林省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                               new YiliOfficialCity(){ Name="江苏省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                new YiliOfficialCity(){ Name="江西省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                 new YiliOfficialCity(){ Name="辽宁省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                  new YiliOfficialCity(){ Name="内蒙古自治区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                   new YiliOfficialCity(){ Name="宁夏回族自治区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                    new YiliOfficialCity(){ Name="青海省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                     new YiliOfficialCity(){ Name="山东省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                      new YiliOfficialCity(){ Name="山西省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                       new YiliOfficialCity(){ Name="陕西省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                        new YiliOfficialCity(){ Name="上海市",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                         new YiliOfficialCity(){ Name="四川省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                          new YiliOfficialCity(){ Name="台湾省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                           new YiliOfficialCity(){ Name="天津市",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                            new YiliOfficialCity(){ Name="西藏自治区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                             new YiliOfficialCity(){ Name="香港特别行政区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                              new YiliOfficialCity(){ Name="新疆维吾尔自治区",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                               new YiliOfficialCity(){ Name="云南省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                                new YiliOfficialCity(){ Name="浙江省",ParentCode=1,CreationTime=now,LastModificationTime=now},
                                                 new YiliOfficialCity(){ Name="重庆市",ParentCode=1,CreationTime=now,LastModificationTime=now}
            };
        }

        public DefaultOfficialCitysCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateOfficialCitys();
        }

        private void CreateOfficialCitys()
        {
            foreach (var city in InitialOfficialCitys)
            {
                AddlOfficialCityIfNotExists(city);
            }
        }

        private void AddlOfficialCityIfNotExists(YiliOfficialCity city)
        {
            if (_context.YiliOfficialCitys.IgnoreQueryFilters().Any(l => l.Name == city.Name))
            {
                return;
            }

            _context.YiliOfficialCitys.Add(city);

            _context.SaveChanges();
        }
    }
}