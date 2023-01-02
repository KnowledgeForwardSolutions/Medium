namespace RefactoredApi.Settings;

public sealed class OutboxSettings
{
   public Int32 BatchSize { get; set; }

   public Int32 PollingInterval { get; set; }
}
