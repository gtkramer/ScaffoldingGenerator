project('amsg', 'cpp',
  version : '0.1',
  default_options : ['warning_level=3',
                     'cpp_std=c++14'])

exe = executable('amsg', 'src/parser.cpp',
  install : true)

test('basic', exe)
