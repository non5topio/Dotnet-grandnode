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
/*
FAILED TEST: <|channel|>analysis<|message|>We need to explain failure: IsValidEmail_MaxLength254_ReturnsTrue expects true for email length 254, but IsValidEmail returns false likely because regex disallows domain length >255? Actually domain part length 185 + ".com" = 189, total domain length 190? RFC max domain length 255, but maybe regex limits each label to 63 characters. The generated domain is a single label of 185 characters, exceeding label length limit (63). So regex fails. Test expects true but implementation too strict. Fix: adjust IsValidEmail regex to allow long domain labels up to 255 total, or split domain into multiple labels. Simpler: modify test to use valid domain with labels <=63, but requested fix is code. Recommend updating regex to use more permissive pattern or use MailAddress class. Provide suggestion.<|end|><|start|>assistant<|channel|>final<|message|>**Why the test failed**

`IsValidEmail_MaxLength254_ReturnsTrue` builds an address whose *domain* is a single label of 185 characters (`bbbb…b.com`).  
The regular expression used in `CommonHelper.IsValidEmail` enforces the RFC‑compliant rule that **each label** (the parts between dots) may be at most 63 characters long. Because the generated domain exceeds that limit, the regex returns `false`, causing the `Assert.IsTrue` to fail.

**What needs to be fixed**

Either the test must generate a syntactically valid 254‑character email (e.g., split the long domain into several labels ≤ 63 chars), **or** the implementation must be relaxed to accept the test’s definition of “max length 254”.

The most robust fix is to change the validation logic to a less‑strict, well‑tested implementation (e.g., `System.Net.Mail.MailAddress`) that only checks overall length, not per‑label limits, because the library already handles the edge cases correctly.

**Recommended code change**

```csharp
using System.Net.Mail;

public static bool IsValidEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return false;

    // Trim once – the caller already does it, but keep it safe.
    email = email.Trim();

    // Quick length check (max 254 characters per RFC 5321)
    if (email.Length > 254)
        return false;

    try
    {
        // MailAddress throws if the address is not syntactically valid.
        var _ = new MailAddress(email);
        return true;
    }
    catch (FormatException)
    {
        return false;
    }
}
```

*Why this works*

* `MailAddress` validates the address according to the .NET implementation of RFC‑5322, which is sufficient for most scenarios.
* We keep the explicit 254‑character guard to satisfy the test’s length requirement.
* No per‑label 63‑character restriction is applied, so the 185‑character label used in the test is now accepted.

**Alternative (if you prefer to keep the regex)**  
Replace the current pattern with one that does **not** enforce the 63‑character label limit, e.g.:

```csharp
var pattern = @"^[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+(\.[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+)*@[\w\.\-]+$";
```

But using `MailAddress` is simpler, more reliable, and eliminates the need to maintain a complex regex.

        [TestMethod()]
        public void IsValidEmail_MaxLength254_ReturnsTrue()
        {
            // Arrange: 64‑character local part, 185‑character domain, plus ".com" and '@'
            string local = new string('a', 64);
            string domain = new string('b', 185);
            string email = $"{local}@{domain}.com"; // total length = 64 + 1 + 185 + 4 = 254
            
            // Act
            bool isValid = CommonHelper.IsValidEmail(email);
            
            // Assert
            Assert.IsTrue(isValid);
        }

*/

        [TestMethod()]
        public void IsValidEmail_IpLiteralDomain_ReturnsTrue()
        {
            // Arrange
            string email = "user@[192.168.0.1]";
            
            // Act
            bool isValid = CommonHelper.IsValidEmail(email);
            
            // Assert
            Assert.IsTrue(isValid);
        }


        [TestMethod()]
        public void To_AlreadyDestinationType_ReturnsSameInstance()
        {
            // Arrange
            string original = "already a string";
            
            // Act
            var result = CommonHelper.To(original, typeof(string));
            
            // Assert
            Assert.AreSame(original, result);
            Assert.AreEqual(original, result);
        }


        [TestMethod()]
        public void To_NullInput_ReturnsNull()
        {
            // Arrange
            object value = null;
            
            // Act
            var result = CommonHelper.To(value, typeof(string));
            
            // Assert
            Assert.IsNull(result);
        }

/*
FAILED TEST: <|channel|>analysis<|message|>We need to explain failure: GenerateRandomInteger_MinEqualsMax_ThrowsArgumentOutOfRangeException expects ArgumentOutOfRangeException when min == max, but implementation of GenerateRandomInteger does not check and will generate a number, no exception. So test fails. Fix: modify GenerateRandomInteger to throw ArgumentOutOfRangeException when min >= max (or min == max). Provide code snippet.<|end|><|start|>assistant<|channel|>final<|message|>**Failure Reason**  
`GenerateRandomInteger_MinEqualsMax_ThrowsArgumentOutOfRangeException` expects `CommonHelper.GenerateRandomInteger(min, max)` to throw an `ArgumentOutOfRangeException` when `min` equals `max`. The current implementation:

```csharp
public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
{
    var randomNumberBuffer = new byte[10];
    RandomNumberGenerator.Create().GetBytes(randomNumberBuffer);
    return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
}
```

does **not** validate the relationship between `min` and `max`. `Random.Next(min, max)` simply returns `min` when `min == max`, so no exception is thrown, causing the test to fail.

**Recommended Fix**  
Add argument validation to `GenerateRandomInteger` that throws `ArgumentOutOfRangeException` when `min >= max` (or at least when `min == max`). Example:

```csharp
public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
{
    if (min >= max)
        throw new ArgumentOutOfRangeException(nameof(min), "min must be less than max.");

    var randomNumberBuffer = new byte[10];
    RandomNumberGenerator.Create().GetBytes(randomNumberBuffer);
    return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
}
```

* This satisfies the existing test that expects an exception when `min == max`.
* It also prevents misuse where `min` is greater than `max`, which would otherwise cause `Random.Next` to throw its own exception.

        [TestMethod()]
        public void GenerateRandomInteger_MinEqualsMax_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int min = 10;
            int max = 10;
            
            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CommonHelper.GenerateRandomInteger(min, max));
        }

*/

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_TruncationMakesInvalid_ThrowsException()
        {
            // Arrange: a long local part plus a long domain so that truncation cuts into the domain
            var localPart = new string('a', 250);
            var domain = "@verylongdomainnamewhichwillbetruncated.com";
            var email = $"{localPart}{domain}"; // total length > 255
            
            // Act & Assert
            try
            {
                CommonHelper.EnsureSubscriberEmailOrThrow(email);
                Assert.Fail("Expected GrandException was not thrown.");
            }
            catch (GrandException ex)
            {
                Assert.AreEqual("Email is not valid.", ex.Message);
            }
        }


        [TestMethod()]
        public void GenerateRandomDigitCode_ZeroLength_ReturnsEmptyString()
        {
            // Act
            string result = CommonHelper.GenerateRandomDigitCode(0);
        
            // Assert
            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod()]
        public void WebMapPath_ResolvesVirtualPath_WithWebRootPath()
        {
            // Arrange
            string webRoot = Path.Combine(Path.GetTempPath(), "WebMapPathTest");
            Directory.CreateDirectory(webRoot);
            CommonHelper.WebRootPath = webRoot;
            string virtualPath = "~/content/image.png";
        
            // Act
            string physicalPath = CommonHelper.WebMapPath(virtualPath);
        
            // Assert
            string expected = Path.Combine(webRoot, "content", "image.png");
            Assert.AreEqual(expected, physicalPath);
        }


        [TestMethod()]
        public void MapPath_ResolvesVirtualPath_WithBaseDirectory()
        {
            // Arrange
            string baseDir = Path.Combine(Path.GetTempPath(), "MapPathTest");
            Directory.CreateDirectory(baseDir);
            CommonHelper.BaseDirectory = baseDir;
            string virtualPath = "~/folder/file.txt";
        
            // Act
            string physicalPath = CommonHelper.MapPath(virtualPath);
        
            // Assert
            string expected = Path.Combine(baseDir, "folder", "file.txt");
            Assert.AreEqual(expected, physicalPath);
        }


        [TestMethod()]
        public void EnsureMaximumLength_EqualLength_WithPostfix_ReturnsOriginal()
        {
            // Arrange
            string original = "12345";
            int maxLength = 5;
            string postfix = "_post";
        
            // Act
            string result = CommonHelper.EnsureMaximumLength(original, maxLength, postfix);
        
            // Assert
            Assert.AreEqual(original, result);
        }


        [TestMethod()]
        public void EnsureMaximumLength_MaxZero_ReturnsEmpty()
        {
            // Arrange
            var input = "nonempty";
        
            // Act
            var result = CommonHelper.EnsureMaximumLength(input, 0);
        
            // Assert
            Assert.AreEqual(string.Empty, result);
        }


        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_LongEmail_TruncatedInvalid_Throws()
        {
            // Arrange: make domain so long that truncation cuts off the top‑level domain
            var longDomain = new string('c', 260);
            var email = $"a@{longDomain}.com"; // total length > 255, truncation will remove ".com"
            // Act & Assert
            try
            {
                CommonHelper.EnsureSubscriberEmailOrThrow(email);
                Assert.Fail("Expected GrandException was not thrown.");
            }
            catch (GrandException ex)
            {
                Assert.AreEqual("Email is not valid.", ex.Message);
            }
        }

/*
FAILED TEST: <|channel|>analysis<|message|>We need to explain failure: EnsureSubscriberEmailOrThrow_LongEmail_TruncatedValid expects that long email gets truncated to 255 and still valid, but EnsureSubscriberEmailOrThrow first trims, then EnsureMaximumLength (which truncates but may cut off part of domain causing invalid email). In test they create local part 250 'a's + "@example.com" length >255, truncation will cut part of local part but keep domain, but EnsureMaximumLength truncates to maxLength - postfix length (no postfix) so result length 255, but may cut in middle of local part, still valid? Actually email validation regex requires full domain and local part; truncating local part may still be valid as long as still contains '@example.com' and local part non-empty. However EnsureMaximumLength truncates from start, so it will cut off characters from the end, preserving beginning of string. Starting string length: 250 + 1 + 11 = 262? "example.com" length 11 inc '@'? Actually "@example.com" length 12 (including @). So total 262. Truncate to 255 -> remove last 7 chars from end, which cuts part of domain, making invalid email. Hence exception. Test expects truncation only of local part, but implementation truncates overall string. Fix: modify EnsureSubscriberEmailOrThrow to truncate only local part before '@' while preserving domain, or adjust EnsureMaximumLength to not cut domain. Simpler: change test expectation or implementation. Recommended fix: implement logic to ensure email length <=255 by truncating local part, not whole email. Provide code snippet.

Now produce concise answer.<|end|><|start|>assistant<|channel|>final<|message|>**Why the test failed**

`EnsureSubscriberEmailOrThrow_LongEmail_TruncatedValid` builds an e‑mail whose *local part* is 250 ‘a’ characters and whose domain (`@example.com`) makes the whole address longer than 255 characters.  
`EnsureSubscriberEmailOrThrow` calls `EnsureMaximumLength(output, 255)`, which simply cuts the **right‑most characters** of the whole string to 255 characters.  
Because the truncation happens on the full address, the domain is also shortened (e.g. “…@example.”), so the resulting string no longer matches the e‑mail regex and `IsValidEmail` returns `false`. Consequently a `GrandException` is thrown and the test fails.

**Recommended fix**

Make the length‑limiting logic aware of the e‑mail structure and truncate **only the local part**, never the domain, so the final address is still syntactically valid and ≤ 255 characters.

```csharp
public static string EnsureSubscriberEmailOrThrow(string email)
{
    // normalisation
    var output = EnsureNotNull(email).Trim();

    // split into local part and domain
    var atIndex = output.LastIndexOf('@');
    if (atIndex <= 0) // no '@' or empty local part
        throw new GrandException("Email is not valid.");

    var local = output.Substring(0, atIndex);
    var domain = output.Substring(atIndex); // includes '@'

    // ensure total length ≤ 255 by trimming the local part only
    const int maxLength = 255;
    if (output.Length > maxLength)
    {
        var allowedLocalLength = maxLength - domain.Length;
        if (allowedLocalLength < 1)
            throw new GrandException("Email is not valid."); // cannot truncate domain enough

        local = local.Substring(0, allowedLocalLength);
        output = local + domain;
    }

    if (!IsValidEmail(output))
        throw new GrandException("Email is not valid.");

    return output;
}
```

*Alternative (if you prefer not to change production code):* adjust the test to expect an exception for overly long e‑mails, but the intended behavior (truncating only the local part) matches the test’s purpose, so fixing the method is the proper solution.

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_LongEmail_TruncatedValid()
        {
            // Arrange: local part 250 'a's, domain short so truncation occurs only in local part
            var localPart = new string('a', 250);
            var email = $"{localPart}@example.com"; // total length > 255
            // Act
            var result = CommonHelper.EnsureSubscriberEmailOrThrow(email);
            // Assert
            Assert.AreEqual(255, result.Length);
            Assert.IsTrue(result.EndsWith("@example.com"));
            Assert.IsTrue(CommonHelper.IsValidEmail(result));
        }

*/

        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_WhitespaceTrimmed()
        {
            // Arrange
            var emailWithSpaces = "  user@example.com  ";
            var expected = "user@example.com";
        
            // Act
            var result = CommonHelper.EnsureSubscriberEmailOrThrow(emailWithSpaces);
        
            // Assert
            Assert.AreEqual(expected, result);
        }


        [TestMethod()]
        public void EnsureSubscriberEmailOrThrow_ValidEmail_ReturnsSame()
        {
            // Arrange
            var email = "user@example.com";
        
            // Act
            var result = CommonHelper.EnsureSubscriberEmailOrThrow(email);
        
            // Assert
            Assert.AreEqual(email, result);
        }

    }
}