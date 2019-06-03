# AMSG: Additive Manufacturing Scaffolding Generator

AMSG is an open source 3D printing application which creates the scaffolding
required to support an object during production.  The motivation behind AMSG is
purely academic with the purpose of learning the complexities of the 3D
printing problem via exploration in programming.

The project aims to incorporate the following technologies, data structures,
tool chains, or specifications as a focus for phase one:

* Interval tree (for indexing edges on triangles)
* KD tree (for indexing vertexes)
* OpenGL 4.5 (for rendering an object)
* Simple DirectMedia Layer 2.0 (for interacting with a render)
* Boost (for linear algebra calculations between planes and a model, and a general-purpose library)
* CLI11 (for parsing command line options with minimal boilerplate code)
* ANTLR (for writing grammars that parse STL files)
* Meson and Ninja (for building sources)
* Google Test (for unit testing)
* GTK+ and Glade (for user interface)

Motivation for implementation choices:

* Use modern build and test systems
* Learn interesting aspects of computing
* Separate user interface from logic using a native tooklit

In phase two, the focus is
[parallelization](https://software.intel.com/en-us/articles/choosing-the-right-threading-framework)
and optimizing the existing implementation.

In phase three (shooting for the stars), the focus is to achieve feature-parity
with [Magics](http://www.materialise.com/en/software/materialise-magics), an
industry-leading application for 3D printing.
