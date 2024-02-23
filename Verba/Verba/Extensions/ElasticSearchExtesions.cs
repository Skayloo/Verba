using Nest;
using Verba.Stock.Domain.ModelsForElastic.Entities;

namespace Verba.Stock.Core.Extensions;

public static class ElasticSearchExtesions
{
    public static void AddElasticSearch(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var url = configuration["ELKConfiguration:Uri"];

        var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson();

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client);
    }


    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings.DefaultMappingFor<Entity>(x => x.IndexName("entity"));

        //settings.DefaultMappingFor<SuspiciousPersons>(x => x.IndexName("suspicious_persons"));

        //settings.DefaultMappingFor<Persons>(x => x.IndexName("un_persons"));
    }

    private static void CreateIndex(IElasticClient client)
    {
        if (!client.Indices.Exists("entity").Exists)
            client.Indices.Create("entity", i => i
                .Map<Entity>(x => x
                    .AutoMap()));

        //if (!client.Indices.Exists("suspicious_persons").Exists)
        //    client.Indices.Create("suspicious_persons", i => i
        //         .Map<SuspiciousPersons>(x => x.AutoMap()));

        //if (!client.Indices.Exists("un_persons").Exists)
        //    client.Indices.Create("un_persons", i => i
        //         .Map<Persons>(x => x.AutoMap()));

        //if (!client.Indices.Exists("un_entity").Exists)
        //    client.Indices.Create("un_entity", i => i
        //    .Settings(s => s
        //        .Analysis(a => a
        //            .Normalizers(n => n.Custom("case_insensitive", c => c.Filters("lowercase")))))
        //        .Map<Entity>(x => x
        //            .Properties(p => p
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("FirstName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("NameOriginalScript"))
        //                )));

        //if (!client.Indices.Exists("suspicious_persons").Exists)
        //    client.Indices.Create("suspicious_persons", x => x
        //    .Settings(s => s
        //        .Analysis(a => a
        //            .Normalizers(n => n.Custom("case_insensitive", c => c.Filters("lowercase")))))
        //         .Map<SuspiciousPersons>(x => x
        //            .Properties(p => p
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("FirstName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("SecondName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("ThirdName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("FIO"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("FIOLat"))
        //                )));

        //if (!client.Indices.Exists("un_persons").Exists)
        //    client.Indices.Create("un_persons", x => x
        //    .Settings(s => s
        //        .Analysis(a => a
        //            .Normalizers(n => n.Custom("case_insensitive", c => c.Filters("lowercase")))))
        //         .Map<Persons>(x => x
        //            .Properties(p => p
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("FirstName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("SecondName"))
        //                .Keyword(k => k.Normalizer("case_insensitive").Name("ThirdName"))
        //                )));
    }
}
