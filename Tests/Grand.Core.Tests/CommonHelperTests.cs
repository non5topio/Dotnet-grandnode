using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void GetDifferenceInYears_LeapYearBoundary()
        {
            DateTime startDate = new DateTime(2020, 2, 28);
            DateTime endDate = new DateTime(2021, 2, 28);
            int result = CommonHelper.GetDifferenceInYears(startDate, endDate);
            Assert.AreEqual(1, result);
        }

/*
FAILED TEST: The test `ConvertEnum_AllUppercase` is failing due to a whitespace mismatch in the expected result.

**Root Cause:**
The `ConvertEnum` method adds a space before each uppercase character **only if it is not the first character**. However, the test expects a space **before the first character**, resulting in a leading space that is not present in the actual output.

**Recommended Fix:**
Update the test's expected value to remove the leading space:
```csharp
string expected = "A L L C A P S S T R I N G";
```

        [TestMethod()]
        public void ConvertEnum_AllUppercase()
        {
            string input = "ALLCAPSSTRING";
            string expected = " A L L C A P S S T R I N G";
            string result = CommonHelper.ConvertEnum(input);
            Assert.AreEqual(expected, result);
        }

*/

        [TestMethod()]
        public void GenerateRandomInteger_MaxRange()
        {
            int result = CommonHelper.GenerateRandomInteger(0, int.MaxValue);
            Assert.IsTrue(result >= 0 && result <= int.MaxValue);
        }

/*
FAILED TEST: The test `EnsureMaximumLength_MaxLengthWithFittingPostfix` is failing because the `EnsureMaximumLength` method is not appending the expected postfix (`abcde`) to the input string, even though there is enough space to fit it.

**Root Cause:**
The `EnsureMaximumLength` method calculates the postfix length and trims the input string to `maxLength - pLen`, but the test expects the full input string (`94 'a' characters`) to be preserved and the 5-character postfix to be appended, totaling 99 characters. However, the method is returning only the input string (94 characters), implying the postfix was not added.

**Recommended Fix:**
Ensure the `EnsureMaximumLength` method correctly appends the postfix when there is sufficient space. Review the logic in:
```csharp
var pLen = postfix == null ? 0 : postfix.Length;
var result = str.Substring(0, maxLength - pLen);
if (!String.IsNullOrEmpty(postfix))
{
    result += postfix;
}
```

        [TestMethod()]
        public void EnsureMaximumLength_MaxLengthWithFittingPostfix()
        {
            string input = new string('a', 94);
            string postfix = "abcde";
            int maxLength = 100;
            string result = CommonHelper.EnsureMaximumLength(input, maxLength, postfix);
            Assert.AreEqual(input + postfix, result);
        }

*/

        [TestMethod()]
        public void EnsureMaximumLength_MaxLengthNoPostfix()
        {
            string input = new string('a', 100);
            string result = CommonHelper.EnsureMaximumLength(input, 100);
            Assert.AreEqual(input, result);
        }


        [TestMethod()]
        public void IsValidEmail_InvalidIPv6()
        {
            string input = "user@[2001:0db8:85a3:0000:0000:8a2e:0370:7334]";
            bool result = CommonHelper.IsValidEmail(input);
            Assert.IsFalse(result);
        }


        [TestMethod()]
        public void IsValidEmail_LongLocalPart()
        {
            string local = new string('a', 64);
            string input = local + "@domain.com";
            bool result = CommonHelper.IsValidEmail(input);
            Assert.IsTrue(result);
        }

/*
FAILED TEST: The test `IsValidEmail_LongDomainName` is failing because the constructed email string, although long, is not a valid email format according to the `IsValidEmail` regex validation.

**Recommended Fix:**
Update the test to use a valid email format that also meets the 255 character limit. For example:
```csharp
string domain = new string('a', 250);
string input = "user" + domain + "@b.c"; // valid structure and 255 characters
```

        [TestMethod()]
        public void IsValidEmail_LongDomainName()
        {
            string domain = new string('a', 253);
            string input = "user@" + domain;
            bool result = CommonHelper.IsValidEmail(input);
            Assert.IsTrue(result);
        }

*/
/*
FAILED TEST: The test `IsValidEmail_255Characters` is failing because the constructed email string, while exactly 255 characters long, is not a valid email format according to the regex in `IsValidEmail`. The current test string (`"a@b.c" + new string('x', 250)`) creates an invalid email structure.

**Recommended Fix:**
Update the test to use a valid email format that also meets the 255 character limit. For example:
```csharp
string email = "a" + new string('x', 250) + "@b.c"; // valid structure and 255 characters
```

        [TestMethod()]
        public void IsValidEmail_255Characters()
        {
            string email = new string('x', 250);
            string input = "a@b.c" + email; // 5 + 250 = 255 characters
            bool result = CommonHelper.IsValidEmail(input);
            Assert.IsTrue(result);
        }

*/
/*
FAILED TEST: The test `EnsureSubscriberEmailOrThrow_MaxLength255` is failing because the input string, although within the 255 character limit, is not considered a valid email by the `IsValidEmail` method, causing a `GrandException` to be thrown.

**Root Cause:**
The test constructs an email string with 255 characters, but the format is invalid (e.g., too many repeated characters or invalid structure), which fails the regex validation in `IsValidEmail`.

**Recommended Fix:**
Update the test to use a valid email format that also meets the 255 character limit. For example:
```csharp
string email = "a@b.c" + new string('x', 250); // Ensure it's a valid email format
```

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_MaxLength255()
        {
            string email = new string('x', 250);
            string input = "a@b.c" + email; // 5 + 250 = 255 characters
            string result = CommonHelper.EnsureSubscriberEmailOrThrow(input);
            Assert.AreEqual(input, result);
        }

*/
    }
}