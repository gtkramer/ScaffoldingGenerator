%{
    enum yytokentype {
        SOLID_START = 258,
        SOLID_END = 259,
        FACET_START = 260,
        FACET_END = 261,
        LOOP_START = 262,
        LOOP_END = 263,
        NORMAL = 264,
        VERTEX = 265,
        FLOAT = 266
    };
    float yyval;
%}

%option noyywrap
%option caseless

%%

"solid"                                 { return SOLID_START; }
"endsolid"                              { return SOLID_END; }
"facet"                                 { return FACET_START; }
"endfacet"                              { return FACET_END; }
"outer loop"                            { return LOOP_START; }
"endloop"                               { return LOOP_END; }
"normal"                                { return NORMAL; }
"vertex"                                { return VERTEX; }

[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?  { yyval = atof(yytext); return FLOAT; }

[a-zA-Z0-9_]+                           {}
[\r\n\t\f\v ]+                          {}

%%

int main(int argc, char **argv) {
    if (argc > 1 && (yyin = fopen(argv[1], "r"))) {
        int tok;
        while (tok = yylex()) {
            printf("%d", tok);
            if (tok == FLOAT) {
                printf(" = %f", yyval);
            }
            printf("\n");
        }
        return 0;
    }
    else {
        fprintf(stderr, "%s", argv[1]);
        return 1;
    }
}