# Sitecore.LogAnalyzer.MemoryDiagnostics

The repo provides intergation between Sitecore Log Analyzer UI and Memory Diagnostics to display memory analysis results.

## How?

Sitecore Log Analyzer supports module-driven architecture, so it is enough to register module in `Sitecore.LogAnalyzer.config`

## Why?

Log Analyzer UI already has rich UI out of the box:
- Group messages by levels of interest 
- Group similar messages together
- Support hierarchical display
- Data filtering
- Raw view

## How to start?

Please refer to `Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DependencyInjection.BaseMemoryDumpAnalysisModule` class comments - I've spend countless nights writting the user-friendly description.

### Architecture in a few words

While `BaseMemoryDumpAnalysisModule` registers tons of dependencies, they are mainly could be divided into a few groups:
- Log Analyzer specific - how to group and locate captions for your data
- Memory Dump reader - what information to read (heap, stack, live, dead) and from were (translate IConnection into `ClrRuntime`)
- Translate `ClrObject` into `LogEntry` Log Analyzer view - the truth is we'll use `ClrObject` -> `IModelMapping` -> `ClrLogEntry`

### Examples

Please feel free to review existing modules implementation to get the idea how it is to be done.

### End goal

Whenever you've got a repeatable scenario to locate certain bits from snapshot, you'll be able to create a module dealing with the job.
