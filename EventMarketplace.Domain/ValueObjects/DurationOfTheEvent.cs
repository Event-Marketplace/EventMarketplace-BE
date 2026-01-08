namespace EventMarketplace.Domain.ValueObjects;

public record DurationOfTheEvent
{
    public DateTime StartEvent { get; }
    public DateTime EndEvent { get; }

    private DurationOfTheEvent(DateTime startEvent, DateTime endEvent)
    {
        StartEvent = startEvent;
        EndEvent = endEvent;
    }

    public static DurationOfTheEvent Create(DateTime startEvent, DateTime endEvent)
    {
        if (startEvent > endEvent)
        {
            throw new ArgumentException("Data rozpoczęcia nie może być większa od daty zakończenia.");
        }
        
        return new DurationOfTheEvent(startEvent, endEvent);
    }

    public DurationOfTheEvent Update(DateTime? start, DateTime? end)
    {
        var newStart = start ?? StartEvent;
        var newEnd = end ?? EndEvent;
        return new DurationOfTheEvent(newStart, newEnd);
    }

    public override string ToString()
    {
        return $"{StartEvent.ToString("dd.MM.yyyy hh:mm")} - {EndEvent.ToString("dd.MM.yyyy hh:mm")}";
    }
}