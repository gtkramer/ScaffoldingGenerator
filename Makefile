all:
	g++ --std=c++14 -Wpedantic parser.cpp -o parser

clean:
	rm -f parser