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
    template<typename Iterator> struct StlTextParser : qi::grammar<Iterator, std::vector<amsg::Facet>(), ascii::space_type> {
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> vertex;
        qi::rule<Iterator, std::vector<std::vector<float>>(), ascii::space_type> outerLoop;
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> normal;
        qi::rule<Iterator, Facet(), ascii::space_type> facet;
        qi::rule<Iterator, std::string()> name;
        qi::rule<Iterator, std::vector<amsg::Facet>(), ascii::space_type> solid;

        StlTextParser() : StlTextParser::base_type(solid) {
            vertex %= ascii::no_case[qi::lit("vertex")] >> qi::float_ >> qi::float_ >> qi::float_;
            outerLoop %= ascii::no_case[qi::lit("outer loop")] >> vertex >> vertex >> vertex >> ascii::no_case[qi::lit("endloop")];
            normal %= ascii::no_case[qi::lit("normal")] >> qi::float_ >> qi::float_ >> qi::float_;
            facet %= ascii::no_case[qi::lit("facet")] >> normal >> outerLoop >> ascii::no_case[qi::lit("endfacet")];
            name %= +ascii::char_("a-zA-Z_0-9");
            solid %= ascii::no_case[qi::lit("solid")] >> qi::omit[-name] >> +facet >> ascii::no_case[qi::lit("endsolid")] >> qi::omit[-name];
        }
    };
}

int main() {
    std::string fileText;
    std::ifstream inputFile("/home/george/Documents/Projects/C++/amsg/samples/text.stl", std::ios::in);
    if (inputFile.is_open()) {
        std::stringstream sstr;
        sstr << inputFile.rdbuf();
        inputFile.close();
        fileText = sstr.str();
    }
    std::cout << fileText << std::endl;

    std::string::const_iterator start = fileText.begin();
    std::string::const_iterator end = fileText.end();
    amsg::StlTextParser<std::string::const_iterator> grammar;
    std::vector<amsg::Facet> facets;
    bool result = qi::phrase_parse(start, end, grammar, ascii::space, facets);
    if (result && start == end) {
        std::cout << "Parsing SUCCEEDED!" << std::endl;
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
        return 0;
    }
    else {
        std::cerr << "Parsing FAILED!" << std::endl;
        std::cerr << "Parsing stopped here: " << *start << std::endl;
        return 1;
    }
}
