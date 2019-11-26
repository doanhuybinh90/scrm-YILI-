using Pb.Wechat.EntityFrameworkCore;

namespace Pb.Wechat.Migrations.Seed.Scrm
{
    public class InitialScrmDbBuilder
    {
        private readonly AbpZeroTemplateDbContext _context;

        public InitialScrmDbBuilder(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultOfficialCitysCreator(_context).Create();
            new DefaultOrganizeCitysCreator(_context).Create();
            new DefaultMemberTypesCreator(_context).Create();
            new DefaultLastBuyProductsCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
