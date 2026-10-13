# CLBFC MASK Format Specification

- **Format name:** CLBFC MASK
- **Extension:** `.mask`
- **Specification version:** 1.0
- **Maintainer:** MaxFlinker
- **Reference implementation:** clbfc

## 1. Purpose

CLBFC MASK is a plain-text format for describing file and directory
structures to be created by the Command Line Batch File Creator Interface
(clbfc).

## 2. Encoding

Files SHOULD use UTF-8 text. Each entry occupies one line.

## 3. Syntax

Each non-empty line describes one directory or file.

- A line ending in `/` describes a directory.
- A line containing `.` that does not end in `/` describes a file.
- Nested paths MAY be expressed using directory separators.
- Empty lines are ignored.
- Entries are processed in the order in which they appear.

## 4. Example

```text
MyProject/
MyProject/src/
MyProject/src/Program.cs
MyProject/docs/
MyProject/docs/README.md
MyProject/MyProject.csproj

5. Execution
A MASK file can be executed using:
clbfc -c -fs structure.mask


An explicit output directory can be supplied:
clbfc -c -fs structure.mask output


If no output directory is specified, the current working directory is used.
6. Validation
Entries that cannot be classified as directories or files are reported as
unclear entries.
Implementations SHOULD validate paths and prevent entries from escaping the
selected output directory. Users SHOULD review MASK files from untrusted
sources before execution.
7. Compatibility
This document defines version 1.0 of the CLBFC MASK format. Incompatible
future changes SHOULD use a new major specification version.
