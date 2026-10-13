## CLBFC v1.12.2


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

### Examples

```bash
clbfc -c -fi
clbfc -c -fi file1.txt folder1 output
clbfc -c -fs structure.mask output --lang de

Notes

This release continues the stabilization of CLBFC and introduces the current generation of FiMode and filesystem creation workflows.

