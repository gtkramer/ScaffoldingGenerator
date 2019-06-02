antlr4 = find_program('antlr4')
parser = custom_target('parser',
  input : ['StlAscii.g4'],
  output : ['StlAsciiBaseListener.cpp', 'StlAsciiBaseListener.h', 'StlAsciiLexer.cpp', 'StlAsciiLexer.h', 'StlAsciiListener.cpp', 'StlAsciiListener.h', 'StlAsciiParser.cpp', 'StlAsciiParser.h'],
  command : [antlr4, '-o', '@OUTDIR@', '-Dlanguage=Cpp', '@INPUT@']
)

antlr4_dep = dependency('antlr4-runtime', version : '>=4.7.0')
executable('amsg', parser, 'parser.cpp', dependencies : antlr4_dep)
