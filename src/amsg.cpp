#include "antlr4-runtime.h"
#include "StlAsciiLexer.h"
#include "StlAsciiParser.h"
#include "CLI.hpp"

std::string getFileContents(std::string filePath) {
	std::ifstream stream;
	stream.open(filePath);
	std::ostringstream content;
	content << stream.rdbuf();
	stream.close();
	return content.str();
}

int main(int argc, char *argv[]) {
	std::string stlFilePath;

	CLI::App app{"A 3D printing application that creates the scaffolding required to support an object during production"};
	app.add_option("-s,--stl-file", stlFilePath, "Input STL file");
	CLI11_PARSE(app, argc, argv);

	std::string stlFile = getFileContents(stlFilePath);
	std::transform(stlFile.begin(), stlFile.end(), stlFile.begin(), ::tolower);

	antlr4::ANTLRInputStream input(stlFile);
	StlAsciiLexer lexer(&input);
	antlr4::CommonTokenStream tokens(&lexer);
	StlAsciiParser parser(&tokens);

	StlAsciiParser::SolidContext* tree = parser.solid();

	return 0;
}
