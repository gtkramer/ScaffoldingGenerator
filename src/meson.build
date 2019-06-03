antlr4_bin = find_program('antlr4')
parser_target = custom_target('parser',
  input : ['StlAscii.g4'],
  output : ['StlAsciiLexer.cpp', 'StlAsciiLexer.h', 'StlAsciiParser.cpp', 'StlAsciiParser.h'],
  command : [antlr4_bin, '-o', '@OUTDIR@', '-Dlanguage=Cpp', '@INPUT@']
)

antlr4_dep = dependency('antlr4-runtime', version : '>=4.7.0')
cli11_incdir = include_directories('/usr/include/CLI')
executable('amsg', parser_target, 'amsg.cpp', dependencies : antlr4_dep, include_directories : cli11_incdir)
