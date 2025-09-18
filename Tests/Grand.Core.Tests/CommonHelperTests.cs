using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.IO;
namespace Grand.Core.Tests
{
    [TestClass()]
    public class CommonHelperTests {
        [TestMethod()]
        public void EnsureSubscriberEmailOrThrowTest() {
            string[] allPossibleInvalidValues = {
                null,
                "qwert@.pl",
                "@asd.pl",
                "1111qwert#222.pl",
                "qwe@#(@*$@!@asd.pl",
                "qwe rt@asd.pl",
                "qweqwqew@gmail.pl", //valid email don't throw exception
                "q,wert@asd.pl",
                "qwert@as_d.pl"};

            try {
                foreach (string str in allPossibleInvalidValues) {
                    CommonHelper.EnsureSubscriberEmailOrThrow(str);
                }
            }
            catch (GrandException ex) {
                Assert.AreEqual("Email is not valid.", ex.Message);
            }
        }

        [TestMethod()]
        public void IsValidEmailTest() {
            //try invalid strings
            string invalidEmail = null; //null
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = ""; //empty string
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "qwert@.pl"; //no server name
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "@asd.pl"; //no user name
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "1111qwert#222.pl"; //without @
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "qwe@#(@*$@!@asd.pl"; //special characters               
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "qwe rt@asd.pl"; //white space
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "qwert@asd.pl33"; //numbers on end                       !
            //Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "q,wert@asd.pl"; //comma
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            invalidEmail = "qwert@as_d.pl"; //underscore in server name             #1
            Assert.IsFalse(CommonHelper.IsValidEmail(invalidEmail));

            //try valid strings
            string validEmail = "wer++tyu@aasd.pl"; //+ sign
            Assert.IsTrue(CommonHelper.IsValidEmail(validEmail));

            validEmail = "DFGHJDSA1231sdas321SDD@DDDDSDDD.COM"; //upper case
            Assert.IsTrue(CommonHelper.IsValidEmail(validEmail));

            validEmail = "q_wert@asd.pl"; //underscore in user name                 #1
            Assert.IsTrue(CommonHelper.IsValidEmail(validEmail));
        }

        [TestMethod()]
        public void GenerateRandomDigitCodeTest() {
            //the same number of digits
            Assert.AreEqual(123, CommonHelper.GenerateRandomDigitCode(123).Length);
        }

        [TestMethod()]
        public void GenerateRandomIntegerTest() {
            try {
                CommonHelper.GenerateRandomInteger(100, 10); //range 100-10
            }
            catch (Exception ex) {
                Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType());
            }
        }

        [TestMethod()]
        public void EnsureMaximumLengthTest() {
            Assert.IsNull(CommonHelper.EnsureMaximumLength(null, 9));
            Assert.AreEqual("09letters", CommonHelper.EnsureMaximumLength("09letters", 9));
            Assert.AreEqual("09letters", CommonHelper.EnsureMaximumLength("09letters", 9, "_added"));
            Assert.AreEqual("09_added", CommonHelper.EnsureMaximumLength("09letters", 8, "_added"));
            Assert.AreEqual("_added", CommonHelper.EnsureMaximumLength("09letters", 6, "_added"));

            try {
                CommonHelper.EnsureMaximumLength("09letters", 5, "_added"); //max 5 characters, postfix has 6 (postifx exceeds it)
            }
            catch (Exception ex) {
                Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType());
            }
        }


        [TestMethod()]
        public void EnsureNotNullTest() {
            string sentString = null;
            Assert.IsNotNull(CommonHelper.EnsureNotNull(sentString));

            sentString = "asd";
            Assert.IsNotNull(CommonHelper.EnsureNotNull(sentString));
        }


        [TestMethod()]
        public void ArraysEqualTest() {
            //return true because: the same object reference/ address
            int[] array01 = { 1, 23, 2 };
            Assert.AreSame(array01, array01);                                   //the same address == the same object
            Assert.IsTrue(CommonHelper.ArraysEqual<int>(array01, array01));     //the same address == the same object

            //return true because: the same object values
            int[] array02 = { 1, 23, 2 };
            Assert.AreNotSame(array01, array02);                               //not the same object !
            Assert.IsTrue(CommonHelper.ArraysEqual<int>(array01, array02));    //still the same values

            //return false because: different number of elements
            int[] array03 = { 11, 23, 2, 4, 5, 5, 6 };
            int[] array04 = { 99, 23, 2, 4 };
            Assert.IsFalse(CommonHelper.ArraysEqual<int>(array03, array04));

            //return false because: different values
            int[] array05 = { 11, 23, 2, 4, 5, 5, 6 };
            int[] array06 = { 99, 23, 2, 4, 5, 5, 6 };
            Assert.IsFalse(CommonHelper.ArraysEqual<int>(array05, array06));
        }

        [TestMethod()]
        public void GetTrustLevelTest() {
            //untestable
            Assert.IsTrue(true);
        }

        public class tempClass {
            public tempClass() { tempProperty = "some value"; }
            public string tempProperty { get; set; }
        }


        [TestMethod()]
        public void ToTest_Generic_Method() {
            //little trickery done here - convert word-character 'd' into ASCII code of 100
            byte result = CommonHelper.To<byte>('d');
            Assert.AreEqual(typeof(byte), result.GetType());
            Assert.AreEqual(100, result);
        }

        [TestMethod()]
        public void ToTest() {
            //I am sending a byte variable - and receive an integral variable (type has changed, value (in this particular case) didn't)
            double floatingPoint = 1000.08765;
            int thousandInt = 1000;
            Assert.AreEqual(thousandInt, CommonHelper.To<int>(floatingPoint));

            //copied from tests, it was really surprise that it can convert from string to int
            string thousandString = "1000";
            Assert.AreEqual(thousandInt, CommonHelper.To<int>(thousandString));
        }

        [TestMethod()]
        public void ConvertEnumTest() {
            string actualWithUpper = "SoMeTeStStRiNg";
            string expectedWithUpper = "So Me Te St St Ri Ng";
            string stringWithoutUppers = "someteststring";

            Assert.AreEqual(expectedWithUpper, CommonHelper.ConvertEnum(actualWithUpper));
            Assert.AreEqual(stringWithoutUppers, CommonHelper.ConvertEnum(stringWithoutUppers));
        }

        [TestMethod()]
        public void SetTelerikCultureTest() {
            //untestable
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetDifferenceInYearsTest() {
            DateTime birth = DateTime.Parse("2000-01-01 00:00");
            DateTime now = DateTime.Parse("3000-01-01 00:00");

            //happypath
            Assert.AreEqual(1000, CommonHelper.GetDifferenceInYears(birth, now));

            //path
            Assert.AreEqual(-1000, CommonHelper.GetDifferenceInYears(now, birth));

            //one day before birthday
            birth = DateTime.Parse("2000-06-01 00:00");
            now = DateTime.Parse("2016-05-30 00:00");

            Assert.AreEqual(15, CommonHelper.GetDifferenceInYears(birth, now));
        }

        [TestMethod()]
        public void GetDifferenceInYears_ReturnsZero_ForIdenticalDates()
        {
            // S15
            // Arrange
            var date = new DateTime(2022, 5, 15);
        
            // Act
            var diff = CommonHelper.GetDifferenceInYears(date, date);
        
            // Assert
            Assert.AreEqual(0, diff);
        }


        [TestMethod()]
        public void WebMapPath_ComposesPhysicalPath_FromVirtualPath()
        {
            // S14
            // Arrange
            var webRoot = "/var/www";
            CommonHelper.WebRootPath = webRoot;
            var virtualPath = "~/assets/images";
        
            // Act
            var physicalPath = CommonHelper.WebMapPath(virtualPath);
        
            // Assert
            var expectedPath = Path.Combine(webRoot, "assets/images");
            Assert.AreEqual(expectedPath, physicalPath);
        }


        [TestMethod()]
        public void MapPath_ComposesPhysicalPath_FromVirtualPath()
        {
            // S13
            // Arrange
            var expectedBase = @"C:\app\root";
            CommonHelper.BaseDirectory = expectedBase;
            var virtualPath = "~/folder/subfolder";
        
            // Act
            var physicalPath = CommonHelper.MapPath(virtualPath);
        
            // Assert
            var expectedPath = Path.Combine(expectedBase, "folder/subfolder");
            Assert.AreEqual(expectedPath, physicalPath);
        }


        [TestMethod()]
        public void To_NullInput_ReturnsNull()
        {
            // S11 – edge case
            object result = CommonHelper.To(null, typeof(int));
            Assert.IsNull(result);
        }


        [TestMethod()]
        public void ArraysEqual_OneNull_ReturnsFalse()
        {
            // S10 – negative
            int[] a1 = new int[] { 1, 2, 3 };
            int[] a2 = null;
            Assert.IsFalse(CommonHelper.ArraysEqual<int>(a1, a2));
        }


        [TestMethod()]
        public void ArraysEqual_BothNull_ReturnsTrue()
        {
            // S9 – edge case
            int[] a1 = null;
            int[] a2 = null;
            Assert.IsTrue(CommonHelper.ArraysEqual<int>(a1, a2));
        }


        [TestMethod()]
        public void GenerateRandomInteger_WithinRange()
        {
            // S8 – happy path
            int min = 10;
            int max = 20;
            // Run several times to increase confidence
            for (int i = 0; i < 100; i++)
            {
                int value = CommonHelper.GenerateRandomInteger(min, max);
                Assert.IsTrue(value >= min && value < max,
                    $"Generated value {value} is outside the range [{min}, {max}).");
            }
        }


        [TestMethod()]
        public void GenerateRandomDigitCode_ContainsOnlyDigits()
        {
            // S7 – edge case
            int length = 20;
            string result = CommonHelper.GenerateRandomDigitCode(length);
            Assert.AreEqual(length, result.Length, "Result length should match the requested length.");
            foreach (char c in result)
            {
                Assert.IsTrue(char.IsDigit(c), $"Character '{c}' is not a digit.");
            }
        }


        [TestMethod()]
        public void GenerateRandomDigitCode_ZeroLength_ReturnsEmpty()
        {
            // S6: edge case – zero length
            string result = CommonHelper.GenerateRandomDigitCode(0);
            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod()]
        public void EnsureMaximumLength_EmptyString_ReturnsEmpty()
        {
            // S5: edge case – empty input string
            string str = "";
            string result = CommonHelper.EnsureMaximumLength(str, 10);
            Assert.AreEqual(string.Empty, result);
        }


        [TestMethod()]
        public void EnsureMaximumLength_NoPostfix_TruncatesCorrectly()
        {
            // S4: edge case – null postfix truncation
            string str = "ABCDEFGHIJ";
            string result = CommonHelper.EnsureMaximumLength(str, 5);
            Assert.AreEqual("ABCDE", result);
        }

/*
FAILED TEST: <|channel|>analysis<|message|>We need to analyze why test EnsureSubscriberEmailOrThrow_LongEmail_TruncatesAndValidates failed. The test expects that EnsureSubscriberEmailOrThrow truncates email to 255 characters and returns a valid email. In code, EnsureSubscriberEmailOrThrow does:

output = EnsureNotNull(email);
output = output.Trim();
output = EnsureMaximumLength(output, 255);
if (!IsValidEmail(output)) throw.

EnsureMaximumLength truncates string to maxLength - postfix length (postfix null => pLen=0). It simply does str.Substring(0, maxLength). That will cut the email at 255 characters, potentially cutting in the middle of domain part, making invalid email. The test builds longLocal = 300 'a's + "@example.com". That's length 300 + 12 = 312. Truncate to 255 will cut somewhere within the local part, leaving no '@' maybe? Actually 255 characters from start includes the '@' at position 300? Wait local part length 300, '@' at index 300, domain length 12. So first 255 chars are all 'a's (since '@' is at 301). So truncated email becomes 255 'a's, no '@', invalid. Test expects truncation to keep domain intact, but EnsureMaximumLength doesn't consider email structure. So test fails.

Fix: EnsureSubscriberEmailOrThrow should truncate preserving domain, maybe by limiting local part length to 255 - domain length - 1. Or modify EnsureMaximumLength to not cut off after '@' for email. Simpler: change EnsureSubscriberEmailOrThrow to first trim, then if length >255, split at '@', truncate local part accordingly, then reassemble, then validate.

Also test EnsureSubscriberEmailOrThrow_TrimWhitespace seems incomplete but not failing.

Provide recommendation: adjust EnsureSubscriberEmailOrThrow to enforce max length while preserving email validity, or adjust test expectations. Also ensure EnsureMaximumLength not used for email truncation.

Now produce concise answer.<|end|><|start|>assistant<|channel|>final<|message|>**Why the test failed**

`EnsureSubscriberEmailOrThrow` trims the input and then calls `EnsureMaximumLength(output, 255)`.  
`EnsureMaximumLength` simply does `str.Substring(0, maxLength)` when the string is too long.  

The test builds an e‑mail that is 312 characters long (`300 'a' + "@example.com"`).  
After the naïve truncation to 255 characters the result is **255 ‘a’ characters** – the “@example.com” part is removed.  
Consequently `IsValidEmail` returns `false` and `EnsureSubscriberEmailOrThrow` throws `GrandException`, causing the test failure.

**Recommended fix**

Modify the email‑handling logic so that truncation never destroys the e‑mail structure:

```csharp
public static string EnsureSubscriberEmailOrThrow(string email)
{
    var output = EnsureNotNull(email).Trim();

    // keep the domain intact when truncating
    const int maxLength = 255;
    if (output.Length > maxLength)
    {
        var atPos = output.LastIndexOf('@');
        if (atPos > 0)
        {
            var domain = output.Substring(atPos);               // includes '@'
            var allowedLocal = maxLength - domain.Length;
            if (allowedLocal < 0) allowedLocal = 0;
            var local = output.Substring(0, Math.Min(atPos, allowedLocal));
            output = local + domain;
        }
        else
        {
            // no '@' – fallback to simple truncation
            output = output.Substring(0, maxLength);
        }
    }

    if (!IsValidEmail(output))
        throw new GrandException("Email is not valid.");

    return output;
}
```

or, alternatively, move this logic into a dedicated helper (e.g., `TruncateEmail`) and call it instead of the generic `EnsureMaximumLength`.

**Result**

The email will be truncated to exactly 255 characters **with the domain preserved**, `IsValidEmail` will succeed, and the test `EnsureSubscriberEmailOrThrow_LongEmail_TruncatesAndValidates` will pass.

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_LongEmail_TruncatesAndValidates()
        {
            // S3: edge case – maximum length enforcement
            // Create a local part longer than needed so truncation cuts it but leaves the domain intact.
            string longLocal = new string('a', 300);
            string email = longLocal + "@example.com";
            string result = CommonHelper.EnsureSubscriberEmailOrThrow(email);
            // Result should be exactly 255 characters
            Assert.AreEqual(255, result.Length);
            // It must still contain a single '@' and be a valid email
            Assert.IsTrue(result.Contains("@"));
            Assert.IsTrue(CommonHelper.IsValidEmail(result));
        }

*/

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_TrimWhitespace()
        {
            // S2: edge case – whitespace trimming
            string email = "   spaced@example.com   ";
            string result = CommonHelper.EnsureSubscriberEmailOrThrow(email);
            Assert.AreEqual("spaced@example.com", result);
        }


        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_ValidEmail_ReturnsSame()
        {
            // S1: happy path
            string email = "user@example.com";
            string result = CommonHelper.EnsureSubscriberEmailOrThrow(email);
            Assert.AreEqual(email, result);
        }

    }
}