/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace MSR.Data.Entities.Mapping.PathSelectors
{
	[TestFixture]
	public class TakePathByRegExpTest
	{
		private TakePathByRegExp selector;

		[SetUp]
		public void SetUp()
		{
			selector = new TakePathByRegExp();
		}
		[Test]
		public void Should_select_file()
		{
			selector.RegExp = @"/dir/file(\d+)";

			selector.InSelection("/dir/file1")
				.Should().Be.True();
			selector.InSelection("/dir/file2")
				.Should().Be.True();
			selector.InSelection("/dir/file")
				.Should().Be.False();
		}
	}
}
