using System;
using Xunit;
using Xunit.Extensions;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class NonceManagerInitializationTest
    {
        [Fact]
        public void Generate_ThrowsWhenPrivateHashEncoderNotInitialized()
        {
            Assert.Throws<InvalidOperationException>(() => NonceManager.Generate("127.0.0.1"));
        }

        [Fact]
        public void Validate_ThrowsWhenPrivateHashEncoderNotInitialized()
        {
            //this is a valid nonce conforming to our standards
            Assert.Throws<InvalidOperationException>(() => NonceManager.Validate("NjM0MzM4ODg3Nzc1MzEuMzpmZDIxNzllOTUzMDY2ODc2YWQyYjY1NmVmZGJkYTc4MQ==", "127.0.0.1"));
        }
    }

    public class NonceManagerInvalidPrivateHashEncoderTest
    {
        static NonceManagerInvalidPrivateHashEncoderTest()
        {
            PrivateHashEncoder.Current = () => null;        		
        }

        [Fact]
        public void Generate_ThrowsWhenPrivateHashEncoderNotInitialized()
        {
            Assert.Throws<InvalidOperationException>(() => NonceManager.Generate("127.0.0.1"));
        }

        [Fact]
        public void Validate_ThrowsWhenPrivateHashEncoderNotInitialized()
        {
            Assert.Throws<InvalidOperationException>(() => NonceManager.Validate("NjM0MzM4ODg3Nzc1MzEuMzpmZDIxNzllOTUzMDY2ODc2YWQyYjY1NmVmZGJkYTc4MQ==", "127.0.0.1"));
        }
    }


    public class NonceManagerTest
    {
        private static string key = "MyPrivateKey";
        private static string validNonce = "NjM0MzM4ODg3Nzc1MzEuMzpmZDIxNzllOTUzMDY2ODc2YWQyYjY1NmVmZGJkYTc4MQ==";
        private static string ipAddress = "192.168.16.1";

        static NonceManagerTest()
        {
            PrivateHashEncoder.Current = () => new PrivateHashEncoder(key);
        }

        [Fact]
        public void Generate_ThrowsOnNullIpAddress()
        {
            Assert.Throws<ArgumentNullException>(() => NonceManager.Generate(null));
        }

        [Fact]
        public void Generate_ThrowsOnEmptyIpAddress()
        {
            Assert.Throws<ArgumentException>(() => NonceManager.Generate(string.Empty));
        }

        [Fact, FreezeClock]
        public void Generate_CanRoundtripThroughValidate()
        {
            string nonce = NonceManager.Generate(ipAddress);
            Assert.True(NonceManager.Validate(nonce, ipAddress));
        }

        [Fact, FreezeClock]
        public void Generate_ProducesSameNonceAtFrozenTime()
        {
            string firstNonce = NonceManager.Generate(ipAddress);
            string secondNonce = NonceManager.Generate(ipAddress);
            Assert.Same(firstNonce, secondNonce);
        }

        [Fact]
        public void Generate_ProducesDifferentNoncesOverTime()
        {
            string firstNonce = NonceManager.Generate(ipAddress);
            string secondNonce = NonceManager.Generate(ipAddress);
            Assert.NotEqual(firstNonce, secondNonce);
        }

        [Fact]
        public void Validate_ThrowsOnNullIpAddress()
        {
            Assert.Throws<ArgumentNullException>(() => NonceManager.Validate(validNonce, null));
        }

        [Fact]
        public void Validate_ThrowsOnEmptyIpAddress()
        {
            Assert.Throws<ArgumentException>(() => NonceManager.Validate(validNonce, string.Empty));
        }

        [Fact]
        public void Validate_ThrowsOnNullNonce()
        {
            Assert.Throws<ArgumentNullException>(() => NonceManager.Validate(null, ipAddress));
        }

        [Fact]
        public void Validate_ThrowsOnEmptyNonce()
        {
            Assert.Throws<ArgumentException>(() => NonceManager.Validate(string.Empty, ipAddress));
        }

        [Fact]
        public void Validate_MatchesPreCalculatedNonce()
        {
            Assert.True(NonceManager.Validate(validNonce, ipAddress));
        }

        [Fact]
        public void IsStale_ThrowsOnNullNonce()
        {
            Assert.Throws<ArgumentNullException>(() => NonceManager.IsStale(null, TimeSpan.FromSeconds(30)));
        }

        [Fact]
        public void IsStale_ThrowsOnEmptyNonce()
        {
            Assert.Throws<ArgumentException>(() => NonceManager.IsStale(string.Empty, TimeSpan.FromSeconds(30)));
        }

        [Fact, FreezeClock]
        public void IsStale_FalseOnCurrentNonce()
        {
            string nonce = NonceManager.Generate(ipAddress);
            Assert.False(NonceManager.IsStale(nonce, TimeSpan.FromSeconds(20)));
        }

        [Fact]
        public void IsStale_TrueOnZeroSeconds()
        {
            string nonce = NonceManager.Generate(ipAddress);
            Assert.True(NonceManager.IsStale(nonce, TimeSpan.FromSeconds(0)));
        }

        [Fact]
        public void IsStale_TrueOnOldPreCalculatedNonce()
        {
            Assert.True(NonceManager.IsStale(validNonce, TimeSpan.FromSeconds(0)));
        }
    }
}
