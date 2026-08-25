# QLIETextPatch (VNTextPatch Custom Fork)

This is a heavily stripped-down, highly optimized fork of the original VNTextPatch tool from [arcusmaximus/VNTranslationTools](https://github.com/arcusmaximus/VNTranslationTools). It has been customized **exclusively for the QLIE engine (.s files)** and modernized to build with the .NET 10.0 SDK.

All other engine support, spreadsheet formats (Excel/Google Docs), and the C++ VNTextProxy have been intentionally removed to make the tool fast and completely standalone for QLIE translation workflows.

## Features

- **QLIE Only**: Explicitly handles QLIE `.s` scripts without any fragile auto-detection logic.
- **JSON Translation Files**: Extracts and inserts translation text strictly using standard `.json` files.
- **Custom Word Wrapping**: Currently auto-wraps lines to fit rendering of MS Gothic font. 
  - Half-width characters (ASCII, half-width Katakana) are calculated as 1 width unit.
  - Full-width characters (Kanji, Hiragana, etc.) are calculated as 2 width units.
  - Safely treats full-width commas (`，`) as valid line-break boundaries so Japanese punctuation spaces wrap naturally.

## Usage

You can use the tool from the command line to extract the original Japanese text into JSON files, and then insert your translated JSON text back into a patched `.s` script.

### Extracting Text
```cmd
VNTextPatch extractlocal <input.s or input_folder> <output.json or output_folder>
```
*Extracts all dialogue and character names from the QLIE script(s) into a JSON file for translation.*

### Inserting Text
```cmd
VNTextPatch insertlocal <input.s or input_folder> <translated.json or json_folder> <patched.s or patched_folder>
```
*Injects the translated text back into the script. The custom line wrapping logic is automatically applied during this step based on the `MonospaceCharactersPerLine` limit configured in `VNTextPatch.exe.config`.*

## Building from Source

Because this fork drops the legacy C++ tools and UI dependencies, you no longer need the .NET Framework 4.8 Developer Pack or MSBuild.

To compile the tool, simply ensure you have the **.NET 10.0 SDK** installed, and run the included batch script:

```cmd
.\build.bat
```

The resulting executable will be placed in the `Build\VNTextPatch\` directory.

## Character name translation
> [!CAUTION]
> This is broken at the moment.
> 
After running `extractlocal`, VNTextPatch will populate a file called `names.xml` with all the character names it encountered. If you add translations for these names inside this file and run `extractlocal` again, the newly extracted JSON will have the translated names prefilled as a convenience.
