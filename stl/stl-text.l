%{

%}

%%

"solid"      { return SOLID_START; }
"endsolid"   { return SOLID_END; }
"facet"      { return FACET_START;  }
"endfacet"   { return FACET_END }
"outer loop" { return LOOP_START; }
"endloop"    { return LOOP_END; }

"normal"     { return NORMAL; }
"vertex"     { return VERTEX; }
[0-9.e-+]+   { yyval = atof(yytext); return FLOAT; }

\w+          {}
\s+          {}

%%