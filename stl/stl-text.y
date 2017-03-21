%{

%}

%token SOLID_START
%token SOLID_END
%token FACET_START
%token FACET_END
%token LOOP_START
%token LOOP_END
%token NORMAL
%token VERTEX
%token FLOAT

%%

solid: SOLID_START facet_list SOLID_END

facet_list: facet
 | facet_list
 ;

facet: FACET_START NORMAL FLOAT FLOAT FLOAT loop FACET_END
 ;

loop: LOOP_START vertex vertex vertex LOOP_END
 ;

vertex: VERTEX FLOAT FLOAT FLOAT
 ;

%%