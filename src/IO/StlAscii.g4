grammar StlAscii;

NAME : [a-zA-Z0-9\-_]+ ;
FLOAT : [-+]?[0-9]* '.'? [0-9]+('e' [-+]?[0-9]+)? ;
WHITESPACE : ('\r'? '\n' | '\t' | ' ' | '\f')+ -> skip ;

solid
    : 'solid' NAME facet+ 'endsolid' NAME EOF
    | 'solid' facet+ 'endsolid' EOF
    ;

facet
    : 'facet' normal loop 'endfacet'
    ;

normal
    : 'normal' FLOAT FLOAT FLOAT
    ;

loop
    : 'outer loop' vertex vertex vertex 'endloop'
    ;

vertex
    : 'vertex' FLOAT FLOAT FLOAT
    ;
