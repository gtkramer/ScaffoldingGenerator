AMSG: Additive Manufacturing Scaffolding Generator
==================================================

AMSG is an open source 3D printing application which creates the scaffolding
required to support an object during production.  The motivation behind AMSG is
purely academic with the purpose of learning the complexities of the 3D printing
problem via exploration in programming.

The project aims to incorporate the following technologies, data structures, or
specifications:

* Flex and bison (for scanning and parsing text STL files)
* Interval tree (for indexing edges on triangles)
* KD tree (for indexing vertexes)
* OpenGL 4.5 (for rendering an object)
* Simple DirectMedia Layer 2.0 (for interacting with a render)
* Boost (for linear algebra calculations)

Specific implementations of data structures need evaluated.  Possible candiates
include:

* `nanoflann`_ (for KD tree)


.. _nanoflann:
   https://github.com/jlblancoc/nanoflann