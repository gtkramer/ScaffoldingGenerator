#include "antlr4-runtime.h"
#include "StlAsciiLexer.h"
#include "StlAsciiParser.h"
#include "CLI.hpp"

int main(int argc, char *argv[]) {
	CLI::App app{"A 3D printing application that creates the scaffolding required to support an object during production"};

	std::string ifile;
	app.add_option("--stl-file", ifile, "Input STL file");

	CLI11_PARSE(app, argc, argv);

	std::cout << ifile << std::endl;

	return 0;
}
