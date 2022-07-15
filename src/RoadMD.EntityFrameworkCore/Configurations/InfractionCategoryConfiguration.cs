using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class InfractionCategoryConfiguration : BaseEntityConfiguration<InfractionCategory>
    {
        public override void Configure(EntityTypeBuilder<InfractionCategory> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            builder.HasData(new InfractionCategory
            {
                Id = new Guid("5A4A7870-E95C-4443-883D-D2126A5EFA04"),
                Name = "Conducerea unui vehicul fără număr de înmatriculare",
            }, new InfractionCategory
            {
                Id = new Guid("4940A717-ECA6-4A9E-9374-A34A5F46E279"),
                Name = "Amplasarea ilegală pe vehicul a unui număr de înmatriculare fals"
            }, new InfractionCategory
            {
                Id = new Guid("FBE7C0C8-FCDA-4314-860D-D3E1F2C29CD7"),
                Name = "Depăşirea vitezei de circulaţie stabilită",
            }, new InfractionCategory
            {
                Id = new Guid("796B8081-BB01-45F2-A76E-0D7A03250914"),
                Name = "Oprirea în locuri interzise"
            }, new InfractionCategory
            {
                Id = new Guid("D7D5AC06-54B1-49A3-9D1F-539E5A48CCD9"),
                Name = "Staţionarea sau parcarea în locuri interzise"
            });

            base.Configure(builder);
        }
    }
}