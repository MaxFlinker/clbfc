# BatchFileCreator — Version 1.11.0

**Command Line Batch File Creator Interface (`clbfc`)**

A tool for creating structured file and directory hierarchies from textual TREE representations and MASK definitions.

## 🇩🇪 Deutsch

### clbfc 1.11.0

Version 1.11.0 is the current development version. It includes work on the FI-mode editor and command-line argument handling. Final release status depends on successful build and regression testing.

### Installation

Install the published version as a global .NET tool:

```powershell
dotnet tool install --global clbfc
```

Update an existing installation:

```powershell
dotnet tool update --global clbfc
```

Check the installed version and available commands:

```powershell
clbfc --help
```

> Install or update to version 1.11.0 only after that version has been published to NuGet.

### Dateiformate

- [MASK-Format](docs/MASK-FORMAT.md)
- [TREE-Format](docs/TREE-FORMAT.md)

### Entwicklungsgeschichte

**Version 1.10.0 — Stabilitätsrelease, 11. Oktober 2026**

Version 1.10.0 continued the work on predictable path handling, TREE parsing and FI-mode output. The aim was to ensure that generated files and directories are placed in the selected destination rather than unexpected build directories.

**Historische Version 1.09**

The v1.09 development notes recorded improvements to output-path handling, automatic directory creation, TREE parsing, nested structures, file/directory collisions and FI-mode output.

These entries describe the historical release notes and do not, by themselves, constitute a guarantee that every edge case is resolved in later versions.

---

## 🇬🇧 English

### Installation

Install `clbfc` as a global .NET tool:

```powershell
dotnet tool install --global clbfc
```

Update an existing installation:

```powershell
dotnet tool update --global clbfc
```

Display the command-line help:

```powershell
clbfc --help
```

### File format specifications

- [MASK format](docs/MASK-FORMAT.md)
- [TREE format](docs/TREE-FORMAT.md)

### Release history

**Version 1.10.0 — Stability release, 11 October 2026**

The v1.10.0 release continued work on path handling, TREE parsing and FI-mode output.

**Version 1.11.0 — Development**

FI-mode editor and command-line handling changes are being tested. Publication should follow a successful build and regression test.

---

## 🇳🇱 Nederlands

### Installatie

Installeer `clbfc` als globale .NET-tool:

```powershell
dotnet tool install --global clbfc
```

Werk een bestaande installatie bij:

```powershell
dotnet tool update --global clbfc
```

Bekijk de beschikbare opdrachten:

```powershell
clbfc --help
```

### Bestandsformaatspecificaties

- [MASK-formaat](docs/MASK-FORMAT.md)
- [TREE-formaat](docs/TREE-FORMAT.md)

### Releasegeschiedenis

**Versie 1.10.0 — Stabiliteitsrelease, 11 oktober 2026**

Deze release zette het werk voort aan padverwerking, TREE-parsing en FI-mode-uitvoer.

**Versie 1.11.0 — In ontwikkeling**

Wijzigingen aan de FI-mode-editor en de verwerking van opdrachtregelargumenten worden getest. Publicatie volgt na een succesvolle build en regressietest.

---

## Project principles

- Predictable output paths
- Clear separation between files and directories
- Structured TREE and MASK input
- Consistent command-line behavior
- Stability before publication

**Maintainer:** MaxFlinker  
**Project:** BatchFileCreator / clbfc  
**License:** See the repository's LICENSE file.