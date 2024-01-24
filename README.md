# Scaffolding Generator

Scaffolding Generator is a 3D printing program that generates the scaffolding
required to support a 3D model while it's being printed.  The motivation behind
Scaffolding Generator is purely academic with the purpose of exploring the
complexities of 3D printing and modern 3D rendering pipelines.  It is
ultimately intended to be a fun experiment, not something to be used in a
production environment.

The C# program is a rewrite of an existing Java program.  Doing a rewrite more
easily allows the program to take advantage of modern technologies and to
improve upon the clarity of the original implementation.

This project aims to incorporate the following technologies, data structures,
tool chains, idea, or specifications:

* .NET Core, for developing in a C# implementation on all platforms
* KD tree, for indexing edges on triangles
* OpenGL, for rendering objects
* ANTLR, for writing grammars that parse STL files
* Avalonia, for a user interface definition separated from logic
* Parallelize work whenever possible, for making things faster
