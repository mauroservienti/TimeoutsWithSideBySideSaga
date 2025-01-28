using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using TheEndpoint.Messages;

var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
endpointConfiguration.EnableInstallers();
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
dialect.JsonBParameterModifier(
    modifier: parameter =>
    {
        var npgsqlParameter = (NpgsqlParameter)parameter;
        npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
    });
persistence.ConnectionBuilder(() => new NpgsqlConnection("Server=localhost;Port=5432;Database=db_user;User Id=db_user;Password=your_password;"));
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

await endpointInstance.SendLocal(new StartNewSagaSaga(){ Correlation = Guid.NewGuid().ToString() });
Console.WriteLine($"Saga started. Press any key to stop the endpoint.");
Console.WriteLine($"Waiting 10 seconds before automatically shutting this endpoint down.");
Console.WriteLine($"Start the V8endpoint and wait for the 2-minute timeout to expire.");

await Task.Delay(10000);

await endpointInstance.Stop();
