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
    struct facet {
        std::vector<float> normal;
        std::vector<std::vector<float>> vertices;
    };

    struct solid {
        std::vector<facet> facets;
    };
}

BOOST_FUSION_ADAPT_STRUCT(
    amsg::facet,
    (std::vector<float>, normal)
    (std::vector<std::vector<float>>, vertices)
)

BOOST_FUSION_ADAPT_STRUCT(
    amsg::solid,
    (std::vector<amsg::facet>, facets)
)

namespace amsg
{
    template<typename Iterator>
    struct stltext_parser : qi::grammar<Iterator, solid(), ascii::space_type>
    {
        stltext_parser() : stltext_parser::base_type(solid_rule)
        {
            using ascii::no_case;
            using qi::lit;
            using qi::omit;
            using qi::float_;
            using qi::lexeme;
            using ascii::char_;
            
            normal %= no_case[lit("normal")] >> float_ >> float_ >> float_;
            vertex %= no_case[lit("vertex")] >> float_ >> float_ >> float_;
            outer_loop %= no_case[lit("outer loop")] >> vertex >> vertex >> vertex >> no_case[lit("endloop")];
            facet_rule %= no_case[lit("facet")] >> normal >> outer_loop >> no_case[lit("endfacet")];
            solid_rule %= no_case[lit("solid")] >> +facet_rule >> no_case[lit("endsolid")];
        }
        
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> normal;
        qi::rule<Iterator, std::vector<float>(), ascii::space_type> vertex;
        qi::rule<Iterator, std::vector<std::vector<float>>(), ascii::space_type> outer_loop;
        qi::rule<Iterator, facet(), ascii::space_type> facet_rule;
        qi::rule<Iterator, solid(), ascii::space_type> solid_rule;
    };
}

int main()
{
    std::string textfile;
    std::ifstream myfile("/home/george/Documents/Projects/C++/amsg/samples/text.stl", std::ios::in);
    if (myfile.is_open()) {
        std::stringstream sstr;
        sstr << myfile.rdbuf();
        myfile.close();
        textfile = sstr.str();
    }
    std::cout << textfile << std::endl;
    std::string::const_iterator iter = textfile.begin();
    std::string::const_iterator end = textfile.end();
    amsg::stltext_parser<std::string::const_iterator> grammar;
    using ascii::space;
    amsg::solid model;
    bool result = qi::phrase_parse(iter, end, grammar, space, model);
    if (result && iter == end) {
        std::cout << "Parsing succeeded!" << std::endl;
        amsg::facet facet = model.facets.at(0);
        std::cout << "Normal: " << std::endl;
        for (float num : facet.normal) {
            std::cout << num << std::endl;
        }
        std::cout << "Vertices" << std::endl;
        for (std::vector<float> vertex : facet.vertices) {
            std::cout << "Vertex" << std::endl;
            for (float num : vertex) {
                std::cout << num << std::endl;
            }
        }
        return 0;
    }
    else {
        std::cerr << "Parsing stopped here: " << *iter << std::endl;
        std::cerr << "Parsing failed!" << std::endl;
        return 1;
    }
}