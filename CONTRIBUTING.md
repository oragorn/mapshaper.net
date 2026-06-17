# Contributing

Thank you for helping improve Mapshaper.Net.

## Before you start

- Check existing issues and pull requests to avoid duplicate work.
- Keep changes focused on one bug fix, feature, or documentation improvement.
- Remember that this project is a thin wrapper around the external mapshaper CLI. New behavior should usually compose or pass through mapshaper commands rather than reimplementing mapshaper itself.

## Development setup

Install the .NET SDK, Node.js, and the external mapshaper CLI:

```shell
npm install -g mapshaper
```

Verify the CLI is available:

```shell
mapshaper -v
```

Restore, build, and test the repository:

```shell
dotnet restore
dotnet build
dotnet test
```

## Pull requests

- Add or update tests when changing behavior.
- Update `README.md` or XML documentation when public APIs change.
- Update `CHANGELOG.md` for user-visible changes.
- Make sure `dotnet test` passes before opening the pull request.

## Coding guidelines

- Follow the style already used in the repository.
- Keep public APIs small and predictable.
- Prefer explicit argument validation for public entry points.
- Do not add a dependency unless it is needed by the wrapper itself.

## Reporting bugs

Include:

- The Mapshaper.Net version.
- The mapshaper CLI version from `mapshaper -v`.
- The operating system and .NET version.
- A minimal code sample or command sequence that reproduces the problem.
- Relevant `StandardOutput`, `StandardError`, and exit code values.
