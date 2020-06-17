# Scaffolding Generator

Scaffolding Generator is a 3D printing application that generates the
scaffolding required to support an object during production.  It is a rewrite
of an existing Java application in C#.  The motivation behind Scaffolding
Generator is purely academic with the purpose of exploring the complexities of
3D printing and modern 3D rendering pipelines.

The project aims to incorporate the following technologies, data structures,
tool chains, or specifications:

* KD tree (for indexing edges on triangles)
* OpenGL (for rendering objects)
* ANTLR (for writing grammars that parse STL files)
* xUnit and Moq (for testing)
* Glade/Gtk# (for user interface on Linux)

Motivation for implementation choices:

* Use modern build and test systems
* Learn interesting aspects of computing
* Separate user interface from logic
