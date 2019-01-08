# Sitecore.LogAnalyzer.MemoryDiagnostics

The repo provides intergation between Sitecore Log Analyzer UI and Memory Diagnostics to display memory analysis results.

## How?

Sitecore Log Analyzer supports module-driven architecture, so it is enough to register module in `Sitecore.LogAnalyzer.config` to get the magic rolling.

## Why?

Log Analyzer UI already has rich UI out of the box:
- Group messages by levels of interest 
- Group similar messages together
- Support hierarchical display
- Data filtering
- Raw view

I have not met a single developer that enjoys developing UI, if you are one - do reach me :)

## How to start?

Please refer to `Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection.BaseMemoryDumpAnalysisModule` class comments - I've spend countless nights writting most user-friendly description in my life.

### Architecture in a few words

While `BaseMemoryDumpAnalysisModule` registers tons of dependencies, they mainly could be divided into a few groups:
- Log Analyzer specific - how to group and locate captions for your data
- Memory Dump reader - what information to read (heap, stack, live, dead) and from were (translate IConnection into `ClrRuntime`)
- Translate `ClrObject` into `LogEntry` Log Analyzer view - the truth is we'll use `ClrObject` -> `IModelMapping` -> `ClrLogEntry`

### Examples

Please feel free to review existing modules implementation to get the idea how it is to be done.

### End goal

Whenever you've got a repeatable scenario to locate certain bits from snapshot, you'll be able to create a module dealing with the job.

A few examples here:

- Fetch `showconfig` from process memory
- Fetch cache statistics 
- Fetch processed requests and their duration
- Show logs buffer (messages that are about to be flushed)
- Fetch SQL queries 

Should you have good ideas, feel free to contribute.

### Technical debts

Niether Sitecore Log Analyzer, nor Memory Diagnostics assemblies are published to nuget - thus we had to copy-paste those, my deepest appologies.

Secondly, the current version of Memory Diagnostics is super-sensitive to field namings, thus whenever field name changes, everything explodes.

### Sample registration

You should add module definition into 'Sitecore.LogAnalyzer.config' by sample :

  <modules>

    <add name="Your Module Name from DI" assembly="Sitecore.SCLA.HTMLCacheViewer">
      <invoker type="Sitecore.SCLA.HTMLCacheViewer.Invoker, Sitecore.SCLA.HTMLCacheViewer"></invoker>
    </add>
