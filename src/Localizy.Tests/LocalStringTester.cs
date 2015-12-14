using System;
using System.Collections.Generic;
using Xunit;

namespace Localizy.Tests
{
    public class LocalStringTester
    {
        [Fact]
        public void two_instances_with_the_same_key_should_equal_each_other()
        {
            var x = new LocalString { Key = "foo" };
            var y = new LocalString { Key = "foo" };

            x.ShouldEqual(y);
            y.ShouldEqual(x);
        }

        [Fact]
        public void two_instances_with_the_same_key_should_be_considered_the_same_for_hashing_purposes()
        {
            var x = new LocalString { Key = "foo" };
            var y = new LocalString { Key = "foo" };

            var dict = new Dictionary<LocalString, int> { { x, 0 } };

            dict.ContainsKey(y).ShouldBeTrue();
        }

        [Fact]
        public void read_from_happy_path()
        {
            LocalString.ReadFrom("key=foo").ShouldEqual(new LocalString("key", "foo"));
            
        }


        [Fact]
        public void read_from_happy_path_trims()
        {
            LocalString.ReadFrom("     key=foo   ").ShouldEqual(new LocalString("key", "foo"));

        }

        [Fact]
        public void read_from_sad_path_fails_with_descriptive_error()
        {

            Assert.Throws<ArgumentException>(() =>
            {
                LocalString.ReadFrom("key:foo");
            }).Message.ShouldEqual("LocalString must be expressed as 'value=display', 'key:foo' is invalid");

            
        }

        [Fact]
        public void read_all()
        {
            Assert.Equal(LocalString.ReadAllFrom(@"
a=a-display
b=b-display
c=c-display
"), new[] { new LocalString("a", "a-display"), new LocalString("b", "b-display"), new LocalString("c", "c-display") });
        }
    }
}