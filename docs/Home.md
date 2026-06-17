# Mapshaper.Net Documentation

Mapshaper.Net is a thin .NET wrapper around the external [mapshaper](https://github.com/mbloch/mapshaper) CLI.

This documentation follows conventional GitHub Markdown structure and can be used as repository docs or as GitHub Wiki pages.

## Start here

- [Getting Started](Getting-Started.md): installation, requirements, first commands, and local development.
- [Supported Functions](Supported-Functions.md): supported client helpers, pipeline commands, options, results, and errors.

## Project links

- [Repository README](https://github.com/oragorn/mapshaper.net#readme)
- [Changelog](https://github.com/oragorn/mapshaper.net/blob/main/CHANGELOG.md)
- [Contributing](https://github.com/oragorn/mapshaper.net/blob/main/CONTRIBUTING.md)
- [Release Guide](https://github.com/oragorn/mapshaper.net/blob/main/RELEASE.md)
- [Security Policy](https://github.com/oragorn/mapshaper.net/blob/main/SECURITY.md)
- [Support](https://github.com/oragorn/mapshaper.net/blob/main/SUPPORT.md)

## Package philosophy

Mapshaper.Net does not parse geospatial files or reimplement mapshaper behavior. It starts the external mapshaper executable, passes arguments through predictably, and captures process output so .NET applications can inspect success, errors, standard output, and standard error.
