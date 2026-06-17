# Security Policy

## Supported versions

Security updates are provided for the latest released version of Mapshaper.Net.

## Reporting a vulnerability

Please report security issues privately instead of opening a public issue.

If GitHub private vulnerability reporting is enabled for this repository, use that feature from the repository security page. Otherwise, contact the maintainers through the repository owner profile and include:

- A description of the issue.
- Steps to reproduce or a minimal proof of concept.
- The affected version.
- Any known mitigations.

The maintainers will review the report and coordinate a fix when the issue is confirmed.

## Scope

Mapshaper.Net runs an external mapshaper executable with arguments supplied by the calling application. Applications should treat input paths, output paths, and raw command arguments as trusted application data or validate them before passing them to this package.
