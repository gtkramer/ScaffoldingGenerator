grammar StlAscii;

SOLID_BEGIN : 'solid' ;
SOLID_END : 'endsolid' ;
FACET_BEGIN : 'facet' ;
FACET_END : 'endfacet' ;
LOOP_BEGIN : 'outer loop' ;
LOOP_END : 'endloop' ;
NORMAL : 'normal' ;
VERTEX : 'vertex' ;
NAME : [a-zA-Z0-9\-_]+ ;
FLOAT : ([0-9]+ '.' [0-9]+) ;
WHITESPACE : ('\r'? '\n' | '\t' | ' ')+ -> skip ;

expression
	: solid EOF
	;

solid
	: SOLID_BEGIN NAME facet+ SOLID_END NAME
	| SOLID_BEGIN facet+ SOLID_END
	;

facet
	: FACET_BEGIN normal loop FACET_END
	;

normal
	: NORMAL FLOAT FLOAT FLOAT
	;

loop
	: LOOP_BEGIN vertex vertex vertex LOOP_END
	;

vertex
	: VERTEX FLOAT FLOAT FLOAT
	;
