using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ProjectBlack.Localization;

public static class ProjectBlackLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(ProjectBlackConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(ProjectBlackLocalizationConfigurer).GetAssembly(),
                    "ProjectBlack.Localization.SourceFiles"
                )
            )
        );
    }
}
