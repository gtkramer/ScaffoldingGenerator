#include <boost/config/warning_disable.hpp>
#include <boost/spirit/include/qi.hpp>
#include <boost/spirit/include/phoenix_core.hpp>
#include <boost/spirit/include/phoenix_operator.hpp>
#include <boost/spirit/include/phoenix_fusion.hpp>
#include <boost/fusion/include/adapt_struct.hpp>

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <iterator>

namespace qi = boost::spirit::qi;
namespace ascii = boost::spirit::ascii;

namespace amsg {
    struct Facet {
        std::vector<float> normal;
        std::vector<std::vector<float>> vertices;
    };
}

BOOST_FUSION_ADAPT_STRUCT(
    amsg::Facet,
    (std::vector<float>, normal)
    (std::vector<std::vector<float>>, vertices)
)

namespace amsg {
    template<typename Iterator> struct StlTextParser : qi::grammar<Iterator, std::vector<Facet>(), ascii::space_type> {
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> vertex;
        qi::rule<Iterator, std::vector<std::vector<float>>(), ascii::space_type> outerLoop;
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> normal;
        qi::rule<Iterator, Facet(), ascii::space_type> facet;
        qi::rule<Iterator, std::string()> name;
        qi::rule<Iterator, std::vector<Facet>(), ascii::space_type> solid;

        StlTextParser() : StlTextParser::base_type(solid) {
            vertex %= ascii::no_case[qi::lit("vertex")] >> qi::repeat(3)[qi::float_];
            outerLoop %= ascii::no_case[qi::lit("outer loop")] >> qi::repeat(3)[vertex] >> ascii::no_case[qi::lit("endloop")];
            normal %= ascii::no_case[qi::lit("normal")] >> qi::repeat(3)[qi::float_];
            facet %= ascii::no_case[qi::lit("facet")] >> normal >> outerLoop >> ascii::no_case[qi::lit("endfacet")];
            name %= +ascii::char_("a-zA-Z_0-9");
            solid %= ascii::no_case[qi::lit("solid")] >> qi::omit[name] >> +facet >> ascii::no_case[qi::lit("endsolid")] >> qi::omit[name] | ascii::no_case[qi::lit("solid")] >> +facet >> ascii::no_case[qi::lit("endsolid")];
        }
    };

    template<typename Iterator> struct StlBinaryParser : qi::grammar<Iterator, std::vector<Facet>()> {
        qi::rule<Iterator, std::vector<float>()> quantity;
        qi::rule<Iterator, std::vector<std::vector<float>>()> grouping;
        qi::rule<Iterator, Facet()> facet;
        qi::rule<Iterator, std::vector<Facet>()> solid;

        StlBinaryParser() : StlBinaryParser::base_type(solid) {
            quantity %= qi::repeat(3)[qi::little_bin_float];
            grouping %= qi::repeat(3)[quantity];
            facet %= quantity >> grouping >> qi::omit[qi::word];
            solid %= qi::omit[qi::repeat(10)[qi::qword] >> qi::dword] >> +facet;
        }
    };
}

void printSolid(std::vector<amsg::Facet> facets) {
    for (amsg::Facet facet : facets) {
        std::cout << "FACET" << std::endl;
        std::cout << "=====" << std::endl;
        std::cout << "Normal: " << std::endl;
        for (float num : facet.normal) {
            std::cout << num << std::endl;
        }
        std::cout << "Vertices" << std::endl;
        std::cout << "--------" << std::endl;
        for (std::vector<float> vertex : facet.vertices) {
            std::cout << "Vertex:" << std::endl;
            for (float num : vertex) {
                std::cout << num << std::endl;
            }
        }
    }
}

void parseTextFile() {
    std::ifstream inputFile("/home/george/Documents/Projects/C++/amsg/samples/text.stl", std::ios::in);
    if (inputFile.is_open()) {
        inputFile.seekg(0, std::ios::end);
        int length = inputFile.tellg();
        inputFile.seekg(0, std::ios::beg);
        std::cout << "Length of file: " << length << std::endl;

        std::string buffer;
        buffer.resize(length);
        inputFile.read(&buffer[0], length);
        inputFile.close();

        std::string::const_iterator begin = buffer.begin();
        std::string::const_iterator end = buffer.end();
        amsg::StlTextParser<std::string::const_iterator> grammar;
        std::vector<amsg::Facet> facets;
        bool result = qi::phrase_parse(begin, end, grammar, ascii::space, facets);
        if (result && begin == end) {
            std::cout << "Parsing SUCCEEDED!" << std::endl;
            printSolid(facets);
        }
        else {
            std::cout << "Parsing FAILED!" << std::endl;
            int iterCounts = 0;
            while (begin != end) {
                begin++;
                iterCounts++;
            }
            std::cout << "Number of iterations left on buffer: " << iterCounts << std::endl;
        }
        std::cout << "Number of facets parsed: " << facets.size() << std::endl;
    }
}

void parseBinaryFile() {
    std::ifstream inputFile("/home/george/Documents/Projects/C++/amsg/samples/binary.stl", std::ios::in | std::ios::binary);
    if (inputFile.is_open()) {
        inputFile.seekg(0, std::ios::end);
        int length = inputFile.tellg();
        inputFile.seekg(0, std::ios::beg);
        std::cout << "Length of file: " << length << std::endl;

        std::vector<char> buffer(length);
        inputFile.read(&buffer[0], length);
        inputFile.close();

        std::vector<char>::const_iterator begin = buffer.begin();
        std::vector<char>::const_iterator end = buffer.end();
        amsg::StlBinaryParser<std::vector<char>::const_iterator> grammar;
        std::vector<amsg::Facet> facets;
        bool result = qi::parse(begin, end, grammar, facets);
        if (result && begin == end) {
            std::cout << "Parsing SUCCEEDED!" << std::endl;
            printSolid(facets);
        }
        else {
            std::cout << "Parsing FAILED!" << std::endl;
            int iterCounts = 0;
            while (begin != end) {
                begin++;
                iterCounts++;
            }
            std::cout << "Number of iterations left on buffer: " << iterCounts << std::endl;
        }
        std::cout << "Number of facets parsed: " << facets.size() << std::endl;
    }
}

int main() {
    //parseBinaryFile();
    parseTextFile();
    return 0;
}
