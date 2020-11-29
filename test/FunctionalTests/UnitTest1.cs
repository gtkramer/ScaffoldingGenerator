using System;
using Xunit;
using AdditiveManufacturing.IO;
using AdditiveManufacturing.Geometry;

namespace FunctionalTests {
	public class UnitTest1 {
		[Fact]
		public void ReadAscii1() {
			StlReader reader = new StlAsciiReader();
			Facet[] facets = reader.Read("/home/george/Documents/Projects/C#/ScaffoldingGenerator/samples/ascii1.stl");
			Assert.NotEmpty(facets);
		}

		[Fact]
		public void ReadAscii2() {
			StlReader reader = new StlAsciiReader();
			Facet[] facets = reader.Read("/home/george/Documents/Projects/C#/ScaffoldingGenerator/samples/ascii2.stl");
			Assert.NotEmpty(facets);
		}

		[Fact]
		public void ReadBinary() {
			StlReader reader = new StlBinaryReader();
			Facet[] facets = reader.Read("/home/george/Documents/Projects/C#/ScaffoldingGenerator/samples/binary.stl");
			Assert.NotEmpty(facets);
		}

		[Fact]
		public void WriteAscii() {
			StlReader reader = new StlBinaryReader();
			Facet[] facets = reader.Read("/home/george/Documents/Projects/C#/ScaffoldingGenerator/samples/binary.stl");
			StlWriter writer = new StlAsciiWriter();
			writer.Write("/tmp/ascii.stl", facets);
			StlReader asciiReader = new StlAsciiReader();
			asciiReader.Read("/tmp/ascii.stl");
		}

		[Fact]
		public void WriteBinary() {
			StlReader reader = new StlAsciiReader();
			Facet[] facets = reader.Read("/home/george/Documents/Projects/C#/ScaffoldingGenerator/samples/ascii1.stl");
			StlWriter writer = new StlBinaryWriter();
			writer.Write("/tmp/binary.stl", facets);
			StlReader binaryReader = new StlBinaryReader();
			binaryReader.Read("/tmp/binary.stl");
		}
	}
}
