#include <iostream>

#include "antlr4-runtime.h"
#include "StlAsciiLexer.h"
#include "StlAsciiParser.h"
#include "CLI.hpp"

int main(int argc, char *argv[]) {
	CLI::App app{"A 3D printing application that creates the scaffolding required to support an object during production"};
	std::string stlFilePath;
	app.add_option("--stl-file", stlFilePath, "Input STL file");
	CLI11_PARSE(app, argc, argv);

	std::ifstream stream;
	stream.open(stlFilePath);

	antlr4::ANTLRInputStream input(stream);
	StlAsciiLexer lexer(&input);
	antlr4::CommonTokenStream tokens(&lexer);
	StlAsciiParser parser(&tokens);

	StlAsciiParser::SolidContext* tree = parser.solid();

	return 0;
}
