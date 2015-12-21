namespace Localizy.Extensions
{
    public class LocalizationProviderWithOptions : LocalizationProvider
    {
        public LocalizationProviderWithOptions(LocalizyOptions options) : base(options.Assemblies, options.LocalizationDataProvider, options.LocalizationMissingHandler, options.LocalizationStorageProviders )
        {
            CurrentCultureFactory = options.CurrentCultureFactory;
        }
    }
}