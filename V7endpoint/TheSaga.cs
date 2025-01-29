using System.ComponentModel.DataAnnotations.Schema;
using NServiceBus;
using TheEndpoint.Messages;

namespace TheEndpoint;

public class TheSaga :
    Saga<TheSagaData>,
    IAmStartedByMessages<StartNewSagaSaga>,
    IHandleTimeouts<TheTimeout>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TheSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.Correlation)
            .ToMessage<StartNewSagaSaga>(message => message.Correlation);
    }


    public Task Handle(StartNewSagaSaga message, IMessageHandlerContext context)
    {
        return RequestTimeout(context, TimeSpan.FromMinutes(2), new TheTimeout(){ AComplexType = new ComplexType(){ S = Guid.NewGuid().ToString() }});
    }

    public Task Timeout(TheTimeout state, IMessageHandlerContext context)
    {
        MarkAsComplete();
        Console.WriteLine("The saga has been completed.");
        return Task.CompletedTask;
    }
}

public class TheSagaData : ContainSagaData
{
    public string Correlation { get; set; }
}

public class TheTimeout
{
    public ComplexType AComplexType { get; set; }
}

public class ComplexType
{
    public string S { get; set; }
}