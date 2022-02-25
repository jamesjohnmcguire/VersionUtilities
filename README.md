# VersionUtilities

This is a project to help support automatic version updates and other versioning functionality.

## Getting Started

### Installation
#### Git
git clone  https://github.com/jamesjohnmcguire/VersionUtilities

### Usage:

NOTE: Always back up any data you might be modifying.

#### Command line usage:

VersionUpdate: \<version file\>

##### Usage notes:
As of now, this utility only acts on known file types. The current known file types are .cs, .csproj, .css and .php.  There are plans to act upon a generic or unknown file type, such as a version file that only contains a version number.  Coming soon!

For the known file types, it only acts upon a preceding identifier marker. This is because these files may reference other version numbers other than it's own.  For example, csproj files often contain references to packages that also have their own version numbers.  There are plans to make the identifier markers configurable.  But at the moment, they are hard-coded.  Here is a current list of identifier markers:
- AssemblyVersion(" - .cs files
- AssemblyFileVersion(" - .cs files
- AssemblyVersion\< - .cs files
- AssemblyFileVersion\< - .cs files
- FileVersion\< - .cs files
- Version\< - .cs files
- Version: .css files
- @version( .php files
- , .php files

Following after the identifier marker, a recognizable version tag must also be present, to be updated.

## Contributing

Any contributions you make are **greatly appreciated**.  If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

### Process:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding style
Please match the current coding style.  Most notably:  
1. One operation per line
2. Use complete English words in variable and method names
3. Attempt to declare variable and method names in a self-documenting manner


## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

James John McGuire - [@jamesmc](https://twitter.com/jamesmc) - jamesjohnmcguire@gmail.com

Project Link: [https://github.com/jamesjohnmcguire/VersionUtilities](https://github.com/jamesjohnmcguire/VersionUtilities)
