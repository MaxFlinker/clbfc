## CLBFC v1.12.3

Command Line Batch File Creator (CLBFC)

### Features
- Interactive FiMode
- Multiline folder structure input
- Mask/script based filesystem generation
- Multiple language support
- Cross-platform .NET application

### Commands
- `-c -fi` Interactive file and folder creation
- `-c -fi -o` Open existing mask or text files
- `-c -fs` Create structures from mask files
- `--lang` Force language selection
- `-version` Show CLBFC version

### Examples

```bash
clbfc -version
clbfc --help
clbfc -c -fi
clbfc -c -fi file1.txt folder1 output
clbfc -c -fs structure.mask output --lang de

Notes
Version 1.12.3 fixes the -version option so it works both on its own and
when combined with -c. The release continues the stabilization of CLBFC.