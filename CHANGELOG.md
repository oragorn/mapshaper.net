# Changelog

## 0.3.0 - 2026-06-18

### Added

- Added typed `CleanAsync`, `EraseAsync`, `JoinAsync`, `RenameFieldsAsync`, and `InfoAsync` helpers to `MapshaperClient`.
- Added integration coverage that runs against the mapshaper CLI in CI.

### Changed

- Use `ProcessStartInfo.ArgumentList` when available for safer, platform-native command argument handling while retaining compatibility with `netstandard2.0` runtimes.
- Kept the broader mapshaper command surface in `MapshaperPipeline`, with raw argument pass-through for command-specific options.

## 0.2.0 - 2026-06-17

### Added

- Added a fluent `MapshaperPipeline` API for composing multi-command mapshaper workflows.
- Added pipeline wrappers for common mapshaper commands while preserving raw argument pass-through.
- Documented supported client methods, pipeline commands, options, result handling, and runtime requirements.

### Changed

- Clarified runtime setup guidance for the external mapshaper CLI and Node.js.
- Expanded README examples for pipeline usage, options, failure handling, and documentation links.

## 0.1.0 - 2026-06-16

### Changed

- Retargeted the library package to `netstandard2.0` for broader .NET compatibility.

## 0.0.2 - 2026-06-15

### Added

- Modeled common mapshaper import and output flags through `MapshaperCommandOptions`, `MapshaperImportOptions`, and `MapshaperOutputOptions`.
- Documented the thin wrapper philosophy in the README.

### Changed

- Derive NuGet package versions from release tags.
- Add richer NuGet metadata, package icon, release notes, and Source Link support.

## 0.0.1 - 2026-06-15

### Added

- Initial `MapshaperClient` wrapper for running the external mapshaper CLI from .NET.
- Raw argument execution through `RunAsync` and failure-checking execution through `RunOrThrowAsync`.
- Convenience methods for common mapshaper workflows: convert, simplify, clip, dissolve, filter fields, and project.
- `MapshaperOptions` for configuring the mapshaper executable path and working directory.
- `MapshaperResult` and `MapshaperException` types for inspecting exit codes, standard output, standard error, and failed command arguments.
- README documentation with installation notes and usage examples.
- GitHub Actions CI for restore, build, test, and pack.
- NuGet publishing through GitHub Actions Trusted Publishing.
