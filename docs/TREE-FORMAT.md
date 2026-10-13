# CLBFC TREE Format Specification

- **Format name:** CLBFC TREE
- **Extension:** `.tree`
- **Specification version:** 1.0
- **Maintainer:** MaxFlinker
- **Reference implementation:** clbfc

## 1. Purpose

CLBFC TREE is a plain-text format for representing file and directory
hierarchies using ASCII or Unicode tree connectors.

## 2. Encoding

Files SHOULD use UTF-8 text. Each entry occupies one line.

## 3. Syntax

The standard tree connectors are:

- `├──` introduces an entry with subsequent siblings.
- `└──` introduces the last entry at a given level.
- `│` represents a continuing parent branch.
- Indentation and branch prefixes define hierarchy.

A directory name MAY end with `/` to identify it explicitly as a directory.
A file name SHOULD be identified by its position as a leaf entry or by an
explicit file convention.

Implementations MUST support nesting beyond two directory levels.

## 4. Example

```text
mfboxsys.App/
├── Data/
│   ├── Repositories/
│   │   ├── ElementRepository.cs
│   │   └── FrameRepository.cs
├── Storage/
│   └── ContentStore.cs
└── mfboxsys.App.csproj

5. Execution
A TREE structure can be entered or opened in the FI editor and executed
with Ctrl+R. The CLI also supports the FI input mode.
The output directory determines where the resulting structure is created.
6. Path safety
Implementations MUST reject paths that escape the selected output directory.
Implementations SHOULD reject absolute paths and invalid path components.
7. Compatibility
This document defines version 1.0 of the CLBFC TREE format. Future
incompatible changes SHOULD use a new major specification version.
