using Xunit;

namespace Localizy.Tests
{
    
    public class StringTokenStronglyTypedTester
    {
        public StringTokenStronglyTypedTester()
        {
            LocalizationManager.Init();
        }

        [Fact]
        public void simple_localization()
        {
            var token = Loc.Basic;
            var locKey = token.ToLocalizationKey();
            Assert.Equal("Loc:Basic", locKey.ToString());
            Assert.Equal("Basic_Trans", token.DefaultValue);
        }

        [Fact]
        public void nested_class_localization()
        {
            var token = Loc.Account.AccountName;
            var locKey = token.ToLocalizationKey();
            Assert.Equal("Loc.Account:AccountName", locKey.ToString());
            Assert.Equal("Account_Trans", token.DefaultValue);
        }

        [Fact]
        public void parameter_localization()
        {
            var token = Loc.User.NameWithParams;
            var locKey = token.ToLocalizationKey();
            Assert.Equal("Loc.User:NameWithParams", locKey.ToString());
            Assert.Equal("Name_Sir", token.FormatTokenWith(new NameParams { LastName = "Sir" }));
        }

        [Fact]
        public void parameter_localization_raw_key()
        {
            var token = Loc.User.NameWithParams;
            Assert.Equal("Name_{LastName}", token.ToRawString());
        }
    }

    public partial class Loc
    {
        public static readonly StringToken Basic = new StringToken<Loc>("Basic_Trans");

        public class User
        {
            public static readonly StringToken Name = new StringToken<User>("Name_Trans");
            public static readonly StringToken<User, NameParams> NameWithParams = new StringToken<User, NameParams>("Name_{LastName}");
        }
    }

    public class NameParams
    {
        public string LastName { get; set; }
    }

    public partial class Loc
    {
        public class Account
        {
            public static readonly StringToken AccountName = new StringToken<Account>("Account_Trans");
        }
    }
}
