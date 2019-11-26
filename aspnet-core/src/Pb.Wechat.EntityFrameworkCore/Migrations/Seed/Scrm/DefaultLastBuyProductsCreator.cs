using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.EntityFrameworkCore;
using Pb.Wechat.YiliLastBuyProducts;

namespace Pb.Wechat.Migrations.Seed.Scrm
{
    public class DefaultLastBuyProductsCreator
    {
        public static List<YiliLastBuyProduct> InitialLastBuyProducts => GetInitialLastBuyProducts();

        private readonly AbpZeroTemplateDbContext _context;

        private static List<YiliLastBuyProduct> GetInitialLastBuyProducts()
        {
            var now = DateTime.Now;
            //var tenantId = AbpZeroTemplateConsts.MultiTenancyEnabled ? null : (int?)1;
            return new List<YiliLastBuyProduct>
            {
                new YiliLastBuyProduct(){Code="246001000210", Name="金装婴儿配方奶粉（听装）1×6×900g",IsDeleted=true,CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246301000610", Name="金领冠0段900g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246701000210", Name="金领冠1段900g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246702000210", Name="金领冠2段900g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246703000210", Name="金领冠3段900g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="247101000310", Name="珍护3段800g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="247101000210", Name="珍护2段800g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="247101000110", Name="珍护1段800g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246404000210", Name="欣活配方奶粉1×6×900g（心活配方）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245904000510", Name="金领冠4段900g听",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245904004910", Name="呵护1段（商超版）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245904005010", Name="呵护2段（商超版）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245904005110", Name="呵护3段（商超版）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246901000510", Name="欣活配方奶粉1×6×900g（忆利配方）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="246901000610", Name="欣活配方奶粉1×6×900g（骨能配方）",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245911000110", Name="培然1段",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245911000210", Name="培然2段",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245911000310", Name="培然3段",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245907001110", Name="珍护1段900g",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245907001210", Name="珍护2段900g",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245907001310", Name="珍护3段900g",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245912000510", Name="金领冠睿护2段900g",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245912000410", Name="金领冠睿护1段900g",CreationTime=now,LastModificationTime=now},
                new YiliLastBuyProduct(){Code="245912000610", Name="金领冠睿护3段900g",CreationTime=now,LastModificationTime=now}
                
            };
        }

        public DefaultLastBuyProductsCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLastBuyProducts();
        }

        private void CreateLastBuyProducts()
        {
            foreach (var city in InitialLastBuyProducts)
            {
                AddlLastBuyProductIfNotExists(city);
            }
        }

        private void AddlLastBuyProductIfNotExists(YiliLastBuyProduct city)
        {
            if (_context.YiliLastBuyProducts.IgnoreQueryFilters().Any(l => l.Name == city.Name))
            {
                return;
            }

            _context.YiliLastBuyProducts.Add(city);

            _context.SaveChanges();
        }
    }
}
