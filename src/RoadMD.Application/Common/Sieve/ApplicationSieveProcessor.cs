using Microsoft.Extensions.Options;
using RoadMD.Domain.Entities;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Common.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, customSortMethods, customFilterMethods)
        {
        }

        /// <inheritdoc />
        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            #region Infraction

            mapper.Property<Infraction>(x => x.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Infraction>(x => x.Description)
                .CanSort()
                .CanFilter();

            mapper.Property<Infraction>(x => x.CategoryId)
                .CanSort()
                .CanFilter();

            #endregion

            #region Infraction Category

            mapper.Property<InfractionCategory>(x => x.Name)
                .CanSort()
                .CanFilter();

            #endregion

            #region Infraction Report

            mapper.Property<InfractionReport>(x => x.InfractionId)
                .CanFilter();

            mapper.Property<InfractionReport>(x => x.ReportCategoryId)
                .CanFilter()
                .CanSort();

            mapper.Property<InfractionReport>(x => x.Description)
                .CanFilter()
                .CanSort();

            #endregion

            #region Report Category

            mapper.Property<ReportCategory>(x => x.Name)
                .CanFilter()
                .CanSort();

            #endregion

            #region Vehicle

            mapper.Property<Vehicle>(x => x.Number)
                .CanFilter()
                .CanSort();

            #endregion

            #region Feedback

            mapper.Property<Feedback>(x => x.Subject)
                .CanSort()
                .CanFilter();

            mapper.Property<Feedback>(x => x.Description)
                .CanSort()
                .CanFilter();

            mapper.Property<Feedback>(x => x.UserEmail)
                .CanSort()
                .CanFilter();

            mapper.Property<Feedback>(x => x.UserName)
                .CanSort()
                .CanFilter();

            #endregion

            return base.MapProperties(mapper);
        }
    }
}